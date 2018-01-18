namespace Microsoft.Bot.Sample.RouterDialog
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Sample.QADialogs;
    using Microsoft.Bot.Sample.Dialogs;

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
                await context.PostAsync("您想开具什么证明？");
                //context.Wait(this.MessageReceivedAsync);
                var certDialog = new CertificationDialog();
                await context.Forward(certDialog, AfterCertificationDialog, message, System.Threading.CancellationToken.None);
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

            await context.PostAsync(answerFound);

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task AfterQnADialog(IDialogContext context, IAwaitable<bool> result)
        {
            var answerFound = await result;

            // we might want to send a message or take some action if no answer was found (false returned)
            if (!answerFound)
            {
                await context.PostAsync("I’m not sure what you want.");
            }

            context.Wait(this.MessageReceivedAsync);
        }

        //private async Task SendWelcomeMessageAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        //{
        //    await context.PostAsync("Hi, I'm the Basic Multi Dialog bot. Let's get started.");

        //    context.Wait(this.MessageReceivedAsync);
        //}

        //private async Task SendWelcomeMessageAsync(IDialogContext context)
        //{
        //    await context.PostAsync("Hi, I'm the Basic Multi Dialog bot. Let's get started.");

        //    context.Call(new NameDialog(), this.NameDialogResumeAfter);
        //}

        //private async Task NameDialogResumeAfter(IDialogContext context, IAwaitable<string> result)
        //{
        //    try
        //    {
        //        this.name = await result;

        //        context.Call(new AgeDialog(this.name), this.AgeDialogResumeAfter);
        //    }
        //    catch (TooManyAttemptsException)
        //    {
        //        await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");

        //        await this.SendWelcomeMessageAsync(context);
        //    }
        //}

        //private async Task AgeDialogResumeAfter(IDialogContext context, IAwaitable<int> result)
        //{
        //    try
        //    {
        //        this.age = await result;

        //        await context.PostAsync($"Your name is { name } and your age is { age }.");

        //    }
        //    catch (TooManyAttemptsException)
        //    {
        //        await context.PostAsync("I'm sorry, I'm having issues understanding you. Let's try again.");
        //    }
        //    finally
        //    {
        //        await this.SendWelcomeMessageAsync(context);
        //    }
        //}
    }
}