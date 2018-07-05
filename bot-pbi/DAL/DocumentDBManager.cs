using bot_pbi.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace bot_pbi.DAL
{
    public class DocumentDBManager : IDBManager, IDisposable
    {
        private static DocumentDBManager _instance;
        private DocumentClient _docuClient;
        private Database _database;
        private readonly string _dbName;

        private readonly string _collectionID = nameof(BotTracking);

        private DocumentDBManager(string dbName, string endPoint, string authKey)
        {

            _dbName = dbName;
            _docuClient = new DocumentClient(new Uri(endPoint), authKey);

            InitializeConnectionAsync().Wait();
        }

        private async Task InitializeConnectionAsync()
        {
            if (_database == null)
            {
                _database = await _docuClient.CreateDatabaseIfNotExistsAsync(
                        new Database { Id = _dbName });
            }
        }

        public static IDBManager GetInstance(string dbName, string endPoint, string authKey)
        {
            if (_instance == null)
                _instance = new DocumentDBManager(dbName, endPoint, authKey);

            return _instance;
        }

        public DBTransactionResult AddData(object info)
        {
            var result = new DBTransactionResult();
            string typename = info.GetType().Name;

            try
            {
                switch (typename)
                {
                    case nameof(BotTracking):
                        var rr = _docuClient.CreateDocumentAsync(
                            UriFactory.CreateDocumentCollectionUri(_dbName, _collectionID), info
                            ).Result;
                        break;
                    default:
                        result.Success = false;
                        result.Message = "Tipo de información no esperado al insertar en la base de datos";
                        break;
                }
            }
            catch (DocumentClientException ex)
            {
                result.SetException(ex);
            }
            return result;

        }

        void IDisposable.Dispose()
        {
        }
    }
}