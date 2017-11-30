using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Diagnostics;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"])))
        //public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute("94acc471-3420-4467-84bd-ac5d054c0f80", "904f0966f6204a4b8f1b8bdf88b9886d")))
        {
            Trace.TraceError("appid:{0}, appkey: {1}", ConfigurationManager.AppSettings["LuisAppId"], ConfigurationManager.AppSettings["LuisAPIKey"]);
        }

        [LuisIntent("控制家用电器")]
        public async Task AppControlIntent(IDialogContext context, LuisResult result)
        {
            string intent = "";
            foreach(var t in result.Entities){
                intent += t;
                intent += ",";
            }
            await context.PostAsync($"You have reached the 控制家电 intent. You said: {result.Query} Intent: {intent}"); //
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            string intent = "";
            foreach(var t in result.Entities){
                intent += t.Entity;
                intent += t.Score;
                intent += ",";
            }
            await context.PostAsync($"-You have reached the none intent. You said: {result.Query} Intents: {intent}"); //
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("")]
        public async Task EmptyIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached the empty intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("LightControl")]
        public async Task LightControlIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached the LightControl intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "MyIntent" with the name of your newly created intent in the following handler
        [LuisIntent("MyIntent")]
        public async Task MyIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"-You have reached the MyIntent intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
        }
        
        [LuisIntent("TVControl")]
        public async Task TVControllIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached the TVControl intent. You said: {result.Query}"); //
            context.Wait(MessageReceived);
        }
        
        
    }
}