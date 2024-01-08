using System.Runtime.InteropServices.JavaScript;

namespace InnAi.Server.Dtos;

public record EvaluateMonthDto(EvaluateDayDto[] days, double sumDeviation,  double averageDeviatoin);
public record EvaluateDayDto(DateTime day, double averageDeviation);
