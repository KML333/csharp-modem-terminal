using System.IO.Ports;
using System.Text;

namespace ModemTerminal;

sealed class SerialConnection : IDisposable
{
    private SerialPort? port;

    // Raised on the serial port thread.
    public event Action<byte[]>? DataReceived;

    public bool IsOpen
    {
        get { return port != null && port.IsOpen; }
    }

    public static string[] GetPortNames()
    {
        return SerialPort.GetPortNames();
    }

    public void Open(string portName, int baudRate, int dataBits,
                     Parity parity, StopBits stopBits, Handshake handshake)
    {
        Close();

        SerialPort newPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

        // Modems accept commands only when DTR is active.
        newPort.DtrEnable = true;
        // RTS must be enabled before flow control is set.
        newPort.RtsEnable = true;
        newPort.Handshake = handshake;
        newPort.DataReceived += Port_DataReceived;

        newPort.Open();
        port = newPort;
    }

    public void SendText(string text)
    {
        SendBytes(Encoding.ASCII.GetBytes(text));
    }

    public void SendBytes(byte[] data)
    {
        if (port == null || !port.IsOpen)
            throw new InvalidOperationException("The port is not open.");

        port.Write(data, 0, data.Length);
    }

    public void Close()
    {
        if (port != null)
        {
            port.DataReceived -= Port_DataReceived;
            port.Close();
            port = null;
        }
    }

    public void Dispose()
    {
        Close();
    }

    private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort current = (SerialPort)sender;
        byte[] buffer;

        try
        {
            if (current.BytesToRead == 0)
                return;

            // Read all bytes waiting in the input buffer.
            buffer = new byte[current.BytesToRead];
            int read = current.Read(buffer, 0, buffer.Length);
            Array.Resize(ref buffer, read);
        }
        catch (Exception)
        {
            // The port was closed while reading.
            return;
        }

        DataReceived?.Invoke(buffer);
    }
}
