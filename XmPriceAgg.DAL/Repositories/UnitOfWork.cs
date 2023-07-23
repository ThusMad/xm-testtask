using System.Data;
using XmPriceAgg.DAL.Repositories.Interfaces;

namespace XmPriceAgg.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    private IAggregationRepository? _aggregationRepository;

    public UnitOfWork(IDbConnection connection)
    {
        _connection = connection;
        _connection.Open();
        _transaction = _connection.BeginTransaction();
    }

    public IAggregationRepository AggregationRepository =>
        _aggregationRepository ??= new AggregationRepository(_transaction);

    public void Commit()
    {
        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = _connection?.BeginTransaction();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}