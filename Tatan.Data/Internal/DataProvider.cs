// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    internal sealed class DataProvider : IDataProvider
    {
        public string Name { get; set; }

        public string Connection { get; set; }

        public string ParameterSymbol { get; set; }

        public string StringSplicingSymbol { get; set; }

        public string FuzzyMatchingSymbol { get; set; }

        public DataProvider(string name, string connection)
        {
            Name = name;
            Connection = connection;
            switch (Name)
            {
                case "System.Data.OleDb":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "*";
                    break;
                case "IBM.Data.DB2":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    break;
                case "IBM.Data.Informix":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    break;
                case "MySql.Data.MySqlClient":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    break;
                case "System.Data.OracleClient":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    break;
                case "Npgsql":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    break;
                case "System.Data.SQLite":
                    ParameterSymbol = "$";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    break;
                case "Sybase.Data.AseClient":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    break;
                case "System.Data.SqlClient":
                    ParameterSymbol = "@";
                    StringSplicingSymbol = "+";
                    FuzzyMatchingSymbol = "%";
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Connection.GetHashCode();
        }

        public override string ToString()
        {
            return Name + "[" + Connection + "]";
        }
    }
}