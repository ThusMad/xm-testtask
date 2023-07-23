using Microsoft.Extensions.Configuration;
using RestSharp;
using XmPriceAgg.DAL.Interfaces;

namespace XmPriceAgg.DAL.Providers;

public abstract class ProviderBase : IPriceProvider
{
    private readonly IConfiguration _configuration;

    protected readonly RestClient RestClient;

    protected ProviderBase(IConfiguration? configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "no configuration provided for provider");

        var baseUrl = configuration.GetValue<string>("baseUrl");
        
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentNullException(nameof(baseUrl), "base url must be set in order to use provider");

        RestClient = new RestClient(new RestClientOptions(baseUrl));
    }

    public abstract Task<float> GetPriceAsync(long timestamp, CancellationToken cancellationToken = default);
    public abstract Task<IEnumerable<float>> GetPriceListAsync(long fromTimestamp, long toTimestamp, CancellationToken cancellationToken = default);
}