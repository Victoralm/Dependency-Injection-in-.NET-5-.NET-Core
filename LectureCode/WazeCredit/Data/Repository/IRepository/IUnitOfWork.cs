using System;

namespace WazeCredit.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICreditApplicationRepository CreditApplicationRepository {  get; }

        void Save();
    }
}
