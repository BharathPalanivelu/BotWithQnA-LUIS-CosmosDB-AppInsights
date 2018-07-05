using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Configuration;
using Microsoft.Cognitive.LUIS;
using bot_pbi.Utilities;
using bot_pbi.Model;

namespace bot_pbi.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            QnADialog dialog = new QnADialog();
            
            dialog.originalQuestion = activity.Text;
            await context.Forward(dialog, AfterQnADialog, activity, CancellationToken.None);
        }

        private async Task AfterQnADialog(IDialogContext context, IAwaitable<object> result)
        {
            var message = context.Activity as IMessageActivity;
            string typeResultInQnA = (await result as Activity).Text;

            switch (typeResultInQnA)
            {
                case SharedObjects.ResultFromQnA.NO_RESULT_FROM_QNA:
                    QnADialog dialog = new QnADialog();
                    dialog.isComingFromLuis = true;


                    string LuisAnswer = GetLuisAnswer(message.Text, out double score);

                    if (LuisAnswer == "None")
                    {
                        await context.PostAsync(SharedObjects.NO_RESULT_ANSWER);

                        AppInsightsTelemetryClient.TrackEvent(message.Id);
                        AppInsightsTelemetryClient.InsertTransaction(message, message.Text, 0, score, SharedObjects.NO_RESULT_ANSWER_IN_DB, LuisAnswer, AnswerType.noAnswer);
                    }
                    else
                    {
                        dialog.originalQuestion = message.Text;
                        message.Text = LuisAnswer;
                        await context.Forward(dialog, AfterQnADialog, message);
                    
                    }
                    break;

                case SharedObjects.ResultFromQnA.NO_RESULT_FROM_QNA_AND_LUIS:
                    await context.PostAsync(SharedObjects.NO_RESULT_ANSWER);
                    break;

                default:
                    context.Done(this);
                    break;
            }
        }

        private string GetLuisAnswer(string text, out double score)
        {
            string LUIS_APP_ID = ConfigurationManager.AppSettings["LuisApplicationId"];
            string LUIS_KEY = ConfigurationManager.AppSettings["LuisSubscriptionKey"];
            LuisClient luisClient = new LuisClient(LUIS_APP_ID, LUIS_KEY);
            var resultLuis = luisClient.Predict(text).Result;
            var luisOriginalIntent = resultLuis.TopScoringIntent.Name;
            score = resultLuis.TopScoringIntent.Score;
            return luisOriginalIntent.Replace('-', ' ');
        }
    }
}