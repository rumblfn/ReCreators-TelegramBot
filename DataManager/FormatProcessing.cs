namespace DataManager;

public class FormatProcessing
{
    private readonly CsvProcessing _csvProcessor = new ();
    private readonly JsonProcessing _jsonProcessor = new ();

    public Stream WriteToStream(string path, FormatEnum type)
    {
        List<ReCreator> reCreators = ReadFile(path) ?? new List<ReCreator>();
        switch (type)
        {
            case FormatEnum.Csv:
                CsvProcessing csvProcessing = new();
                return csvProcessing.Write(reCreators);
            case FormatEnum.Json:
                JsonProcessing jsonProcessing = new();
                return jsonProcessing.Write(reCreators);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    public List<ReCreator>? ReadFile(string path)
    {
        using FileStream fs = new (path, FileMode.Open);
        return ProcessFile(fs, path);
    }

    public List<ReCreator>? ProcessFile(FileStream fileStream, string path)
    {
        try
        {
            try
            {
                fileStream.Position = 0;
                return _jsonProcessor.Read(fileStream).ToList();
            }
            catch (Exception)
            {
                fileStream.Position = 0;
                IEnumerable<ReCreator> reCreators = _csvProcessor.Read(fileStream);

                // Save csv as json.
                Stream jsonFs = _jsonProcessor.Write(reCreators);
                SaveStreamToFile(jsonFs, path);
                
                return _jsonProcessor.Read(jsonFs).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex}");
        }

        return null;
    }

    public void SaveStreamToFile(Stream stream, string filePath)
    {
        using FileStream fileStream = File.Create(filePath);
        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
    }
}