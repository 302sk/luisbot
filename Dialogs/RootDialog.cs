﻿namespace Microsoft.Bot.Sample.RouterDialog
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Sample.QADialogs;
    using Microsoft.Bot.Sample.Dialogs;
    using global::LuisBot.RPADemo;

#pragma warning disable 1998

    [Serializable]
    public class RootDialog : IDialog<object>
    {

        private string name;
        private int age;

        public async Task StartAsync(IDialogContext context)
        {
            /* Wait until the first message is received from the conversation and call MessageReceviedAsync 
             *  to process that message. */
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            /* When MessageReceivedAsync is called, it's passed an IAwaitable<IMessageActivity>. To get the message,
             *  await the result. */
            var message = await result;
            //在这里判断用户输入是否包含关键字“证明”，如果包含，启动证明表单 Dialog
            if(message.Text.Contains("证明"))
            {
                //不现实该提示，避免微信中多条回复导致报错
                //await context.PostAsync("您想开具什么证明？");

                var certDialog = new CertificationDialog();
                await context.Forward(certDialog, AfterCertificationDialog, message, System.Threading.CancellationToken.None);
            }
            else if (message.Text.Contains("rpademo"))
            {
                //magic word for triggering rpa demo
                RPADemoCSVPortTypeClient testProcess = new RPADemoCSVPortTypeClient("RPADemoCSVSoap");
                testProcess.ClientCredentials.UserName.UserName = "admin";
                testProcess.ClientCredentials.UserName.Password = "Pass@word2";
                testProcess.RPADemoCSV();
                await context.PostAsync("RPA Demo has been triggered!");
                context.Wait(this.MessageReceivedAsync);
            }
            else
            {
                var qnadialog = new QnADialog();
                var messageToForward = message;
                //context.Call(qnadialog, this.AfterQnADialog);
                await context.Forward(qnadialog, AfterQnADialog, messageToForward, System.Threading.CancellationToken.None);
            }
            //await this.SendWelcomeMessageAsync(context);
            //context.Wait(this.MessageReceivedAsync);
        }
        private async Task AfterCertificationDialog(IDialogContext context, IAwaitable<string> result)
        {
            var answerFound = await result;
            //不显示流程完毕的提示，避免微信中多条回复导致报错
            //await context.PostAsync(answerFound);
            var fbDialog = new FeedbackDialog();
            await context.Forward(fbDialog, AfterFeedbackDialog, new Activity(), System.Threading.CancellationToken.None);

            //context.Wait(this.MessageReceivedAsync);
        }

        private async Task AfterQnADialog(IDialogContext context, IAwaitable<bool> result)
        {
            var answerFound = await result;

            // we might want to send a message or take some action if no answer was found (false returned)
            //if (answerFound)
            //{
            //    //await context.PostAsync("I’m not sure what you want.");
            //    var fbDialog = new FeedbackDialog();
            //    await context.Forward(fbDialog, AfterFeedbackDialog,new Activity(), System.Threading.CancellationToken.None);
            //}
            //else
            //{
                context.Wait(this.MessageReceivedAsync);
            //}          
        }

        private async Task AfterFeedbackDialog(IDialogContext context, IAwaitable<string> result)
        {
            context.Wait(this.MessageReceivedAsync);
        }

    }
}