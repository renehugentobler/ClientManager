namespace HSoft.SQL
{

    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Security.Cryptography;
    using System.Collections.Generic;
    using System.Net.Mail;


    using System.Linq;
    using System.Collections;

    public class SqlServer
    {

        SqlConnection sqlconnection = null;
        Boolean IsError = false;

        public void Execute(string ssql)
        {
            if (sqlconnection.State != ConnectionState.Open) 
            { 
                try { sqlconnection.Close(); } catch { }
                if (sqlconnection.State == ConnectionState.Closed)
                {
                    try { sqlconnection.Open(); } catch (SqlException _sex) {throw _sex; }
                }
                else
                {
                    throw new Exception(String.Format("SQL ConnectionState : {1}", sqlconnection.State)); 
                }
            }
            try
            {
                SqlCommand sqlcommand = new SqlCommand(ssql, sqlconnection);
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                sqlcommand.ExecuteNonQuery();
                sqlcommand.Dispose();
                sqldataadapter.Dispose();
            
            }
            catch (SqlException _sex)
            {
                if (IsError) { return; }
                IsError = true;
                try
                {
                    ssql = String.Format("INSERT INTO _auditt_sql(sql) VALUES('{0}')", ssql.Replace("'", "''"));
                    Execute(ssql);
                }
                catch { }
                finally { throw _sex; }
            }
            IsError = false;
        }
        public object ExecuteScalar(string ssql)
        {
            if (sqlconnection.State != ConnectionState.Open)
            {
                try { sqlconnection.Close(); }
                catch { }
                if (sqlconnection.State == ConnectionState.Closed)
                {
                    try { sqlconnection.Open(); }
                    catch (SqlException _sex) { throw _sex; }
                }
                else
                {
                    throw new Exception(String.Format("SQL ConnectionState : {1}", sqlconnection.State));
                }
            }
            try
            {
                SqlCommand sqlcommand = new SqlCommand(ssql, sqlconnection);
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                object res = sqlcommand.ExecuteScalar();
                sqlcommand.Dispose();
                sqldataadapter.Dispose();
                if (res == null) { res = new object(); }

                IsError = false;
                return res;
            }
            catch (SqlException _sex)
            {
                if (IsError) { return new object(); }
                IsError = true;
                try
                {
                    ssql = String.Format("INSERT INTO _auditt_sql(sql) VALUES('{0}')", ssql.Replace("'", "''"));
                    Execute(ssql);
                }
                catch { }
                finally { throw _sex; }
            }
        }
        public object ExecuteScalar2(string ssql)
        {
            if (sqlconnection.State != ConnectionState.Open)
            {
                try { sqlconnection.Close(); }
                catch { }
                if (sqlconnection.State == ConnectionState.Closed)
                {
                    try { sqlconnection.Open(); }
                    catch (SqlException _sex) { throw _sex; }
                }
                else
                {
                    throw new Exception(String.Format("SQL ConnectionState : {1}", sqlconnection.State));
                }
            }
            try
            {
                SqlCommand sqlcommand = new SqlCommand(ssql, sqlconnection);
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                object res = sqlcommand.ExecuteScalar();
                sqlcommand.Dispose();
                sqldataadapter.Dispose();

                IsError = false;
                return res;
            }
            catch (SqlException _sex)
            {
                if (IsError) { return new object(); }
                IsError = true;
                try
                {
                    ssql = String.Format("INSERT INTO _auditt_sql(sql) VALUES('{0}')", ssql.Replace("'", "''"));
                    Execute(ssql);
                }
                catch { }
                finally { throw _sex; }
            }
        }
        public DataTable GetTable(string ssql)
        {
            DataTable datatable = null;
            if (sqlconnection.State != ConnectionState.Open)
            {
                try { sqlconnection.Close(); }
                catch { }
                if (sqlconnection.State == ConnectionState.Closed)
                {
                    try { sqlconnection.Open(); }
                    catch (SqlException _sex) { throw _sex; }
                }
                else
                {
                    throw new Exception(String.Format("SQL ConnectionState : {1}", sqlconnection.State));
                }
            }
            try
            {
                SqlCommand sqlcommand = new SqlCommand(ssql, sqlconnection);
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                DataSet dataset = new DataSet();
                sqldataadapter.Fill(dataset);
                datatable = dataset.Tables[0];
                sqldataadapter.Dispose();
                sqlcommand.Dispose();
                IsError = false;
                return datatable;
            }
            catch (SqlException _sex)
            {
                if (IsError) { return null; }
                IsError = true;
                try
                {
                    ssql = String.Format("INSERT INTO _auditt_sql(sql) VALUES('{0}')", ssql.Replace("'", "''"));
                    Execute(ssql);
                }
                catch { }
                finally { throw _sex; }
            }
        }
        public DataRow GetRow(string ssql)
        {
            return GetTable(ssql).Rows[0];
        }
        public void InsertDataset(string stablename, DataTable dt)
        {
            SqlCommandBuilder sqlcommandbuilder;

            if (sqlconnection.State != ConnectionState.Open)
            {
                try { sqlconnection.Close(); }
                catch { }
                if (sqlconnection.State == ConnectionState.Closed)
                {
                    try { sqlconnection.Open(); }
                    catch (SqlException _sex) { throw _sex; }
                }
                else
                {
                    throw new Exception(String.Format("SQL ConnectionState : {1}", sqlconnection.State));
                }
            }
            try
            {
                DataSet ds_dummy = new DataSet();

                SqlCommand sqlcommand = new SqlCommand(String.Format("SELECT * FROM {0} WHERE 1=2", stablename), sqlconnection);
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                sqldataadapter.SelectCommand = sqlcommand;
                sqldataadapter.Fill(ds_dummy);

                sqlcommandbuilder = new SqlCommandBuilder(sqldataadapter);
                sqldataadapter.InsertCommand = sqlcommandbuilder.GetInsertCommand();

                ds_dummy.Merge(dt);
                sqldataadapter.Update(ds_dummy);

                sqldataadapter.Dispose();
                sqlcommand.Dispose();

            }
            catch (SqlException _sex)
            {
                throw _sex;
            }
        }

        public SqlServer(String s_con)
        {
            sqlconnection = new SqlConnection(s_con);
            sqlconnection.Open();
        }

        public static Guid? GetUserGuid(String suser, String sconn)
        {
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            try
            {
                return Guid.Parse(_sql.ExecuteScalar(String.Format("SELECT id FROM _usercredentials WHERE username = '{0}'", suser)).ToString());
            }
            catch
            {
                return null;
            }
        }

        public static String StringSHA256(String str)
        {
            byte[] salt = { 1, 2, 4, 8, 16, 32 };
            byte[] value = System.Text.Encoding.ASCII.GetBytes(str);
            List<byte> list1 = new List<byte>(salt);
            List<byte> list2 = new List<byte>(value);
            list1.AddRange(list2);
            byte[] sum2 = list1.ToArray();
            byte[] hash = new SHA256Managed().ComputeHash(sum2);
            return Convert.ToBase64String(hash);
        }

        public static void AddUser(String suser, String spass, DateTime dtime, Guid gguid)
        {
            HSoft.SQL.SqlServer _sql = new SqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["ClientManager"].ConnectionString);
            String ssql = String.Format("INSERT INTO _usercredentials(username,password,createdate,creadedby) VALUES ('{0}','{1}','{2}','{3}')", suser, spass, dtime, gguid);
            _sql.Execute(ssql);
        }
        public static Guid? Validate_Login(String username, String password, String s_con)
        {
            byte[] salt = { 1, 2, 4, 8, 16, 32 };
            byte[] value = System.Text.Encoding.ASCII.GetBytes (password);
            List<byte> list1 = new List<byte>(salt);
            List<byte> list2 = new List<byte>(value);
            list1.AddRange(list2);
            byte[] sum2 = list1.ToArray();
            byte[] hash = new SHA256Managed().ComputeHash(sum2);
            String spass = Convert.ToBase64String(hash);

            value = System.Text.Encoding.ASCII.GetBytes (username);
            list1 = new List<byte>(salt);
            list2 = new List<byte>(value);
            list1.AddRange(list2);
            sum2 = list1.ToArray();
            hash = new SHA256Managed().ComputeHash(sum2);
            String sname = Convert.ToBase64String(hash);

            HSoft.SQL.SqlServer _sql = new SqlServer(s_con);

            try
            {
                return Guid.Parse(_sql.ExecuteScalar(String.Format("SELECT UserId " +
                                                                   "  FROM _usercredentials " +
                                                                   " WHERE username = '{0}' " +
                                                                   "   AND password = '{1}' " +
                                                                   "   AND IsDeleted = 0 " +
                                                                   "   AND IsActive = 1 " +
                                                                   "   AND UserId IN (SELECT Id FROM Employee WHERE isdeleted = 0 AND isactive=1) " +
                                                                   "", sname, spass)).ToString());
            }
            catch
            {
                return null;
            }
        }

        public void Close()
        {
            if (sqlconnection.State != ConnectionState.Open)
            {
                try { sqlconnection.Close(); }
                catch { }
                if (sqlconnection.State == ConnectionState.Closed)
                {
                }
                else
                {
                    throw new Exception(String.Format("SQL ConnectionState : {1}", sqlconnection.State));
                }
            }
        }

        ~SqlServer() { this.Close(); }

    }
}
