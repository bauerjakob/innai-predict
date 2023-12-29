using System.Globalization;
using ImageMagick;
using InnAi.Server.Clients;
using InnAi.Server.Extensions;

namespace InnAiServer.Services;

public class InnAiService : IInnAiService
{
    private readonly IInnLevelClient _innLevelClient;
    private readonly IPrecipitationMapClient _precipitationMapClient;
    private readonly IInnAiPredictionClient _predictionClient;

    public InnAiService(IInnLevelClient innLevelClient, IPrecipitationMapClient precipitationMapClient, IInnAiPredictionClient predictionClient)
    {
        _innLevelClient = innLevelClient;
        _precipitationMapClient = precipitationMapClient;
        _predictionClient = predictionClient;
    }
    
    public Task<double[]> PredictCurrentAsync()
    {
        return PredictAsync(DateTime.UtcNow);
    }

    public async Task<double[]> PredictAsync(DateTime timestamp)
    {
        var time = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, 0, 0);

        var innLevels = await GetNormalizedInnLevelsAsync(time);

        var precipitationMap = await GetNormalizedPrecipitationMapAsync(time);

        var modelInput = precipitationMap.Concat(innLevels).ToArray();

        var predicitons = await _predictionClient.PredictAsync(modelInput);

        var denormalizedPredictions = predicitons.Select(x => x.Denormalize(0, 255));

        return denormalizedPredictions.ToArray();
    }

    public async Task<double[]> GetActualAsync(DateTime dateTime)
    {
        var result = new double[24];
        for (var i = 0; i < result.Length; i++)
        {
            var innlevels = await _innLevelClient.GetAsync(dateTime);
            result[i] = innlevels.First();
            dateTime += TimeSpan.FromHours(1);
        }

        return result;
    }

    public async Task<double[]> GetNormalizedPrecipitationMapAsync(DateTime time)
    {
        var precipitationMap = await _precipitationMapClient.GetAsync(time);
        var imageValues = ImageToValues(precipitationMap);
        var imageValuesReduced = ReduceImageValues(imageValues, x => (int)x.Average(), 32);

        var size = imageValuesReduced.GetLength(0);
        var precipitationMapNormalized = new List<double>(size * size);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                precipitationMapNormalized.Add(((double) imageValuesReduced[i, j]).Normalize(0, 255));
            }
        }

        return precipitationMapNormalized.ToArray();
    }

    private async Task<IEnumerable<double>> GetNormalizedInnLevelsAsync(DateTime time)
    {
        var innLevels = await _innLevelClient.GetAsync(time);
        var innLevelsNormalized = innLevels.Select(x => x.Normalize(0, 500));
        return innLevelsNormalized;
    }

    private int[,] ImageToValues(byte[] imageData)
    {
        var image = new MagickImage(imageData, MagickFormat.Png);
        
        var pixels = image.GetPixels();
        var width = image.Width;
        var height = image.Height;
        
        var values = new int[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pixel = pixels.GetPixel(x, y);
                var color = pixel.ToColor();
                var hexColor = color?.ToHexString();
                var value = ReadValueAsync(hexColor);
                values[y, x] = value;
            }
        }

        return values;
    }

    private int ReadValueAsync(string hexColor)
    {
        var hexColorExtended = $"{hexColor}{(hexColor.Length == 7 ? "ff" : string.Empty)}";

        var hexColorUnextended = hexColorExtended.Substring(1, 2);

        return int.Parse(hexColorUnextended, NumberStyles.HexNumber);  
    }


    private int[,] ReduceImageValues(int[,] imageData, Func<int[], int> selector, int factor)
    {
        var size = imageData.GetLength(0) / factor;

        var ret = new int[size, size];

        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                List<int> values = new();
                for (var x = 0; x < factor; x++)
                {
                    for (var y = 0; y < factor; y++)
                    { 
                        var value = imageData[i * factor + x, j * factor + y];
                        values.Add(value);
                    }
                }

                ret[i, j] = selector(values.ToArray());
            }
        }

        return ret;
    }

    private double[,] NormalizeData(int[,] data)
    {
        int min = 0;
        int max = 255;
        
        var size = data.GetLength(0);
        var ret = new double[size, size];
        
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                var value = data[i, j];

                ret[i, j] = ((double)value - min) / ((double)max - min);
            }
        }

        return ret;
    }
}