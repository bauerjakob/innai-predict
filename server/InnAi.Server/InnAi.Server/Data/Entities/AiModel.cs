namespace InnAi.Server.Data.Entities;

public class AiModel
{
    public int Id { get; set; }
    
    public Guid ExternalId { get; set; }

    public string Name { get; set; } = null!;
    public bool Default { get; set; }

    public int PrecipitationMapSize { get; set; }
    
    public int NumberOfInnLevels { get; set; }

    public virtual IEnumerable<AiModelResult>? Results { get; set; }
}