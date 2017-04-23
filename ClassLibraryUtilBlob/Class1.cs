using System;
using System.Data.SqlClient;

namespace ClassLibraryUtilBlob {
    /// <summary>
    /// This abstract class contains credentials for connecting
    /// to a database. The class is project specific.
    /// </summary>
    public abstract class DbParams {
        public const string SERVER = "CV-BB-5490\\SQLEXPRESS";
        public const string DB = "employee";
        public const string USER = "niml";
        public const string PASSWORD = "verysecretpassword";
        // Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
    }
    /// <summary>
    /// MSSQLWrapper is a wrapperclass abstracting the SqlConnection
    /// so that programs are facing an RDBMS neutral interface.
    /// MSSQLWrapper is a Singleton.
    /// </summary>
    /// <remarks>
    /// This interface/wrapper is meant to be invariant with respect 
    /// to projects.
    /// The interface inherits the DbParams class containing the
    /// credentials for opening a connection.
    /// </remarks>
    public class MSSQLWrapper : DbParams {
        private static MSSQLWrapper dbh = null;
        private const string CnString = "user id=" + DbParams.USER + ";" +
                            "password=" + DbParams.PASSWORD + ";" +
                            "server=" + DbParams.SERVER + ";" +
                            "Trusted_Connection=yes;" +
                            "database=" + DbParams.DB + ";" +
                            "connection timeout=30";
        private SqlConnection dbcon;
        private SqlDataReader dbReader;
        private SqlCommand dbSQL;

        private MSSQLWrapper() {
            try {
                this.dbcon = new SqlConnection(CnString);
                this.dbReader = null;
                this.dbSQL = null;
            } catch (Exception e) {
                throw e;
            }
        }

        public static MSSQLWrapper getConnection() {
            if (dbh == null) {
                dbh = new MSSQLWrapper();
            }
            return dbh;
        }

        public void open() {
            try {
                dbcon.Open();
            } catch (Exception e) {
                throw e;
            }
        }

        public void close() {
            try {
                dbcon.Close();
            } catch (Exception e) {
                throw e;
            } finally {
                this.dbSQL = null;
            }
        }

        public void startTransaction() {
            this.dbSQL = new SqlCommand("begin transaction;", dbcon);
            this.otherQuery();
        }
        public void commit() {
            this.dbSQL = new SqlCommand("commit transaction;", dbcon);
            this.otherQuery();
        }
        public SqlCommand getCommand() {
            return this.dbSQL;
        }
        public void setCommand(string sql) {
            this.dbSQL = new SqlCommand(sql, dbcon);
        }

        public int getRecentKey() {
            return (int) this.dbSQL.ExecuteScalar();
        }

        public void addParam(SqlParameter sqlp) {
            this.dbSQL.Parameters.Add(sqlp);
        }

        public SqlDataReader select() {
            try {
                this.dbReader = this.dbSQL.ExecuteReader();
            } catch (Exception e) {
                throw e;
            }
            return this.dbReader;
        }

        public void otherQuery() {
            try {
                this.dbSQL.ExecuteNonQuery();
            } catch (Exception e) {
                throw e;
            }
        }

        public void prepare() {
            try {
                this.dbSQL.Prepare();
            } catch (Exception e) {
                throw e;
            }
        }

    }
}