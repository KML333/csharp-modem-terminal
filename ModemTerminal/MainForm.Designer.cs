namespace ModemTerminal
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            textBoxTerminal = new TextBox();
            textBoxInput = new TextBox();
            buttonSend = new Button();
            labelPort = new Label();
            comboBoxPort = new ComboBox();
            buttonRefresh = new Button();
            labelBaudRate = new Label();
            comboBoxBaudRate = new ComboBox();
            labelDataBits = new Label();
            comboBoxDataBits = new ComboBox();
            labelParity = new Label();
            comboBoxParity = new ComboBox();
            labelStopBits = new Label();
            comboBoxStopBits = new ComboBox();
            labelHandshake = new Label();
            comboBoxHandshake = new ComboBox();
            buttonConnect = new Button();
            buttonEscape = new Button();
            buttonSendFile = new Button();
            buttonClear = new Button();
            checkBoxLocalEcho = new CheckBox();
            SuspendLayout();

            textBoxTerminal.BackColor = Color.Black;
            textBoxTerminal.ForeColor = Color.LightGreen;
            textBoxTerminal.Font = new Font("Consolas", 10F);
            textBoxTerminal.Location = new Point(12, 12);
            textBoxTerminal.Size = new Size(700, 526);
            textBoxTerminal.Multiline = true;
            textBoxTerminal.ReadOnly = true;
            textBoxTerminal.ScrollBars = ScrollBars.Vertical;
            textBoxTerminal.Name = "textBoxTerminal";

            textBoxInput.Font = new Font("Consolas", 10F);
            textBoxInput.Location = new Point(12, 552);
            textBoxInput.Size = new Size(600, 23);
            textBoxInput.Name = "textBoxInput";

            buttonSend.Location = new Point(618, 550);
            buttonSend.Size = new Size(94, 27);
            buttonSend.Text = "Send";
            buttonSend.Name = "buttonSend";

            labelPort.AutoSize = true;
            labelPort.Location = new Point(730, 12);
            labelPort.Text = "Port";
            labelPort.Name = "labelPort";

            comboBoxPort.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPort.Location = new Point(730, 32);
            comboBoxPort.Size = new Size(168, 23);
            comboBoxPort.Name = "comboBoxPort";

            buttonRefresh.Location = new Point(904, 31);
            buttonRefresh.Size = new Size(84, 25);
            buttonRefresh.Text = "Refresh";
            buttonRefresh.Name = "buttonRefresh";

            labelBaudRate.AutoSize = true;
            labelBaudRate.Location = new Point(730, 66);
            labelBaudRate.Text = "Baud rate";
            labelBaudRate.Name = "labelBaudRate";

            comboBoxBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxBaudRate.Location = new Point(730, 86);
            comboBoxBaudRate.Size = new Size(258, 23);
            comboBoxBaudRate.Name = "comboBoxBaudRate";

            labelDataBits.AutoSize = true;
            labelDataBits.Location = new Point(730, 120);
            labelDataBits.Text = "Data bits";
            labelDataBits.Name = "labelDataBits";

            comboBoxDataBits.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDataBits.Location = new Point(730, 140);
            comboBoxDataBits.Size = new Size(258, 23);
            comboBoxDataBits.Name = "comboBoxDataBits";

            labelParity.AutoSize = true;
            labelParity.Location = new Point(730, 174);
            labelParity.Text = "Parity";
            labelParity.Name = "labelParity";

            comboBoxParity.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxParity.Location = new Point(730, 194);
            comboBoxParity.Size = new Size(258, 23);
            comboBoxParity.Name = "comboBoxParity";

            labelStopBits.AutoSize = true;
            labelStopBits.Location = new Point(730, 228);
            labelStopBits.Text = "Stop bits";
            labelStopBits.Name = "labelStopBits";

            comboBoxStopBits.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxStopBits.Location = new Point(730, 248);
            comboBoxStopBits.Size = new Size(258, 23);
            comboBoxStopBits.Name = "comboBoxStopBits";

            labelHandshake.AutoSize = true;
            labelHandshake.Location = new Point(730, 282);
            labelHandshake.Text = "Flow control";
            labelHandshake.Name = "labelHandshake";

            comboBoxHandshake.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxHandshake.Location = new Point(730, 302);
            comboBoxHandshake.Size = new Size(258, 23);
            comboBoxHandshake.Name = "comboBoxHandshake";

            buttonConnect.Location = new Point(730, 342);
            buttonConnect.Size = new Size(258, 35);
            buttonConnect.Text = "Connect";
            buttonConnect.Name = "buttonConnect";

            buttonEscape.Location = new Point(730, 392);
            buttonEscape.Size = new Size(258, 35);
            buttonEscape.Text = "+++ (Command Mode)";
            buttonEscape.Name = "buttonEscape";

            buttonSendFile.Location = new Point(730, 437);
            buttonSendFile.Size = new Size(258, 35);
            buttonSendFile.Text = "Send File";
            buttonSendFile.Name = "buttonSendFile";

            buttonClear.Location = new Point(730, 482);
            buttonClear.Size = new Size(258, 35);
            buttonClear.Text = "Clear Terminal";
            buttonClear.Name = "buttonClear";

            checkBoxLocalEcho.AutoSize = true;
            checkBoxLocalEcho.Location = new Point(730, 527);
            checkBoxLocalEcho.Text = "Local echo";
            checkBoxLocalEcho.Name = "checkBoxLocalEcho";

            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 590);
            Controls.Add(textBoxTerminal);
            Controls.Add(textBoxInput);
            Controls.Add(buttonSend);
            Controls.Add(labelPort);
            Controls.Add(comboBoxPort);
            Controls.Add(buttonRefresh);
            Controls.Add(labelBaudRate);
            Controls.Add(comboBoxBaudRate);
            Controls.Add(labelDataBits);
            Controls.Add(comboBoxDataBits);
            Controls.Add(labelParity);
            Controls.Add(comboBoxParity);
            Controls.Add(labelStopBits);
            Controls.Add(comboBoxStopBits);
            Controls.Add(labelHandshake);
            Controls.Add(comboBoxHandshake);
            Controls.Add(buttonConnect);
            Controls.Add(buttonEscape);
            Controls.Add(buttonSendFile);
            Controls.Add(buttonClear);
            Controls.Add(checkBoxLocalEcho);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Modem Terminal";
            Name = "MainForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxTerminal;
        private TextBox textBoxInput;
        private Button buttonSend;
        private Label labelPort;
        private ComboBox comboBoxPort;
        private Button buttonRefresh;
        private Label labelBaudRate;
        private ComboBox comboBoxBaudRate;
        private Label labelDataBits;
        private ComboBox comboBoxDataBits;
        private Label labelParity;
        private ComboBox comboBoxParity;
        private Label labelStopBits;
        private ComboBox comboBoxStopBits;
        private Label labelHandshake;
        private ComboBox comboBoxHandshake;
        private Button buttonConnect;
        private Button buttonEscape;
        private Button buttonSendFile;
        private Button buttonClear;
        private CheckBox checkBoxLocalEcho;
    }
}