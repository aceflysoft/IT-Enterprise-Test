using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SQLTest
{
    /// <summary>
    /// Работа с БД
    /// </summary>
    public class DataDriver : IDisposable
    {
        private Table _table;

        string _connectionString = string.Empty;
        /// <summary>
        /// Строка соединения с БД
        /// </summary>
        public string ConnectionString { get => _connectionString; set => _connectionString = value; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public DataDriver(Table table)
        {
            _connectionString = ConfigurationSettings.AppSettings["ConnectionString"];
            _table = table;
        }

        /// <summary>
        /// Очистка
        /// </summary>
        public void Dispose()
        {
            GC.Collect();
        }

        private void Sq_MessageChanged(object sender, SQLConnect.MessageEventArgs e)
        {
            //TODO тут надо делать вывод сообщений не через winforms
            MessageBox.Show(e.Message, "DataDriver", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Проверка соединения
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            var retValue = false;
            using (SQLConnect sq = new SQLConnect())
            {
                sq.ConnectionString = _connectionString;
                sq.MessageChanged += Sq_MessageChanged;
                retValue = sq.TestConnection();
            }

            return retValue;
        }

        /// <summary>
        /// Получим таблицу Table
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetAllTable()
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine("SELECT");
            for (int i = 0; i < _table.Fields.Count; i++)
            {
                Field field = _table.Fields[i];
                sbSql.Append(field.Id);
                if ((i + 1) < _table.Fields.Count)
                    sbSql.AppendLine(",");
            }
            sbSql.AppendLine();
            sbSql.AppendLine("FROM");
            sbSql.AppendLine(_table.TableName);
            sbSql.AppendLine();


            DataTable dtResult;
            using (SQLConnect sq = new SQLConnect())
            {
                sq.ConnectionString = _connectionString;
                sq.MessageChanged += Sq_MessageChanged;
                dtResult = sq.ExecuteSql(sbSql.ToString());
            }

            return dtResult;
        }

        /// <summary>
        /// Получим таблицу Table
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable GetGroupTable(List<Field> groupFields)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.AppendLine("SELECT");
            for (int i = 0; i < groupFields.Count; i++)
            {
                Field field = groupFields[i];
                sbSql.Append(field.Id);
                sbSql.AppendLine(",");
            }
            for (int i = 0; i < _table.Fields.Count; i++)
            {
                Field field = _table.Fields[i];
                if (field.type != typeof(double))
                    continue;
                sbSql.AppendFormat("sum({0}) AS \"{0}\"", field.Id);
                sbSql.AppendLine(",");
            }
            sbSql = sbSql.Remove(sbSql.Length - 3, 3);
            sbSql.AppendLine();
            sbSql.AppendLine("FROM");
            sbSql.AppendLine(_table.TableName);
            sbSql.AppendLine();
            sbSql.AppendLine("GROUP BY");
            for (int i = 0; i < groupFields.Count; i++)
            {
                Field field = groupFields[i];
                sbSql.Append(field.Id);
                if ((i + 1) < groupFields.Count)
                    sbSql.AppendLine(",");
            }
            sbSql.AppendLine();

            DataTable dtResult;
            using (SQLConnect sq = new SQLConnect())
            {
                sq.ConnectionString = _connectionString;
                sq.MessageChanged += Sq_MessageChanged;
                dtResult = sq.ExecuteSql(sbSql.ToString());
            }

            return dtResult;
        }

    }
}
