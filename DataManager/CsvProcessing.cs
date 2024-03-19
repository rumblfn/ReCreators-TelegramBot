using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace DataManager;

/// <summary>
/// Class for processing files in csv format (Read and Write). 
/// </summary>
public class CsvProcessing
{
    // Csv reader and writer configuration.
    private readonly CsvConfiguration _csvConfig = new (CultureInfo.InvariantCulture)
    {
        Delimiter = ";",
    };
    
    /// <summary>
    /// Read stream with data in csv format.
    /// Handle it with <see cref="CsvReader"/> from CsvHelper.
    /// </summary>
    /// <param name="stream">Input stream.</param>
    /// <returns>List of <see cref="ReCreator"/> (read data).</returns>
    public IEnumerable<ReCreator> Read(FileStream stream)
    {
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _csvConfig);
        
        return csv.GetRecords<ReCreator>().ToList();
    }
    
    /// <summary>
    /// Write collection to stream in csv format.
    /// </summary>
    /// <param name="reCreators">Enumerable collection of data.</param>
    /// <returns>Stream with data in csv format.</returns>
    public Stream Write(IEnumerable<ReCreator> reCreators)
    {
        MemoryStream stream = new ();
        
        // Stream should be opened to get data from it: leaveOpen: true
        using (StreamWriter writer = new (stream, leaveOpen: true))
        using (CsvWriter csv = new (writer, _csvConfig))
        {
            csv.WriteRecords(reCreators);
            writer.Flush();
        }

        stream.Position = 0;
        return stream;
    }
}