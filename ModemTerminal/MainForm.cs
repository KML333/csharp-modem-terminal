using System.IO.Ports;
using System.Text;

namespace ModemTerminal;

public partial class MainForm : Form
{
    private readonly SerialConnection connection = new SerialConnection();
    private readonly FileReceiver receiver = new FileReceiver();

    public MainForm()
    {
        InitializeComponent();

        // Port settings that can be chosen in the window.
        comboBoxBaudRate.Items.AddRange(new object[] { 300, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 });
        comboBoxDataBits.Items.AddRange(new object[] { 5, 6, 7, 8 });
        comboBoxParity.Items.AddRange(new object[] { Parity.None, Parity.Odd, Parity.Even });
        comboBoxStopBits.Items.AddRange(new object[] { StopBits.One, StopBits.OnePointFive, StopBits.Two });
        comboBoxHandshake.Items.AddRange(new object[] { Handshake.None, Handshake.XOnXOff, Handshake.RequestToSend });

        comboBoxBaudRate.SelectedItem = 9600;
        comboBoxDataBits.SelectedItem = 8;
        comboBoxParity.SelectedItem = Parity.None;
        comboBoxStopBits.SelectedItem = StopBits.One;
        comboBoxHandshake.SelectedItem = Handshake.RequestToSend;

        connection.DataReceived += Connection_DataReceived;
        receiver.FileSaved += Receiver_FileSaved;

        buttonRefresh.Click += (sender, e) => RefreshPorts();
        buttonConnect.Click += ButtonConnect_Click;
        buttonSend.Click += (sender, e) => SendLine();
        buttonEscape.Click += ButtonEscape_Click;
        buttonSendFile.Click += ButtonSendFile_Click;
        buttonClear.Click += (sender, e) => textBoxTerminal.Clear();

        // Enter sends the typed line.
        textBoxInput.KeyDown += (sender, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                SendLine();
            }
        };

        // The port is closed before the window disappears.
        FormClosing += (sender, e) => connection.Close();

        RefreshPorts();
        UpdateControls(false);
    }

    private void RefreshPorts()
    {
        comboBoxPort.Items.Clear();
        comboBoxPort.Items.AddRange(SerialConnection.GetPortNames());

        if (comboBoxPort.Items.Count > 0)
            comboBoxPort.SelectedIndex = 0;
        else
            AppendInfo("No COM ports found");
    }

    private void ButtonConnect_Click(object? sender, EventArgs e)
    {
        if (connection.IsOpen)
        {
            connection.Close();
            AppendInfo("Disconnected");
            UpdateControls(false);
            return;
        }

        if (comboBoxPort.SelectedItem == null)
        {
            ShowError("Select a COM port first.");
            return;
        }

        string portName = (string)comboBoxPort.SelectedItem;
        int baudRate = (int)comboBoxBaudRate.SelectedItem!;
        int dataBits = (int)comboBoxDataBits.SelectedItem!;
        Parity parity = (Parity)comboBoxParity.SelectedItem!;
        StopBits stopBits = (StopBits)comboBoxStopBits.SelectedItem!;
        Handshake handshake = (Handshake)comboBoxHandshake.SelectedItem!;

        try
        {
            connection.Open(portName, baudRate, dataBits, parity, stopBits, handshake);
            AppendInfo("Connected to " + portName + " (" + baudRate + " baud)");
            UpdateControls(true);
            textBoxInput.Focus();
        }
        catch (Exception error)
        {
            ShowError("Failed to open the port.", error);
        }
    }

    private void SendLine()
    {
        if (!connection.IsOpen)
            return;

        try
        {
            // Modem commands end with carriage return.
            connection.SendText(textBoxInput.Text + "\r");

            if (checkBoxLocalEcho.Checked)
                AppendTerminal(textBoxInput.Text + "\r");

            textBoxInput.Clear();
        }
        catch (Exception error)
        {
            ShowError("Failed to send data.", error);
        }
    }

    private async void ButtonEscape_Click(object? sender, EventArgs e)
    {
        SetSendingEnabled(false);

        try
        {
            // "+++" needs about 1 second of silence before and after.
            await Task.Delay(1100);
            connection.SendText("+++");
            await Task.Delay(1100);
            AppendInfo("Escape sequence sent");
        }
        catch (Exception error)
        {
            ShowError("Failed to send the escape sequence.", error);
        }

        SetSendingEnabled(connection.IsOpen);
    }

    private async void ButtonSendFile_Click(object? sender, EventArgs e)
    {
        OpenFileDialog dialog = new OpenFileDialog();

        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        SetSendingEnabled(false);
        AppendInfo("Sending file: " + Path.GetFileName(dialog.FileName));

        try
        {
            byte[] packet = FileSender.BuildPacket(dialog.FileName);

            // Sending can be slow, so it runs in the background.
            await Task.Run(() => connection.SendBytes(packet));
            AppendInfo("File sent");
        }
        catch (Exception error)
        {
            ShowError("Failed to send the file.", error);
        }

        SetSendingEnabled(connection.IsOpen);
    }

    // Runs on the serial port thread, so the window is updated with BeginInvoke.
    private void Connection_DataReceived(byte[] data)
    {
        try
        {
            // File bytes are saved by the receiver; only text is returned.
            byte[] text = receiver.Process(data);

            if (text.Length > 0)
                BeginInvoke(new Action(() => AppendTerminal(Encoding.Latin1.GetString(text))));
        }
        catch (Exception error)
        {
            BeginInvoke(new Action(() => ShowError("Failed to process received data.", error)));
        }
    }

    // Runs on the serial port thread.
    private void Receiver_FileSaved(string path, long size)
    {
        BeginInvoke(new Action(() => AppendInfo("File received: " + path + " (" + size + " bytes)")));
    }

    private void AppendTerminal(string text)
    {
        // TextBox needs \r\n; modems send \r, \n or \r\n.
        text = text.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\n", "\r\n");
        textBoxTerminal.AppendText(text);
    }

    private void AppendInfo(string message)
    {
        textBoxTerminal.AppendText("\r\n[" + message + "]\r\n");
    }

    private void UpdateControls(bool open)
    {
        // Port settings can be changed only when disconnected.
        comboBoxPort.Enabled = !open;
        buttonRefresh.Enabled = !open;
        comboBoxBaudRate.Enabled = !open;
        comboBoxDataBits.Enabled = !open;
        comboBoxParity.Enabled = !open;
        comboBoxStopBits.Enabled = !open;
        comboBoxHandshake.Enabled = !open;

        buttonConnect.Text = open ? "Disconnect" : "Connect";
        SetSendingEnabled(open);
    }

    private void SetSendingEnabled(bool enabled)
    {
        textBoxInput.Enabled = enabled;
        buttonSend.Enabled = enabled;
        buttonEscape.Enabled = enabled;
        buttonSendFile.Enabled = enabled;
    }

    private void ShowError(string message, Exception? error = null)
    {
        if (IsDisposed)
            return;

        if (error != null)
            message += "\n" + error.Message;

        MessageBox.Show(this, message, "Modem Terminal", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}