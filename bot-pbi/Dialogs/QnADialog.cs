using bot_pbi.Model;
using bot_pbi.Utilities;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace bot_pbi.Dialogs
{
    [Serializable]
    public class QnADialog : QnAMakerDialog
    {
        private static string typeResultInQnA;
        private double QnAMakerScoreThresHold;
        public string originalQuestion { get; set; }
        public bool isComingFromLuis { get; set; }

        public QnADialog() : base(
           new QnAMakerService(
               new QnAMakerAttribute(
                   ConfigurationManager.AppSettings["QnAMakerAuthKey"],
                   ConfigurationManager.AppSettings["QnAMakerKnowledgeBaseID"],
                   "No se encontró resultado",
                   0,
                   Convert.ToInt32(ConfigurationManager.AppSettings["QnAMakerTop"]),
                   ConfigurationManager.AppSettings["QnAMakerEndPointHostName"])))
        {
            QnAMakerScoreThresHold = Convert.ToDouble(ConfigurationManager.AppSettings["QnAMakerScoreThresHold"], new CultureInfo("en-us"));
            isComingFromLuis = false;
        }

        protected override bool IsConfidentAnswer(QnAMakerResults qnaMakerResults)
        {
            return true;
        }
        
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context,
                                                                    IMessageActivity message,
                                                                    QnAMakerResults result)
        {
            var answers = GetAnswerData(result);
            var counter = answers.Count();

            if (counter == 0)
            {
                if (isComingFromLuis)
                {
                    AppInsightsTelemetryClient.TrackEvent(message.Id);
                    AppInsightsTelemetryClient.InsertTransaction(context.Activity as IMessageActivity,
                       originalQuestion, 0, 0, SharedObjects.NO_RESULT_ANSWER_IN_DB, (context.Activity as IMessageActivity).Text,
                    AnswerType.noAnswer);
                    typeResultInQnA = SharedObjects.ResultFromQnA.NO_RESULT_FROM_QNA_AND_LUIS;
                }

                else
                    typeResultInQnA = SharedObjects.ResultFromQnA.NO_RESULT_FROM_QNA;
            }
            else
            {
                typeResultInQnA = SharedObjects.ResultFromQnA.RESULT_FROM_QNA;

                AppInsightsTelemetryClient.TrackEvent(message.Id);
                AppInsightsTelemetryClient.InsertTransaction(context.Activity as IMessageActivity,
                                originalQuestion, counter, answers[0].Score, answers[0].Questions[0],
                                (isComingFromLuis ? message.Text : "None"),
                                (isComingFromLuis ? AnswerType.QnALuis : AnswerType.QnA));

                var carrusel = context.MakeMessage();
                carrusel.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                var options = new List<Attachment>();

                if (counter == 1)
                    await context.PostAsync($"I found 1 answer:");
                else
                    await context.PostAsync($"Are any of these {counter} answers helpful?");

                foreach (var qnaMakerResult in answers)
                    options.Add(CreateCard(qnaMakerResult));

                carrusel.Attachments = options;

                await context.PostAsync(carrusel);
            }
        }

        private static Attachment CreateCard(QnAMakerResult qnaMakerResult)
        {
            return new HeroCard
            {
                Subtitle = qnaMakerResult.Questions.First(),
                Text = qnaMakerResult.Answer
            }.ToAttachment();
        }

        private List<QnAMakerResult> GetAnswerData(QnAMakerResults result)
        {
            const double TRUST_RANGE = 0.9;

            var minScore = (result.Answers[0].Score * TRUST_RANGE);

            var res = (from r in result.Answers
                       where r.Score > minScore && r.Score >= QnAMakerScoreThresHold
                       select r).ToList();
            return res;
        }

        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            IMessageActivity newMessage = context.MakeMessage();
            newMessage.Text = typeResultInQnA;
            context.Done<IMessageActivity>(newMessage);
        }
    }
}