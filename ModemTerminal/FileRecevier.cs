
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace ModemTerminal;

sealed class FileReceiver
{
    private static readonly byte[] Marker =
        Encoding.ASCII.GetBytes("##PLIK##|");

    private const int MaxHeaderLength = 512;

    private readonly List<byte> markerBuffer = new();
    private readonly List<byte> headerBuffer = new();

    private bool readingHeader = false;

    private MemoryStream? fileBuffer;
    private string? fileName;
    private int remainingBytes;

    public event Action<string, long>? FileSaved;

    public bool IsReceiving =>
        readingHeader || fileBuffer != null;

    public byte[] Process(byte[] incoming)
    {
        ArgumentNullException.ThrowIfNull(incoming);

        List<byte> terminalData = new();

        foreach (byte value in incoming)
        {
            // File content is being received.
            if (fileBuffer != null)
            {
                fileBuffer.WriteByte(value);
                remainingBytes--;

                if (remainingBytes == 0)
                {
                    CompleteFile();
                }

                continue;
            }

            // File header is being received.
            if (readingHeader)
            {
                if (value == (byte)'\n')
                {
                    if (!TryStartFile())
                    {
                        terminalData.AddRange(Marker);
                        terminalData.AddRange(headerBuffer);
                        terminalData.Add(value);
                    }

                    headerBuffer.Clear();
                    readingHeader = false;
                }
                else if (headerBuffer.Count >= MaxHeaderLength)
                {
                    // Invalid or excessively long header.
                    terminalData.AddRange(Marker);
                    terminalData.AddRange(headerBuffer);
                    terminalData.Add(value);

                    headerBuffer.Clear();
                    readingHeader = false;
                }
                else
                {
                    headerBuffer.Add(value);
                }

                continue;
            }

            // Search for the file marker in ordinary data.
            markerBuffer.Add(value);

            while (markerBuffer.Count > 0 &&
                   !MatchesMarkerPrefix())
            {
                terminalData.Add(markerBuffer[0]);
                markerBuffer.RemoveAt(0);
            }

            if (markerBuffer.Count == Marker.Length)
            {
                markerBuffer.Clear();
                readingHeader = true;
            }
        }

        return terminalData.ToArray();
    }

    private bool MatchesMarkerPrefix()
    {
        for (int i = 0; i < markerBuffer.Count; i++)
        {
            if (markerBuffer[i] != Marker[i])
            {
                return false;
            }
        }

        return true;
    }

    private bool TryStartFile()
    {
        // Header must contain printable ASCII characters.
        foreach (byte value in headerBuffer)
        {
            if (value < 32 || value > 126)
            {
                return false;
            }
        }

        string header =
            Encoding.ASCII.GetString(headerBuffer.ToArray());

        int separator = header.LastIndexOf('|');

        if (separator <= 0 || separator == header.Length - 1)
        {
            return false;
        }

        string receivedName = header[..separator];
        string sizeText = header[(separator + 1)..];

        // Reject directory paths and invalid file names.
        if (string.IsNullOrWhiteSpace(receivedName) ||
            receivedName == "." ||
            receivedName == ".." ||
            receivedName.EndsWith('.') ||
            receivedName.EndsWith(' ') ||
            receivedName.Contains('/') ||
            receivedName.Contains('\\') ||
            receivedName.Contains('|') ||
            receivedName.IndexOfAny(
                Path.GetInvalidFileNameChars()) >= 0 ||
            Path.GetFileName(receivedName) != receivedName)
        {
            return false;
        }

        if (!int.TryParse(
                sizeText,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out int size))
        {
            return false;
        }

        fileName = receivedName;
        remainingBytes = size;
        fileBuffer = new MemoryStream();

        // An empty file is also a valid file.
        if (remainingBytes == 0)
        {
            CompleteFile();
        }

        return true;
    }

    private void CompleteFile()
    {
        if (fileBuffer == null || fileName == null)
        {
            return;
        }

        MemoryStream completedFile = fileBuffer;
        string completedName = fileName;

        fileBuffer = null;
        fileName = null;
        remainingBytes = 0;

        try
        {
            string directory = Path.Combine(
                AppContext.BaseDirectory, "odebrane_pliki");

            Directory.CreateDirectory(directory);

            string path = Path.Combine(
                directory, completedName);

            // Do not overwrite previously received files.
            using (FileStream output = new FileStream(
                path, FileMode.CreateNew, FileAccess.Write))
            {
                completedFile.Position = 0;
                completedFile.CopyTo(output);
            }

            FileSaved?.Invoke(path, completedFile.Length);
        }
        finally
        {
            completedFile.Dispose();
        }
    }
}

