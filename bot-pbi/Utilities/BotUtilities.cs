using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bot_pbi.Utilities
{
    public class BotUtilities
    {
        public static async Task DisplayWelcomeMessage(Activity activity, string message)
        {
            Activity replyMessage = activity.CreateReply("");

            ConnectorClient client = new ConnectorClient(new Uri(activity.ServiceUrl));

            var card = new HeroCard()
            { Title = message };


            var builder = new UriBuilder(SharedObjects.AppUri)
            { Path = "assets/img/logo.jpg" };

            card.Images = new List<CardImage>()
            { new CardImage(url: builder.Uri.AbsoluteUri) };

            replyMessage.Attachments.Add(card.ToAttachment());
            await client.Conversations.ReplyToActivityAsync(replyMessage);
        }
    }
}