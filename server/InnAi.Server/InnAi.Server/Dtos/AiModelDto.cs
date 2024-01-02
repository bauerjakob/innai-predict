namespace InnAi.Server.Dtos;

public class AiModelDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public bool Default { get; set; }
}