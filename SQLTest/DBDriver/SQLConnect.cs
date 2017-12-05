using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

/*
Data Source=(local)\SQLEXPRESS;Initial Catalog=itent;Persist Security Info=True;User ID=taurus;Password=acetaurus
*/

namespace SQLTest
{
    /// <summary>
    /// Класс работы с БД
    /// </summary>
    public class SQLConnect : IDisposable
    {


        string _connectionString = @"Data Source=(local)\SQLEXPRESS;Initial Catalog=itent;Persist Security Info=True;User ID=taurus;Password=acetaurus";
        /// <summary>
        /// Строка соединения с БД
        /// </summary>
        public string ConnectionString { get => _connectionString; set => _connectionString = value; }


        /// <summary>
        /// Конструктор
        /// </summary>
        public SQLConnect()
        {
        }

        /// <summary>
        /// Тест соединения
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            var retValue = false;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    conn.InfoMessage += Conn_InfoMessage;
                    conn.Close();
                    retValue = true;
                }
                catch (Exception e)
                {
                    OnMessage(e.Message);
                }

            }
            return retValue;

        }


        /// <summary>
        /// Выполняет SQL и возвращает DataTable
        /// </summary>
        /// <param name="sql">sql statement</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteSql(string sql)
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    conn.Open();
                    conn.InfoMessage += Conn_InfoMessage;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dtResult);

                    conn.Close();
                    da.Dispose();
                }
                catch (Exception e)
                {
                    OnMessage(e.Message);
                }
            }

            return dtResult;
        }


        private void Conn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            OnMessage(e.Message);
        }

        #region Событие
        /// <summary>
        /// При наступлении события сообщения
        /// </summary>
        public event MessageEventHandler MessageChanged;
        /// <summary>
        /// Cобытие сообщения
        /// </summary>
        /// <param name="message"></param>
        protected void OnMessage(string message)
        {
            if (MessageChanged != null)
                MessageChanged(this, new MessageEventArgs(message));
        }

        public class MessageEventArgs : EventArgs
        {
            private string message;
            public string Message
            {
                get { return message; }
                set { message = value; }
            }

            public MessageEventArgs(string message)
            {
                this.message = message;
            }
        }
        public delegate void MessageEventHandler(object sender, MessageEventArgs e);
        #endregion

        /// <summary>
        /// Очистим память
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
        }

    }
}
