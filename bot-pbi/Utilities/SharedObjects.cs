using bot_pbi.DAL;
using System;

namespace bot_pbi.Utilities
{
    public static class SharedObjects
    {
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static IDBManager DatabaseManager { get; internal set; }
        public static Uri AppUri { get; internal set; }

        public struct ResultFromQnA
        {
            public const string NO_RESULT_FROM_QNA = "NoResultFromQnA";
            public const string NO_RESULT_FROM_QNA_AND_LUIS = "NoResultFromQnAandLuis";
            public const string RESULT_FROM_QNA = "ResultFromQnA";
        }

        public const string NO_RESULT_ANSWER_IN_DB = "No hubo respuesta";
        public const string NO_RESULT_ANSWER = "Lo siento reformula la pregunta";

    }
}