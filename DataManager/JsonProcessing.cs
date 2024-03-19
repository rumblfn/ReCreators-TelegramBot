using System.Text.Json;
using System.Text.Encodings.Web;

namespace DataManager;

/// <summary>
/// Class for processing files in json format (Read and Write). 
/// </summary>
public class JsonProcessing
{
    // Json writer configuration.
    private readonly JsonWriterOptions _jsonWriterOptions = new ()
    {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };
    
    /// <summary>
    /// Method for reading data from stream.
    /// </summary>
    /// <param name="stream">Input stream.</param>
    /// <returns>Enumerable collection of <see cref="ReCreator"/></returns>
    public IEnumerable<ReCreator> Read(Stream stream)
    {
        stream.Position = 0;
        return JsonSerializer.Deserialize<List<ReCreator>>(stream) 
               ?? new List<ReCreator>();
    }

    /// <summary>
    /// Method for writing data to stream.
    /// </summary>
    /// <param name="reCreators">Enumerable collection of <see cref="ReCreator"/></param>
    /// <returns>Output stream with data.</returns>
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