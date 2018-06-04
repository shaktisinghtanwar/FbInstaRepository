using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Fb_InstaWpf.Model;


namespace Fb_InstaWpf.Helper
{
    class SqLiteHelper
    {
        string appStartupPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/DBFbInstaComment/FbInstaCommentDb.s3db";
        public String DbConnection { get; set; }
        /// <summary>
        ///     Default Constructor for SQLiteDatabase Class.
        /// </summary>
        public SqLiteHelper()
        {
            DbConnection = String.Format("Data Source={0}", appStartupPath);
        }

        public int ExecuteNonQuery(StringBuilder sql)
        {
            int rowsUpdated;
            SQLiteConnection cnn = new SQLiteConnection(DbConnection);
            cnn.Open();
            using (var transaction = cnn.BeginTransaction())
            {

                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandTimeout = int.MaxValue;
                mycommand.CommandText = sql.ToString();
                rowsUpdated = mycommand.ExecuteNonQuery();

                transaction.Commit();
                cnn.Close();
            }


            return rowsUpdated;
        }
        /// <summary>
        ///     Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        public SqLiteHelper(string inputFile)
        {
            DbConnection = String.Format("Data Source={0}", ConstantClass.ConnectrionString);
        }

        /// <summary>
        ///     Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        public SqLiteHelper(Dictionary<String, String> connectionOpts)
        {
            String str = "";
            foreach (KeyValuePair<String, String> row in connectionOpts)
            {
                str += String.Format("{0}={1}; ", row.Key, row.Value);
            }
            str = str.Trim().Substring(0, str.Length - 1);
            DbConnection = str;
        }

        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection(DbConnection);
                cnn.Open();

                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                cnn.Close();
            }
            catch (Exception e)
            {
               
            }
            return dt;
        }

        public int ExecuteNonQuery(string sql)
        {
            int rowsUpdated;
            SQLiteConnection cnn = new SQLiteConnection(DbConnection);
            cnn.Open();
            using (var transaction = cnn.BeginTransaction())
            {
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                rowsUpdated = mycommand.ExecuteNonQuery();
                transaction.Commit();
                cnn.Close();
            }
            return rowsUpdated;
        }

        public string ExecuteScalar(string sql)
        {
            SQLiteConnection cnn = new SQLiteConnection(DbConnection);
            cnn.Open();
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            object value = mycommand.ExecuteScalar();
            cnn.Close();
            if (value != null)
            {
                return value.ToString();
            }
            return "";
        }

        public bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }
        public bool Delete(String tableName, String where)
        {
            Boolean returnCode = true;
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
            }
            catch (Exception fail)
            {
                
                returnCode = false;
            }
            return returnCode;
        }
        public bool Insert(String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value.Replace("'", "''"));
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception fail)
            {
               
                returnCode = false;
            }
            return returnCode;
        }

    

        public bool ClearDB()
        {
            DataTable tables;
            try
            {
                tables = this.GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearTable(String table)
        {
            try
            {

                this.ExecuteNonQuery(String.Format("delete from {0};", table));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IsTableExists(String tableName)
        {
            string count = "0";
            if (DbConnection == default(string))
                return false;
            using (SQLiteConnection cnn = new SQLiteConnection(DbConnection))
            {
                try
                {
                    cnn.Open();
                    if (tableName == null || cnn.State != ConnectionState.Open)
                    {
                        return false;
                    }
                    String sql = string.Format("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name ='{0}'", tableName);
                    count = ExecuteScalar(sql);
                }
                finally
                {
                    // Close the database connection  
                    if ((cnn != null) && (cnn.State != ConnectionState.Open))
                        cnn.Close();
                }
            }
            return Convert.ToInt32(count) > 0;
        }

        public bool IsTableColoumnExits(string tableName, string coloumnName)
        {
            var table = new DataTable();
            try
            {
                using (var con = new SQLiteConnection(DbConnection))
                {
                    using (var cmd = new SQLiteCommand("PRAGMA table_info(" + tableName + ");"))
                    {
                        cmd.Connection = con;
                        cmd.Connection.Open();
                        SQLiteDataAdapter adp = null;
                        adp = new SQLiteDataAdapter(cmd);
                        adp.Fill(table);
                        con.Close();
                    }
                }
                var list = table.AsEnumerable().Select(r => r["name"].ToString()).ToList();
                if (list.Count > 0)
                {
                    return list.Any(t => (t.Equals(coloumnName)));
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool AddColoumn(string tableName, string columnName, string type)
        {
            try
            {
                int executeNonQuery = this.ExecuteNonQuery(String.Format("ALTER TABLE {0} add COLUMN {1} {2};", tableName, columnName, type));
                if (executeNonQuery > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;

        }

    }

}
