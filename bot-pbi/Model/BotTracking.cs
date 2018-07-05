using bot_pbi.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bot_pbi.Model
{
    [Serializable]
    public class BotTracking
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string IdActivity { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AnswerType AnswerType { get; set; }

        public DateTime Date { get; set; }

        public string EntryQuestion { get; set; }

        public string LuisIntent { get; set; }

        public string IdConversation { get; set; }

        public double MaxScore { get; set; }

        public string MaxScoredQuestion { get; set; }

        public int NumAnswer { get; set; }

        public BotTracking()
        {
            AnswerType = AnswerType.noAnswer;
            NumAnswer = 0;
            Date = DateTime.Now;
            MaxScore = 0;
            LuisIntent = "N/A";
            MaxScoredQuestion = SharedObjects.NO_RESULT_ANSWER_IN_DB;
        }
    }
}