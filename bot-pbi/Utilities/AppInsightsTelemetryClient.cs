using bot_pbi.Model;
using Microsoft.ApplicationInsights;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace bot_pbi.Utilities
{
    public static class AppInsightsTelemetryClient
    {
        static TelemetryClient _instance;
        public static TelemetryClient GetInstance()
        {
            if (_instance == null)
                _instance = new TelemetryClient();
            return _instance;
        }

        public static void TrackEvent(string custonNameEvent)
        {
            _instance.TrackEvent(custonNameEvent);
        }

        public static void InsertTransaction(
                    IMessageActivity activity,
                    string originalQuestion,
                    int counter,
                    double maxScore,
                    string maxScoreQuestion,
                    string LuisIntent,
                    AnswerType answerType)
        {


            var persistency = SharedObjects.DatabaseManager;

            BotTracking tracking = new BotTracking
            {
                IdActivity = activity.Id,
                IdConversation = activity.Conversation.Id,
                EntryQuestion = originalQuestion,
                NumAnswer = counter,
                AnswerType = answerType,
                MaxScore = maxScore,
                LuisIntent = LuisIntent,
                MaxScoredQuestion = maxScoreQuestion
            };
            var transactionResult = persistency.AddData(tracking);

            if (!transactionResult.Success)
            {
                if (transactionResult.Ex != null)
                    Trace.TraceWarning($"{transactionResult.Message}: {transactionResult.Ex.Message} ");
                else
                    Trace.TraceWarning(transactionResult.Message);

            }
        }
    }
}