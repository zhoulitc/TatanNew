// ReSharper disable once CheckNamespace
namespace Tatan.Data
{
    using System;

    internal sealed class DataProvider : IDataProvider
    {
        public string Name { get; set; }

        public string Connection { get; set; }

        public string ParameterSymbol { get; set; }

        public string StringSplicingSymbol { get; set; }

        public string FuzzyMatchingSymbol { get; set; }

        public string CallStoredProcedure { get; set; }

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
                    CallStoredProcedure = "CALL";
                    break;
                case "IBM.Data.DB2":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "CALL";
                    break;
                case "IBM.Data.Informix":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "CALL";
                    break;
                case "MySql.Data.MySqlClient":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "CALL";
                    break;
                case "System.Data.OracleClient":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "CALL";
                    break;
                case "Npgsql":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "CALL";
                    break;
                case "System.Data.SQLite":
                    ParameterSymbol = "$";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "";
                    break;
                case "Sybase.Data.AseClient":
                    ParameterSymbol = "?";
                    StringSplicingSymbol = "||";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "CALL";
                    break;
                case "System.Data.SqlClient":
                    ParameterSymbol = "@";
                    StringSplicingSymbol = "+";
                    FuzzyMatchingSymbol = "%";
                    CallStoredProcedure = "EXEC";
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
            return String.Format("{0}[{1}]", Name, Connection);
        }
    }
}