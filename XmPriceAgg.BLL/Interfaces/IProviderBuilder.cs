using Microsoft.Extensions.Configuration;
using XmPriceAgg.BLL.Data.Interfaces;

namespace XmPriceAgg.BLL.Interfaces;

public interface IProviderBuilder
{
    IProviderBuilder BuildBitfinexProvider(IConfiguration configuration);

    IProviderBuilder BuildBitstampProvider(IConfiguration configuration);

    IDataSources GetSources();
}