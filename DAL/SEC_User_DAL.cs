using Hostel_Management_System.DAL;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Docs_Editor.DAL
{
    public class SEC_User_DAL : DAL_Helper
    {
        #region PR_SEC_UserByUN_PS_EM
        public DataTable PR_SEC_UserByUN_PS_EM(string Emial, string Password)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(MyConnectionStr);
                DbCommand cmd = sqlDB.GetStoredProcCommand("PR_SEC_UserByUN_PS_EM");
                sqlDB.AddInParameter(cmd, "@Email", DbType.String, Emial);
                sqlDB.AddInParameter(cmd, "@Password", DbType.String, Password);
                DataTable dt = new DataTable();
                using (IDataReader reader = sqlDB.ExecuteReader(cmd))
                {
                    dt.Load(reader);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }
        #endregion
    }
}
