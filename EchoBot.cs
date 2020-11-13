// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.10.3

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

using IBM.Watson.Assistant.v1;
using IBM.Cloud.SDK.Core.Authentication.Iam; 

using Newtonsoft.Json; 
using Newtonsoft.Json.Linq;

namespace EchoBot1.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // オリジナルのコード
            //var replyText = $"watson: {turnContext.Activity.Text}";
            //await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);

            // Watson AssistantのAPI Referrenceにて提供されているコード
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: "***YourAPIKey***"
            );

            AssistantService assistant = new AssistantService("2020-04-01", authenticator);
            assistant.SetServiceUrl("https://api.us-south.assistant.watson.cloud.ibm.com/instances/***YourInstance***");

            var result = assistant.Message(
                workspaceId: "***YourSkillId***",
                input: new IBM.Watson.Assistant.v1.Model.MessageInput()
                {
                    Text = turnContext.Activity.Text
                }
            );

            //Console.WriteLine(result.Response);
            //await turnContext.SendActivityAsync(MessageFactory.Text(result.Response, result.Response), cancellationToken);

            var jsonObject = JObject.Parse(result.Response);
            var res = jsonObject["output"]["generic"][0]["text"].ToString();

            await turnContext.SendActivityAsync(MessageFactory.Text(res, res), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Watson Assistantだよ";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
