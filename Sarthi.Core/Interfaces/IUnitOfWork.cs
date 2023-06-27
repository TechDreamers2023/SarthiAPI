namespace Sarthi.Core.Interfaces
{
    public interface IUnitOfWork  
    { 
        ICommonRepository CommonRepository { get; }
        IRequestRepository RequestRepository { get; }
        IVendorRepository VendorRepository { get; }
        void Commit();
    }
}
