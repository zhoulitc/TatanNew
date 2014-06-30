using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatan.Common.Configuration;

namespace Tatan.Test
{
    public class DbSource
    {
        public T UseSession<T>(Func<IDbSession, T> function)
        {
            using (var session = new DbSession())
            {
                return function(session);
            }
        }
    }

    public interface IDbSession : IDisposable
    {
        
    }

    public class DbSession : IDbSession
    {
        private readonly DbCommand _command;
        private readonly IDataProvider _provider;

        private static readonly IDictionary<string, DbProviderFactory> _dbFactories =
            new Dictionary<string, DbProviderFactory>();

        public string Id { get; set; }

        internal DbSession(string id, string providerName, string connectionString)
        {
            //ExceptionHandler.ArgumentNull("source", source);
            //ExceptionHandler.ArgumentNull("connectionString", connectionString);

            if (string.IsNullOrEmpty(id) || id.Length > 128)
                id = "0";
            Id = id;

            if (!_dbFactories.ContainsKey(providerName))
                _dbFactories.Add(providerName, DbProviderFactories.GetFactory(providerName));
            var dbProviderFactory = _dbFactories[providerName];
            var connection = dbProviderFactory.CreateConnection();
            //ExceptionHandler.ArgumentNull("conn", conn);

// ReSharper disable once PossibleNullReferenceException
            connection.ConnectionString = connectionString;
        }
    }
}
