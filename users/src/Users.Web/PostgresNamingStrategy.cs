using System.Runtime.CompilerServices;
using NHibernate.Cfg;

namespace Users.Web
{
    public class PostgresNamingStrategy : INamingStrategy
    {
        public string ClassToTableName(string className) 
            => DoubleQuote(className);

        public string PropertyToColumnName(string propertyName) 
            => DoubleQuote(propertyName);

        public string TableName(string tableName) 
            => DoubleQuote(tableName);

        public string ColumnName(string columnName) 
            => DoubleQuote(columnName);

        public string PropertyToTableName(string className, string propertyName) 
            => DoubleQuote(propertyName);

        public string LogicalColumnName(string columnName, string propertyName) 
            => string.IsNullOrWhiteSpace(columnName) ?
                DoubleQuote(propertyName) :
                DoubleQuote(columnName);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string DoubleQuote(string raw) 
        {
            // In some cases the identifier is single-quoted.
            // We simply remove the single quotes:
            raw = raw.Replace("`", "");
            return $"\"{raw}\"";
        }
    }
}