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
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, _csvConfig);
        
        return csv.GetRecords<ReCreator>().ToList();
    }

    public FileStream Write(string path, IEnumerable<ReCreator> reCreators)
    {
        var fileStream = new FileStream(path, FileMode.Create);
        using (var writer = new StreamWriter(fileStream))
        using (var csv = new CsvWriter(writer, _csvConfig))
        {
            csv.WriteRecords(reCreators);
        }

        fileStream.Position = 0;
        return fileStream;
    }
}