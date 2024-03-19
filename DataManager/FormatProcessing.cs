namespace DataManager;

/// <summary>
/// Class for processing files in csv or json format.
/// </summary>
public class FormatProcessing
{
    private readonly CsvProcessing _csvProcessor = new ();
    private readonly JsonProcessing _jsonProcessor = new ();

    /// <summary>
    /// Method for writing data to stream in specified format.
    /// </summary>
    /// <param name="path">Path to data.</param>
    /// <param name="type">Output format type: csv or json.</param>
    /// <returns>Stream with data.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If specified invalid data format.</exception>
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

    /// <summary>
    /// Method for reading data from file.
    /// </summary>
    /// <param name="path">Path to file with data.</param>
    /// <returns>List of data.</returns>
    public List<ReCreator>? ReadFile(string path)
    {
        using FileStream fs = new (path, FileMode.Open);
        return ProcessFile(fs, path);
    }

    /// <summary>
    /// Method for reading data from stream and store it in specified path.
    /// </summary>
    /// <param name="fileStream">Input stream.</param>
    /// <param name="path">Output path.</param>
    /// <returns>List of data.</returns>
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

    /// <summary>
    /// Method for saving data from stream to file.
    /// </summary>
    /// <param name="stream">Input stream.</param>
    /// <param name="filePath">Output path.</param>
    public static void SaveStreamToFile(Stream stream, string filePath)
    {
        using FileStream fileStream = File.Create(filePath);
        stream.Seek(0, SeekOrigin.Begin);
        stream.CopyTo(fileStream);
    }
}