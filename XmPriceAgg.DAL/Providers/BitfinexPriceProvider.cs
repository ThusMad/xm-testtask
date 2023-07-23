using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using XmPriceAgg.DAL.Models;

namespace XmPriceAgg.DAL.Providers;

public class BitfinexPriceProvider : ProviderBase
{
    private const string KlineEndpoint = "v2/candles/trade:1h:tBTCUSD/hist";
    private const string KlineRequestFormat = "?start={0}&end={1}";

    public BitfinexPriceProvider(IConfiguration? configuration) : base(configuration)
    {
    }

    private static IEnumerable<BitfinexKlineResponse> ParseResponse(string json)
    {
        try
        {
            var list = JsonConvert.DeserializeObject<List<JArray>>(json);
            if (list == null)
                return new List<BitfinexKlineResponse>();

            return list.Select(klineRaw => new BitfinexKlineResponse()
            {
                Timestamp = klineRaw[0].Value<long>(),
                Open = klineRaw[1].Value<int>(),
                Close = klineRaw[2].Value<int>(),
                High = klineRaw[3].Value<int>(),
                Low = klineRaw[4].Value<int>(),
                Volume = klineRaw[5].Value<float>(),
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public override async Task<float> GetPriceAsync(long timestamp, CancellationToken cancellationToken = default)
    {
        var resource = new StringBuilder(KlineEndpoint);
        resource.AppendFormat(KlineRequestFormat, timestamp, timestamp + TimeSpan.FromHours(1).TotalMilliseconds);
        resource.Append("&limit=1");
        var request = new RestRequest(resource.ToString());
        var response = await RestClient.ExecuteAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ErrorMessage, response.ErrorException);

        if (string.IsNullOrEmpty(response.Content))
            return 0;

        var responseData = ParseResponse(response.Content).ToList();

        return !responseData.Any() ? 0 : responseData.Last().Close;
    }

    public override async Task<IEnumerable<float>> GetPriceListAsync(long fromTimestamp, long toTimestamp, CancellationToken cancellationToken = default)
    {
        var resource = new StringBuilder(KlineEndpoint);
        resource.AppendFormat(KlineRequestFormat, fromTimestamp, toTimestamp);

        var request = new RestRequest(resource.ToString());
        var response = await RestClient.ExecuteAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ErrorMessage, response.ErrorException);

        if (string.IsNullOrEmpty(response.Content))
            return new List<float>();

        var responseData = ParseResponse(response.Content).ToList();

        return !responseData.Any() ? new List<float>() : responseData.Select(x => (float)x.Close);
    }
}