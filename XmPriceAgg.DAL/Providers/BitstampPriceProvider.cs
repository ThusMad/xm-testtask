using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Text;
using XmPriceAgg.DAL.Models;

namespace XmPriceAgg.DAL.Providers;

public class BitstampPriceProvider : ProviderBase
{
    private const string KlineEndpoint = "api/v2/ohlc/btcusd/";
    private const string KlineRequestFormat = "?step=3600&start={0}&end={1}";
    public BitstampPriceProvider(IConfiguration? configuration) : base(configuration)
    {
    }

    public override async Task<float> GetPriceAsync(long timestamp, CancellationToken cancellationToken = default)
    {
        var resource = new StringBuilder(KlineEndpoint);
        resource.AppendFormat(KlineRequestFormat, timestamp / 1000, (timestamp + TimeSpan.FromHours(1).TotalMilliseconds) / 1000);
        resource.Append("&limit=1");
        var request = new RestRequest(resource.ToString());
        var response = await RestClient.ExecuteAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException(response.ErrorMessage, response.ErrorException);

        if (string.IsNullOrEmpty(response.Content))
            return 0;

        var responseData = JsonConvert.DeserializeObject<BitstampKlineResponse>(response.Content);

        return responseData == null ? 0 : responseData.Data.Ohlc.Last().Close;
    }

    public override async Task<IEnumerable<float>> GetPriceListAsync(long fromTimestamp, long toTimestamp, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}