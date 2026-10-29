
using System;
using System.IO;
using System.Text;

namespace ModemTerminal;

static class FileSender
{
    public static byte[] BuildPacket(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "File path cannot be empty.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "File does not exist.", filePath);
        }

        string fileName = Path.GetFileName(filePath);

        // The protocol header supports ASCII file names.
        foreach (char character in fileName)
        {
            if (character > 127 || character == '|')
            {
                throw new ArgumentException(
                    "File name must contain only ASCII characters and no pipe symbol.");
            }
        }

        byte[] fileData = File.ReadAllBytes(filePath);

        string header = $"##PLIK##|{fileName}|{fileData.Length}\n";
        byte[] headerData = Encoding.ASCII.GetBytes(header);

        byte[] packet = new byte[headerData.Length + fileData.Length];

        Buffer.BlockCopy(
            headerData, 0, packet, 0, headerData.Length);

        Buffer.BlockCopy(
            fileData, 0, packet, headerData.Length, fileData.Length);

        return packet;
    }
}

