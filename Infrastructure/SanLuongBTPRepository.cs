using Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class SanLuongBTPRepository : RepositoryBase<SanLuongBTP>
    {
        public SanLuongBTPRepository(KHKDEntities entities) : base(entities)
        {
        }
        public void AddMulti(IEnumerable<SanLuongBTP> sanLuongBTPs)
        {
            _dbSet.AddRange(sanLuongBTPs);
        }
    }
}