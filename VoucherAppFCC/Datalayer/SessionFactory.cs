using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;
using VoucherAppFCC.Helpers;

namespace VoucherAppFCC.Datalayer
{
    public class SessionFactory : IDisposable
    {
        public string ConnectionString { get; set; }
        SqlConnection _Conn;
        public string CurrentSql { get; set; }
 
      
        public SessionFactory()
        {
            AppSettings AppSettings_ = new AppSettings();
             
              ConnectionString = AppSettings_.getvalue("ConnectionString");
            AppSettings_ = null;
        }

        public void Dispose()
        {
            if (Conn != null)
                Conn.Dispose();
        }

        public SqlConnection Conn
        {
            get
            {
                if (_Conn == null)
                {
                    _Conn = new SqlConnection(ConnectionString);
                }
                return _Conn;
            }
            set
            {
                _Conn = value;
            }
        }

        public void Exec(string sql = "", DynamicParameters paramters = null)
        {
           
            sql = string.IsNullOrEmpty(sql) ? CurrentSql : sql;
            Conn.Open();
            Conn.Execute(sql, paramters);
        }

        public IEnumerable<T> Exec<T>(string sql = "", DynamicParameters paramters = null)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    sql = string.IsNullOrEmpty(sql) ? CurrentSql : sql;
                    Conn.Open();
                    var result = Conn.Query<T>(sql, paramters);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Execint(string sql = "", DynamicParameters paramters = null)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    sql = string.IsNullOrEmpty(sql) ? CurrentSql : sql; 
                    var result = Conn.Query<int>(sql, paramters).Single();


                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
       

    }
}
