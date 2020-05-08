using Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class SanLuongBTPRepository : RepositoryBase<SanLuongBTP>, ISanLuongBTPRepository
    {
        public SanLuongBTPRepository(KHKDEntities entities) : base(entities)
        {
        }
        public void AddMulti(IEnumerable<SanLuongBTP> sanLuongBTPs)
        {
            _dbSet.AddRange(sanLuongBTPs);
        }

        public IEnumerable<SanLuongBTP> GetAllTest()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SanLuongBTP> GetAllTestBy(string masp)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SanLuongBTP> GetByPredicateTest(Expression<Func<SanLuongBTP, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public SanLuongBTP GetTest()
        {
            throw new NotImplementedException();
        }
    }
}