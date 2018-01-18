namespace Microsoft.Bot.Sample.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class CertificationDialog : IDialog<string>
    {
        private int attempts = 3;
        //是否第三方证明
        private bool isThirdParty = false;
        private int step = 1;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("1.第三方模板证明 2.开具公司模板证明");

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if(step == 1) //第一步，选择第三方或者公司模板
            {
                if(message.Text == "1"){
                    await context.PostAsync("需要经理审批，然后发信给 HR，三个工作日后领取");
                    context.Done("开具证明流程完毕");
                    //step = 0;
                }
                else if(message.Text == "2"){
                    await context.PostAsync("请在以下模板中选择，然后回复选中的序号：1.收入证明 2.离职证明");
                    this.step = 2;
                    context.Wait(this.MessageReceivedAsync);

                }else{
                    await context.PostAsync("您输入的是" + message.Text +".序号有误，请输入如下序号：1.第三方模板证明 2.开具公司模板证明");

                    context.Wait(this.MessageReceivedAsync);
                }

            }
            else if(step == 2){
                string certificationType = "";
                switch(message.Text)
                {
                    case "1":
                        certificationType = "收入证明";
                        step = 3;
                        break;
                    case "2":
                        certificationType = "离职证明";
                        step = 3;
                        break;
                    default:
                        await context.PostAsync("非法序号，请重新输入:1.收入证明 2.离职证明");
                        break;
                }
                if(step == 3){
                    await context.PostAsync("你要开具"+certificationType+",请问需要盖 1.HR公章 2. 公司公章");

                }    
                context.Wait(this.MessageReceivedAsync);

            }
            else if(step == 3){
                if(message.Text == "1"){ //选择 HR 公章
                    step = 4;
                    await context.PostAsync("请选择获取方式：1.自取 2.邮寄");
                    context.Wait(this.MessageReceivedAsync);
                }
                else if(message.Text == "2"){ //选择公司公章
                    //step = 0; //流程完毕
                    await context.PostAsync("请联系法务部门申请");
                    context.Done("开具证明流程完毕");
                }
                else{
                    await context.PostAsync("不支持的选项，请重新选择：1.HR公章 2. 公司公章");
                    context.Wait(this.MessageReceivedAsync);
                }

            }
            else if(step == 4){
                if(message.Text == "1"){
                    //step = 0;
                    await context.PostAsync("请您三个工作日后到 HR 办公室领取");
                    context.Done("开具证明流程完毕");

                }
                else if(message.Text == "2"){
                    step = 5;
                    await context.PostAsync("请您留下邮寄地址");
                    context.Wait(this.MessageReceivedAsync);
                }else{
                    await context.PostAsync("不支持的递送方式，请重新选择：1.自取 2.邮寄");
                    context.Wait(this.MessageReceivedAsync);

                }

            }
            else if(step == 5){
                await context.PostAsync("您的证明信将在三个工作日后发往" + message.Text);
                //step = 0;
                context.Done("开具证明流程完毕");

            }
            else{
                await context.PostAsync("证明信流程执行完毕");
                context.Done("证明信流程执行完毕");
            }


        }
    }
}