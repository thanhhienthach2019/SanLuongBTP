using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace Services
{
    public class ImportExcel
    {
        public static DataTable ConvertExcelToDataTable(string path)
        {
            DataTable dtResult = null;
            int totalSheet = 0;
            using (OleDbConnection objConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';"))
            {
                objConn.Open();
                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                DataTable dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = string.Empty;
                if (dt != null)
                {
                    var tmpTbl = (from dataRow in dt.AsEnumerable()
                                  where !dataRow["TABLE_NAME"].ToString().Contains("FilterDatabase")
                                  select dataRow).CopyToDataTable();
                    dt = tmpTbl;
                    totalSheet = dt.Rows.Count;
                    sheetName = dt.Rows[0]["TABLE_NAME"].ToString();
                }
                cmd.Connection = objConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                dbDataAdapter = new OleDbDataAdapter(cmd);
                dbDataAdapter.Fill(ds, "excelData");
                dtResult = ds.Tables["excelData"];
                objConn.Close();
                return dtResult;
            }
        }
    }
}