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
    
    public IEnumerable<ReCreator> Read(Stream stream)
    {
        stream.Position = 0;
        return JsonSerializer.Deserialize<List<ReCreator>>(stream) 
               ?? new List<ReCreator>();
    }

    public Stream Write(IEnumerable<ReCreator> reCreators)
    {
        MemoryStream stream = new ();
        using (var writer = new Utf8JsonWriter(stream, _jsonWriterOptions))
        {
            JsonSerializer.Serialize(writer, reCreators);
        }

        stream.Position = 0;
        return stream;
    }
}