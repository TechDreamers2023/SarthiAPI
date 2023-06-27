using System.Data;
using Sarthi.Core.Interfaces;

namespace Sarthi.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public ICommonRepository CommonRepository { get; }
        public IRequestRepository RequestRepository { get; }
        public IVendorRepository VendorRepository { get; }
        IDbTransaction _dbTransaction;

        public UnitOfWork(IDbTransaction dbTransaction,ICommonRepository commonRepository, IRequestRepository requestRepository, 
            IVendorRepository vendorRepository)
        {
            _dbTransaction = dbTransaction;
            CommonRepository = commonRepository;
            RequestRepository = requestRepository;
            VendorRepository = vendorRepository;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
