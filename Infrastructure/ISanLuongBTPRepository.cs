using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface ISanLuongBTPRepository
    {
        SanLuongBTP GetTest();
        IEnumerable<SanLuongBTP> GetAllTest();
        IEnumerable<SanLuongBTP> GetAllTestBy(string masp);
        IEnumerable<SanLuongBTP> GetByPredicateTest(Expression<Func<SanLuongBTP, bool>> predicate);
    }
}
