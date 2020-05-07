using Data;
using System;

namespace Infrastructure
{
    public class UnitOfWork
    {
        private KHKDEntities _entities;
        private bool disposed;
        public SanLuongBTPRepository SanLuongBTPRepository { get; private set; }
        public DonViBTPSapRepository DonViBTPSapRepository { get; private set; }

        public UnitOfWork(KHKDEntities entities)
        {
            _entities = entities;
            disposed = false;
            SanLuongBTPRepository = new SanLuongBTPRepository(_entities);
            DonViBTPSapRepository = new DonViBTPSapRepository(_entities);
        }

        public void Commit()
        {
            _entities.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _entities.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose();
        }
    }
}