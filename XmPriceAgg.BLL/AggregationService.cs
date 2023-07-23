using Microsoft.Extensions.Logging;
using XmPriceAgg.BLL.Data.Interfaces;
using XmPriceAgg.BLL.Interfaces;
using XmPriceAgg.DAL.Repositories.Interfaces;

namespace XmPriceAgg.BLL;

public class  AggregationService : IAggregationService
{
    private readonly ILogger<AggregationService> _logger;
    private readonly IDataSources _sources;
    private readonly IAggregationAlgorithm _aggregationAlgorithm;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public AggregationService(ILogger<AggregationService> logger, IDataSources sources, IAggregationAlgorithm aggregationAlgorithm, IUnitOfWorkFactory unitOfWorkFactory)
    {
        _logger = logger;
        _sources = sources;
        _aggregationAlgorithm = aggregationAlgorithm;
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    private async Task<IEnumerable<float>> GetAggPrice(long timestamp)
    {
        _logger.LogInformation("Retrieving prices from external sources");

        var prices = new List<float>();
        var providers = _sources.GetProviders().ToList();

        if (!providers.Any())
        {
            _logger.LogWarning("External sources not configured!");
            return prices;
        }

        _logger.LogInformation("Asking for prices from [{0}] sources", providers.Count);
        await Task.WhenAll(providers.Select(provider => Task.Run(async () =>
        {
            var price = await provider.GetPriceAsync(timestamp);
            prices.Add(price);
        })).ToArray());

        return prices;
    }

    public async Task<float> RetrievePriceAsync(long timestamp)
    {
        using var unitOfWork = _unitOfWorkFactory.Create();

        _logger.LogInformation("Looking for cached price for timestamp : {timestamp}", timestamp);
        var storedPrice = await unitOfWork.AggregationRepository.GetStoredPriceAsync(timestamp);

        if (storedPrice != null)
        {
            _logger.LogInformation("Cached price for timestamp : {timestamp} found!", timestamp);
            return storedPrice.Price;
        }

        _logger.LogInformation("There is no cached prices for timestamp : {timestamp}", timestamp);

        var prices = (await GetAggPrice(timestamp)).ToList();

        _logger.LogInformation("Fetched [{0}] prices from external sources", prices.Count);

        if (prices.Count == 0) return 0;

        var aggregated = _aggregationAlgorithm.Aggregate(prices);

        _logger.LogInformation("Prices was aggregated, aggregated price: {aggregated}", aggregated);
        _logger.LogInformation("Caching aggregated price!");

        await unitOfWork.AggregationRepository.InsertStoredPriceAsync(timestamp, aggregated);
        unitOfWork.Commit();

        _logger.LogInformation("Price was cached, aggregated price: {aggregated}, timestamp: {timestamp}", aggregated, timestamp);

        return aggregated;
    }

    public async Task<IEnumerable<float>> RetrievePricesAsync(long timestampFrom, long timestampTo)
    {
        using var unitOfWork = _unitOfWorkFactory.Create();

        _logger.LogInformation("Looking for cached price for range : {timestampFrom}-{timestampTo}", timestampFrom, timestampTo);
        var storedPrice = await unitOfWork.AggregationRepository.GetStoredPricesAsync(timestampFrom, timestampTo);
        _logger.LogInformation("Found : {0} entries", storedPrice.Count());

        return storedPrice.Select(x => x.Price);
    }
}