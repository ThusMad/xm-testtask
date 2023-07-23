using XmPriceAgg.BLL.Data.Interfaces;
using XmPriceAgg.DAL.Interfaces;

namespace XmPriceAgg.BLL.Data;

public class DataSources : IDataSources
{
    private List<IPriceProvider> _providers = new List<IPriceProvider>();

    public void AddProvider(IPriceProvider provider)
    {
        _providers.Add(provider);
    }

    public IEnumerable<IPriceProvider> GetProviders()
    {
        return _providers;
    }
}