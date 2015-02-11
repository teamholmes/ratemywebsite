using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MyApp.Business.Services;
using System.Web;

namespace MyApp.Business.DomainObjects.Models
{
    public class Log : ILog
    {
        private string _Applogkey;

        private int _MaxLogEntriesToStore;

        public Log()
        {
            _Applogkey = "BACKLOGKEYFORAPP";

            _MaxLogEntriesToStore = 1000;
        }

        public void Debug(string message)
        {

            AddToApplicationCache(message);
        }

        public void Info(string message)
        {
            AddToApplicationCache(message);
        }

        public void Warning(string message)
        {
            AddToApplicationCache(message);
        }

        public void Error(string message)
        {
            string errormessage = message;
            string oldlogs = ReturnStringOfLogEntries();
        }

        private string GetLogEntry(string message)
        {
            StackFrame frame = new StackFrame(2);

            return ", " + frame.GetMethod().Name + ", " + message;
        }

        private void AddToApplicationCache(string message)
        {
            try
            {
                if (HttpContext.Current.Application[_Applogkey] == null)
                {
                    HttpContext.Current.Application[_Applogkey] = new List<String>();
                }

                List<string> logentries = (List<string>)HttpContext.Current.Application[_Applogkey];
                if (logentries.Count > _MaxLogEntriesToStore) logentries.RemoveAt(0);

                string identity = string.Empty;

                try
                {
                    identity = (string)HttpContext.Current.Request.ServerVariables["AUTH_USER"];
                }
                catch (Exception err) { }

                logentries.Add(String.Format("{0} - {1} - {2}", DateTime.Now.ToLongTimeString(), identity, message));

                HttpContext.Current.Application[_Applogkey] = logentries;
            }
            catch (Exception)
            {

            }
        }

        private string ReturnStringOfLogEntries()
        {
            if (HttpContext.Current.Application[_Applogkey] == null)
            {
                HttpContext.Current.Application[_Applogkey] = new List<String>();
            }

            StringBuilder sb = new StringBuilder();

            try
            {
                foreach (string str in (List<string>)HttpContext.Current.Application[_Applogkey])
                {
                    sb.Append(String.Format("{0}{1}", str, "\n\r"));
                }
            }
            catch (Exception err)
            {

            }

            return sb.ToString();
        }
    }
}
