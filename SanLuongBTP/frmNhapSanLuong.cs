using Data;
using Entities;
using Infrastructure;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SanLuongBTP
{
    public partial class frmNhapSanLuong : Form
    {
        private UnitOfWork _unitOfWork;
        private APIProvider _apiProvider;
        private int checkUpdateStatus;
        private Guid sanLuongID;

        private enum UpdateStatus
        {
            NONE = 0,
            INSERT = 1,
            UPDATE = 2
        }

        public frmNhapSanLuong()
        {
            InitializeComponent();
            _unitOfWork = new UnitOfWork(new KHKDEntities());
            _apiProvider = new APIProvider();
            checkUpdateStatus = (int)UpdateStatus.NONE;
        }

        private void FrmNhapSanLuong_Load(object sender, EventArgs e)
        {
            dtpTuNgay.Value = dtpDenNgay.Value = dtpNgay.Value = dtpDefDate.Value = DateTime.Today;
            LoadDtgv(dtpDefDate.Value);
            EnableTextBox();
        }

        private void LoadDtgv(DateTime defDate)
        {
            grcSanLuongBTP.DataSource = _unitOfWork.SanLuongBTPRepository.GetByCondition(e => e.Ngay == defDate).ToList();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void EnableTextBox(bool maSPStatus = false, bool maDVStatus = false, bool ngayStatus = false, bool donGiaStatus = false)
        {
            txtMaSP.Enabled = maSPStatus;
            txtMaDV.Enabled = maDVStatus;
            dtpNgay.Enabled = ngayStatus;
            numSoLuong.Enabled = donGiaStatus;
        }

        private void ClearTextBox()
        {
            txtMaSP.Clear();
            txtMaDV.Clear();
            numSoLuong.Value = 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (checkUpdateStatus == (int)UpdateStatus.NONE)
            {
                checkUpdateStatus = (int)UpdateStatus.INSERT;
                EnableTextBox(true, true, true, true);
                ClearTextBox();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (sanLuongID != Guid.Empty && checkUpdateStatus == (int)UpdateStatus.NONE)
            {
                if (MessageBox.Show(string.Format("Xóa mã có id {0}?", sanLuongID), "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    _unitOfWork.SanLuongBTPRepository.DeleteById(sanLuongID);
                    _unitOfWork.Commit();
                    LoadDtgv(dtpDefDate.Value);
                }
            }
        }

        private void grvSanLuongBTP_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (checkUpdateStatus == (int)UpdateStatus.NONE)
            {
                sanLuongID = (Guid)grvSanLuongBTP.GetRowCellValue(grvSanLuongBTP.FocusedRowHandle, grvSanLuongBTP.Columns["Id"]);
                txtMaSP.Text = grvSanLuongBTP.GetRowCellValue(grvSanLuongBTP.FocusedRowHandle, grvSanLuongBTP.Columns["MaSP"]).ToString();
                txtMaDV.Text = grvSanLuongBTP.GetRowCellValue(grvSanLuongBTP.FocusedRowHandle, grvSanLuongBTP.Columns["MS_DV"]).ToString();
                dtpNgay.Value = (DateTime)grvSanLuongBTP.GetRowCellValue(grvSanLuongBTP.FocusedRowHandle, grvSanLuongBTP.Columns["Ngay"]);
                numSoLuong.Value = (decimal)grvSanLuongBTP.GetRowCellValue(grvSanLuongBTP.FocusedRowHandle, grvSanLuongBTP.Columns["SoLuong"]);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (sanLuongID != Guid.Empty && checkUpdateStatus == (int)UpdateStatus.NONE)
            {
                checkUpdateStatus = (int)UpdateStatus.UPDATE;
                EnableTextBox(true, true, true, true);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            checkUpdateStatus = (int)UpdateStatus.NONE;
            EnableTextBox();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (checkUpdateStatus != (int)UpdateStatus.NONE)
            {
                Data.SanLuongBTP sanLuongBTP = new Data.SanLuongBTP();
                sanLuongBTP.MaSP = txtMaSP.Text;
                sanLuongBTP.MS_DV = txtMaDV.Text;
                sanLuongBTP.Ngay = dtpNgay.Value;
                sanLuongBTP.SoLuong = numSoLuong.Value;
                if (checkUpdateStatus == (int)UpdateStatus.INSERT)
                {
                    sanLuongBTP.Id = Guid.NewGuid();
                    _unitOfWork.SanLuongBTPRepository.Add(sanLuongBTP);
                }
                else if (checkUpdateStatus == (int)UpdateStatus.UPDATE)
                {
                    sanLuongBTP.Id = sanLuongID;
                    _unitOfWork.SanLuongBTPRepository.Update(sanLuongBTP);
                }
                _unitOfWork.Commit();
                LoadDtgv(dtpDefDate.Value);
                checkUpdateStatus = (int)UpdateStatus.NONE;
                EnableTextBox();
            }
        }

        private async void btnSanLuongSAP_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value;
            DateTime denNgay = dtpDenNgay.Value;

            _unitOfWork.SanLuongBTPRepository.Delete(x => x.Ngay >= tuNgay && x.Ngay <= denNgay && x.IsSapData);

            IEnumerable<SanLuongNhapKhoSAP> sanLuongNhapKhoSAPs = await GetSLSAP(tuNgay, denNgay);

            IEnumerable<Data.SanLuongBTP> sanLuongBTPs = ConvertSapData(sanLuongNhapKhoSAPs);

            ExportExcel(ConvertData.ConvertModelToDataTable<Data.SanLuongBTP>(sanLuongBTPs.ToList()));
            _unitOfWork.Commit();
            LoadDtgv(dtpDefDate.Value);
        }

        private void ExportExcel(DataTable data)
        {
            string fileName;
            SaveFileDialog fileDialog = new SaveFileDialog() { Filter = "Excel files (*.xlsx)|*.xlsx" };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = fileDialog.FileName;
                CreateExcelFile.CreateExcelDocument(data, fileName);
            }
        }

        private IEnumerable<Data.SanLuongBTP> ConvertSapData(IEnumerable<SanLuongNhapKhoSAP> sanLuongNhapKhoSAPs)
        {
            IEnumerable<DonViBTPSap> donViBTPSaps = _unitOfWork.DonViBTPSapRepository.GetAll();
            return sanLuongNhapKhoSAPs
                            .Join(donViBTPSaps,
                                  sap => sap.ZCHUYEN,
                                  dv => dv.MS_DV_Sap,
                                  (sap, dv) => new Data.SanLuongBTP()
                                  {
                                      MaSP = sap.MAKTX,
                                      MS_DV = dv.MS_DV,
                                      Ngay = DateTime.Parse(sap.BUDAT),
                                      SoLuong = sap.WEMNG,
                                      IsSapData = true
                                  }).ToList();
        }

        private async Task<IEnumerable<SanLuongNhapKhoSAP>> GetSLSAP(DateTime tuNgay, DateTime denNgay)
        {
            return await _apiProvider.WebAPIGetAsync<IEnumerable<SanLuongNhapKhoSAP>>($"SAP/GetSLNhapKhoBTP/8000/{tuNgay.ToString("yyyyMMdd")}/{denNgay.ToString("yyyyMMdd")}");
        }

        private void dtpDefDate_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = sender as DateTimePicker;
            LoadDtgv(dtp.Value);
        }

        private void btnGetFilePath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog();
            txtFilePath.Text = openFileDialog.FileName;
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilePath.Text))
            {
                DataTable data = ImportExcel.ConvertExcelToDataTable(txtFilePath.Text);
                IEnumerable<Data.SanLuongBTP> sanLuongBTPs = ConvertDatatableToObj(data);
                _unitOfWork.SanLuongBTPRepository.AddMulti(sanLuongBTPs);
                _unitOfWork.Commit();
                LoadDtgv(dtpDefDate.Value);
            }
        }

        private IEnumerable<Data.SanLuongBTP> ConvertDatatableToObj(DataTable data)
        {
            foreach (DataRow rows in data.Rows)
            {
                Data.SanLuongBTP sanLuongBTP = new Data.SanLuongBTP();
                sanLuongBTP.Id = Guid.NewGuid();
                sanLuongBTP.MaSP = rows[0].ToString();
                sanLuongBTP.MS_DV = rows[1].ToString();
                sanLuongBTP.Ngay = Convert.ToDateTime(rows[2]);
                sanLuongBTP.SoLuong = Convert.ToDecimal(rows[3]);
                sanLuongBTP.IsSapData = Convert.ToBoolean(rows[4]);
                yield return sanLuongBTP;
            }
        }
    }
}