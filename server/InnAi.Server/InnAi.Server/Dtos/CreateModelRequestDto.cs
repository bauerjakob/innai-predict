namespace InnAi.Server.Dtos;

public class CreateModelRequestDto
{
    public Guid ExternalId { get; set; }
    public string Name { get; set; }
    public bool Default { get; set; }
    public int PrecipitationMapSize { get; set; }
    public int NumberOfInnLevels { get; set; }
}