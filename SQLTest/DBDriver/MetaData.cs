using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLTest
{

    /// <summary>
    /// Класс описи таблицы
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public string TableName { get; } = "shipment";


        List<Field> _fields = new List<Field>();
        /// <summary>
        /// Список полей таблицы
        /// </summary>
        public List<Field> Fields { get => _fields; set => _fields = value; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Table()
        {
            _fields.Clear();
            _fields.Add(new Field("id") { type = typeof(int) });
            _fields.Add(new Field("data") { type = typeof(DateTime) });
            _fields.Add(new Field("organization"));
            _fields.Add(new Field("city"));
            _fields.Add(new Field("country"));
            _fields.Add(new Field("manager"));
            _fields.Add(new Field("quantity") { type = typeof(double) });
            _fields.Add(new Field("summ") { type = typeof(double) });
        }
    }

    /// <summary>
    /// Класс описи поля
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Имя поля
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Тип поля
        /// </summary>
        public Type type { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id"></param>
        public Field(string id)
        {
            Id = id;
            type = typeof(string);
        }
    }
}
