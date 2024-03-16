using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace DataManager;

public class CsvProcessing
{
    private readonly CsvConfiguration _csvConfig = new (CultureInfo.InvariantCulture)
    {
        Delimiter = ";",
    };
    
    public IEnumerable<ReCreator> Read(FileStream stream)
    {
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _csvConfig);
        
        return csv.GetRecords<ReCreator>().ToList();
    }
    
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