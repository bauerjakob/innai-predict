namespace InnAi.Server.Data.Entities;

public class AiModelResult
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public int AiModelId { get; set; }

    public double[] PredictionValues { get; set; } = null!;

    public double[] ActualValues { get; set; } = null!;
    
    
    public double AverageDeviation { get; set; }
    
    public double PercentageDeviation { get; set; }

    public virtual AiModel? AiModel { get; set; }
}