using Data;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDiscountDate
    {
        DateTime GetDiscountDate();
    }
    public class SanLuongBTPService
    {
        private ISanLuongBTPRepository _sanLuongBTPRepository;
        public SanLuongBTPService(ISanLuongBTPRepository sanLuongBTPRepository)
        {
            _sanLuongBTPRepository = sanLuongBTPRepository;
        }
        public SanLuongBTP GetTest(DateTime discountDate)
        {
            SanLuongBTP sanLuongBTP = _sanLuongBTPRepository.GetTest();
            if (discountDate.DayOfWeek == (int)DayOfWeek.Sunday)
            {
                sanLuongBTP.SoLuong += sanLuongBTP.SoLuong;
            }
            return sanLuongBTP;
        }
        public IEnumerable<SanLuongBTP> GetAllTest()
        {
            var result = _sanLuongBTPRepository.GetAllTest();
            return result.Where(s => s.SoLuong % 2 == 0);
        }
        public IEnumerable<SanLuongBTP> GetAllTestBy(string masp)
        {
            var result = _sanLuongBTPRepository.GetAllTestBy(masp);
            return result.Where(s => s.SoLuong % 2 != 0 &&  s.MaSP.Contains(masp));
        }
        /// <summary>
        /// Predicate: tìm những sản lượng có ngày thuộc thứ 3, 5, 7, CN
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<SanLuongBTP> GetByPredicateTest(Expression<Func<SanLuongBTP, bool>> predicate)
        {
            var result = _sanLuongBTPRepository.GetByPredicateTest(predicate);
            return result.Where(s => s.SoLuong % 2 != 0);
        }
    }
}
