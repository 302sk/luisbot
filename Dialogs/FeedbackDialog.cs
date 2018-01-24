namespace Microsoft.Bot.Sample.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class FeedbackDialog : IDialog<string>
    {
        
        private int step = 0;

        public async Task StartAsync(IDialogContext context)
        {
            //await context.PostAsync("1.第三方模板证明 2.开具公司模板证明");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (step == 0)
            {
                await context.PostAsync("请您对我的服务做出评价：1. 非常满意 2. 满意 3. 不满意");
                step = 1;
                context.Wait(this.MessageReceivedAsync);
            }
            else if (step == 1)
            {
                string feedback = "";
                switch (message.Text)
                {
                    case "1":
                        feedback = "非常满意";
                        step = 3;
                        break;
                    case "2":
                        feedback = "满意";
                        step = 3;
                        break;
                    default:
                        step = 2;
                        await context.PostAsync("对不起，没有帮到您，请您留下宝贵意见：");
                        break;
                }
                if (step == 3)
                {
                    await context.PostAsync("谢谢您的评价，我会继续努力！");
                    context.Done("感谢您的评价");

                }
                else
                {
                    context.Wait(this.MessageReceivedAsync);
                }
                

            }
            else if (step == 2)
            {
                
                await context.PostAsync("谢谢，我会持续改进服务质量！");
                context.Done("");

            }
            else
            {
                
                context.Done("");
            }


        }
    }
}