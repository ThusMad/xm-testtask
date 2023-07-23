using Microsoft.Extensions.Configuration;
using XmPriceAgg.BLL.Data;
using XmPriceAgg.BLL.Data.Interfaces;
using XmPriceAgg.BLL.Interfaces;
using XmPriceAgg.DAL.Providers;

namespace XmPriceAgg.BLL;

public class ProviderBuilder : IProviderBuilder
{
    private IDataSources _dataSources = new DataSources();

    public ProviderBuilder()
    {
        Reset();
    }

    public void Reset()
    {
        _dataSources = new DataSources();
    }

    public IProviderBuilder BuildBitfinexProvider(IConfiguration configuration)
    {
        _dataSources.AddProvider(new BitfinexPriceProvider(configuration));
        return this;
    }

    public IProviderBuilder BuildBitstampProvider(IConfiguration configuration)
    {
        _dataSources.AddProvider(new BitstampPriceProvider(configuration));
        return this;
    }

    public IDataSources GetSources()
    {
        var result = _dataSources;

        Reset();

        return result;
    }
}