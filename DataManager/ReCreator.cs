using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace DataManager;

public class ReCreator
{
    public string Name { get; init; }
    public string RankYear { get; init; }
    public string MainObjects { get; init; }
    public string Workplace { get; init; }
    public string Photo { get; init; }
    
    [Name("global_id")]
    [JsonPropertyName("global_id")]
    public string GlobalId { get; init; }

    public ReCreator()
    {
        Name = string.Empty;
        RankYear = string.Empty;
        MainObjects = string.Empty;
        Workplace = string.Empty;
        Photo = string.Empty;
        GlobalId = string.Empty;
    }

    [JsonConstructor]
    public ReCreator(string name, string rankYear, string mainObjects, string workplace, string photo, string globalId)
    {
        Name = name;
        RankYear = rankYear;
        MainObjects = mainObjects;
        Workplace = workplace;
        Photo = photo;
        GlobalId = globalId;
    }
}