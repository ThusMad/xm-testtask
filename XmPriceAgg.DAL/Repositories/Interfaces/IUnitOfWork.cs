namespace XmPriceAgg.DAL.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IAggregationRepository AggregationRepository { get; }

    void Commit();
}