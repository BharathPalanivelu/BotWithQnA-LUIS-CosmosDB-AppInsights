using bot_pbi.DAL;
using bot_pbi.Utilities;
using System.Web.Http;
using System.Diagnostics;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace bot_pbi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AppInsightsTelemetryClient.GetInstance();

            try
            {
                StringDictionary parameters = new StringDictionary
                {
                    ["connectionString"] = ConfigurationManager.AppSettings["MongoDBStringConnection"],
                    ["mongoDbName"] = ConfigurationManager.AppSettings["MongoDBName"],
                    ["documentDbName"] = ConfigurationManager.AppSettings["DocumentDBName"],
                    ["authKey"] = ConfigurationManager.AppSettings["DocumentDBAuthKey"],
                    ["endPoint"] = ConfigurationManager.AppSettings["DocumentDBEndPoint"]
                };

                if (Enum.TryParse<DAL.DBType>(ConfigurationManager.AppSettings["DBType"], out DAL.DBType dbt))
                {
                    SharedObjects.DatabaseManager = DBManagerFactory.GetInstance(dbt, parameters);
                }
                else
                {
                    throw new ConfigurationErrorsException("Valor de DBType Incorrecto en el archivo de configuración");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceWarning($"El servicio de Bot funciona pero no la conexión a la base de datos de preguntas no fue posible: {ex}");
            }
        }

        public override void Dispose()
        {
            SharedObjects.DatabaseManager.Dispose();
            base.Dispose();
        }
    }
}
