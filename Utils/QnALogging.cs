using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace LuisBot.Utils
{
    [Serializable]
    public class QnALogging
    {
        private string filePath;

        public QnALogging(string path)
        {
            //this.filePath = System.Web.HttpContext.Current.Server.MapPath(path);
            //FileInfo fInfo = new FileInfo(this.filePath);

            //if (!fInfo.Exists)
            //{
            //    using (StreamWriter sw = new StreamWriter(this.filePath, false, Encoding.UTF8))
            //    {
            //        sw.WriteLine("Question,Answer,TimeStamp");
            //    }
            //}
           
        }

        public void WriteLog(string question, string answer)
        {
            //string log = question.Replace(",","，") + "," + answer.Replace(",", "，") + "," + DateTime.Now.ToString();
            //using (StreamWriter sw = File.AppendText(this.filePath))
            //{
            //    sw.WriteLine(log);
            //}
        }
    }
}