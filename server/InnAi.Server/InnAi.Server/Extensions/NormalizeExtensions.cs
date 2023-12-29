namespace InnAi.Server.Extensions;

public static class NormalizeExtensions
{
    public static double Normalize(this double value, int min, int max)
    {
        return (value - min) / ((double) max - min);
    }

    public static double Denormalize(this double value, int min, int max)
    {
        return value * (max - min) + min;
    }
}