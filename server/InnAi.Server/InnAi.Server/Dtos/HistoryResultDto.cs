namespace InnAi.Server.Dtos;

public class HistoryResultDto
{
    public double AverageDeviation { get; set; }
    public double PercentageDeviation { get; set; }
    public double[] PredictionValues { get; set; }
    public double[] ActualValues { get; set; }
}