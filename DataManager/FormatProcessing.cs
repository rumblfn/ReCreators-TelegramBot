namespace DataManager;

public class FormatProcessing
{
    private readonly CsvProcessing _csvProcessor = new ();
    private readonly JsonProcessing _jsonProcessor = new ();

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
                FileStream jsonFs = _jsonProcessor.Write(path, reCreators);
                return _jsonProcessor.Read(jsonFs).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex}");
        }

        return null;
    }
}