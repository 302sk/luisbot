using System;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using QnAMakerDialog;
//using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;

namespace Microsoft.Bot.Sample.QADialogs
{
    [Serializable]
    //[QnAMakerService(ConfigurationManager.AppSettings["QnaSubscriptionKey"], ConfigurationManager.AppSettings["QnaKnowledgebaseId"])]
    [QnAMakerService("da40658c25604d178dab6d13769ec56c", "878cfb2e-fa4c-4aa1-9ecd-d194470d16aa")]
    public class QnADialog : QnAMakerDialog<bool>
    {
        /// <summary>
        /// Handler used when the QnAMaker finds no appropriate answer
        /// </summary>
        public override async Task NoMatchHandler(IDialogContext context, string originalQueryText)
        {
            await context.PostAsync($"Sorry, I couldn't find an answer for '{originalQueryText}'.");
            //context.Wait(MessageReceived);
            context.Done(false);
        }

        /// <summary>
        /// This is the default handler used if no specific applicable score handlers are found
        /// </summary>
        public override async Task DefaultMatchHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            // ProcessResultAndCreateMessageActivity will remove any attachment markup from the results answer
            // and add any attachments to a new message activity with the message activity text set by default
            // to the answer property from the result
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
            messageActivity.Text = $"I found an answer that might help...{result.Answer}.";

            await context.PostAsync(messageActivity);

            //context.Wait(MessageReceived);
            context.Done(true);
        }

        /// <summary>
        /// Handler to respond when QnAMakerResult score is a maximum of 50
        /// </summary>
        [QnAMakerResponseHandler(50)]
        public async Task LowScoreHandler(IDialogContext context, string originalQueryText, QnAMakerResult result)
        {
            var messageActivity = ProcessResultAndCreateMessageActivity(context, ref result);
            messageActivity.Text = $"I found an answer that might help...{result.Answer}.";
            await context.PostAsync(messageActivity);

            //context.Wait(MessageReceived);
            context.Done(true);
        }
    }
}