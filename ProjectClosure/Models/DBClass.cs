using Focus.DatabaseFactory;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace ProjectClosure.Models
{
    public class DBClass
    {
        public static int GetExecute(string strInsertOrUpdateQry, int CompId, ref string error)
        {
            try
            {
                Database obj = Focus.DatabaseFactory.DatabaseWrapper.GetDatabase(CompId);
                return (obj.ExecuteNonQuery(CommandType.Text, strInsertOrUpdateQry));
            }
            catch (Exception e)
            {
                error = e.Message;
                return 0;
            }
        }
        public static DataSet GetData(string strSelQry, int CompId, ref string error)
        {
            try
            {
                Database obj = Focus.DatabaseFactory.DatabaseWrapper.GetDatabase(CompId);
                return (obj.ExecuteDataSet(CommandType.Text, strSelQry));
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }

        public static DataSet GetDataSet(string strselQry, int companyId, ref string logText)
        {
            DataSet dataset = null;
            try
            {
                Database _db = DatabaseWrapper.GetDatabase(companyId);

                using (var con = _db.CreateConnection())
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = strselQry;
                        cmd.CommandTimeout = 0;
                        dataset = _db.ExecuteDataSet(cmd);
                    }
                }


                return dataset;

            }
            catch (Exception e)
            {
                SetLog(DateTime.Now.ToString() + " GetData :" + e.Message);
                return null;
            }

        }


        public static void SetLog(string content)
        {
            StreamWriter objSw = null;
            try
            {
                string AppLocation = "";
                AppLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                string folderName = AppLocation + "\\LogFiles";
                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                string sFilePath2 = folderName + "\\PrjProjectClosureLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                objSw = new StreamWriter(sFilePath2, true);
                objSw.WriteLine(DateTime.Now.ToString() + " " + content + Environment.NewLine);
            }
            catch (Exception ex)
            {
                //SetLog("Error -" + ex.Message);
            }
            finally
            {
                if (objSw != null)
                {
                    objSw.Flush();
                    objSw.Dispose();
                }
            }
        }
    }
}