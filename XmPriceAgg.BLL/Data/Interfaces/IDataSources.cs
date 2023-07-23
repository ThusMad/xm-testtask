using XmPriceAgg.DAL.Interfaces;

namespace XmPriceAgg.BLL.Data.Interfaces;

public interface IDataSources
{
    public void AddProvider(IPriceProvider provider);
    public IEnumerable<IPriceProvider> GetProviders();
}