using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace bot_pbi.DAL
{
    public static class DBManagerFactory
    {
        static IDBManager _instance;

        public static IDBManager GetInstance(DBType dbType, StringDictionary parameters)

        {
            switch (dbType)
            {
                case DBType.DocumentDB:
                    if (_instance == null)
                        _instance = CreateDocumentInstance(parameters);
                    break;
                default:
                    break;
            }
            return _instance;
        }

        private static IDBManager CreateDocumentInstance(StringDictionary parameters)
        {
            IDBManager localInstance;
            var dbName = parameters.GetParameter("documentDbName");
            var endPoint = parameters.GetParameter("endPoint");
            var authKey = parameters.GetParameter("authKey");


            if (
                !string.IsNullOrEmpty(dbName)
            && !string.IsNullOrEmpty(endPoint)
            && !string.IsNullOrEmpty(authKey)

            )
            {
                localInstance = DocumentDBManager.GetInstance(dbName, endPoint, authKey);
            }
            else
            {
                throw new Exception($"Parámetros errados para conexión con DocumentDB: dbName={dbName}, endPoint={endPoint}, authKey={authKey} ");
            }

            return localInstance;
        }

        private static string GetParameter(this StringDictionary sd, string parameter)
        {
            var rta = string.Empty;

            if (sd.ContainsKey(parameter))
                rta = sd[parameter];

            return rta;
        }
    }
}