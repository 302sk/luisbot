using System;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Dialogs;
using System.Web.Http.Description;
using System.Net.Http;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Bot.Sample.QADialogs;
using Microsoft.Bot.Sample.RouterDialog;


namespace Microsoft.Bot.Sample.LuisBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        //private TelemetryClient telemetry = new TelemetryClient();
        /// <summary>
        /// POST: api/Messages
        /// receive a message from a user and send replies
        /// </summary>
        /// <param name="activity"></param>
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            Trace.TraceError("appid:{0}, appkey: {1}", ConfigurationManager.AppSettings["LuisAppId2"], ConfigurationManager.AppSettings["LuisAPIKey2"]);
            // check if activity is of type message
            //throw(new Exception("api message has been called"));
            System.Console.WriteLine("Message : {0}", activity.GetActivityType().ToString());
            try
            {
                if (activity.GetActivityType() == ActivityTypes.Message)
                {
                    //await Conversation.SendAsync(activity, () => new BasicLuisDialog());
                    //add  root dialog for routing message aganist user's intent
                    await Conversation.SendAsync(activity, () => new RootDialog());

                    //await Conversation.SendAsync(activity, () => new QnaDialog());
                }
                else
                {
                    HandleSystemMessage(activity);
                }
            }
            catch(Exception ex)
            {
                Trace.TraceError("Throw Exception when process Chinese character: {0}", ex.Message);
                return new HttpResponseMessage(System.Net.HttpStatusCode.Ambiguous);
            }
            
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}