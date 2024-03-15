using System.Text.Json;
using System.Text.Encodings.Web;

namespace DataManager;

public class JsonProcessing
{
    private readonly JsonWriterOptions _jsonWriterOptions = new ()
    {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };
    
    public IEnumerable<ReCreator> Read(FileStream stream)
    {
        return JsonSerializer.Deserialize<List<ReCreator>>(stream) 
               ?? new List<ReCreator>();
    }

    public FileStream Write(string path, IEnumerable<ReCreator> reCreators)
    {
        var fileStream = new FileStream(path, FileMode.Create);
        using (var writer = new Utf8JsonWriter(fileStream, _jsonWriterOptions))
        {
            JsonSerializer.Serialize(writer, reCreators);
        }

        fileStream.Position = 0;
        return fileStream;
    }
}