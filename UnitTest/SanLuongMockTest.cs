using Data;
using Infrastructure;
using Moq;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class SanLuongMockTest
    {
        private SanLuongBTPService _SanLuongBTPService;
        private Mock<ISanLuongBTPRepository> _mockSanLuong;
        public static object[][] toong = {
            new object[] { new DateTime(2020,05,8),100},
            new object[] { new DateTime(2020,05,9),100},
            new object[] { new DateTime(2020,05,10),200}
        };

        public SanLuongMockTest()
        {
            _mockSanLuong = new Mock<ISanLuongBTPRepository>();
            _SanLuongBTPService = new SanLuongBTPService(_mockSanLuong.Object);
        }
        [Theory]
        [MemberData(nameof(toong))]
        public void GetTest(DateTime d, int ex)
        {
            //A
            SanLuongBTP sanLuongBTP = new SanLuongBTP()
            {
                Id = Guid.NewGuid(),
                SoLuong = 100
            };
            _mockSanLuong.Setup(m => m.GetTest()).Returns(sanLuongBTP);
            //Ac
            SanLuongBTP actual = _SanLuongBTPService.GetTest(d);
            //Ex
            Assert.Equal(ex, actual.SoLuong);
        }
        [Fact]
        public void GetAllTest()
        {
            //Arrange
            IEnumerable<SanLuongBTP> sanLuongBTPs = new List<SanLuongBTP>()
            {
                new SanLuongBTP(){ Id = Guid.NewGuid(),SoLuong = 100},
                new SanLuongBTP(){ Id = Guid.NewGuid(),SoLuong = 200},
                new SanLuongBTP(){ Id = Guid.NewGuid(),SoLuong = 101}
            };
            _mockSanLuong.Setup(m => m.GetAllTest()).Returns(sanLuongBTPs);
            //Actual
            IEnumerable<SanLuongBTP> actual = _SanLuongBTPService.GetAllTest();
            int ex = 2;
            //Assert
            Assert.Equal(ex, actual.Count());
            Assert.NotNull(actual);
        }
        [Theory]
        [InlineData("SP1", 0)]
        [InlineData("SP2", 2)]
        public void GetAllTestBy(string masp, int ex)
        {
            //Arrange
            IEnumerable<SanLuongBTP> sanLuongBTPs = new List<SanLuongBTP>()
            {
                new SanLuongBTP(){Id = Guid.NewGuid(),MaSP = "SP1",SoLuong = 100},
                new SanLuongBTP(){Id = Guid.NewGuid(),MaSP = "SP2",SoLuong = 101},
                new SanLuongBTP(){Id = Guid.NewGuid(),MaSP = "SP2",SoLuong = 103}
            };
            _mockSanLuong.Setup(m => m.GetAllTestBy(It.IsAny<string>())).Returns(sanLuongBTPs);
            //Actual
            IEnumerable<SanLuongBTP> actual = _SanLuongBTPService.GetAllTestBy(masp);
            Assert.Equal(ex, actual.Count());
        }
    }

}
