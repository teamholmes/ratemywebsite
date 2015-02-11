using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Diagnostics;
using OP.General.Extensions;

namespace OP.General.Performance
{


    public class Performance
    {
        private DateTime _StartDateTime { get; set; }

        private DateTime _EndDateTime { get; set; }


        private string _Taskname { get; set; }


        public void StartPerformance(string taskname)
        {
            _Taskname = taskname.ToUpperCheckForNull();
            _StartDateTime = DateTime.Now;
        }


        public void EndPerformance()
        {
            TimeSpan diff = (DateTime.Now - _StartDateTime);
#if DEBUG
            string decimalplaces = "F2";
            Debug.WriteLine(String.Format("  >>>> Performance : Task '{3}' {0} ms or {1} sec or {2} min  <<<<", diff.TotalMilliseconds.ToString(decimalplaces), diff.TotalSeconds.ToString(decimalplaces), diff.TotalMinutes.ToString(decimalplaces), _Taskname));
#endif
        }


        public void RestartPerformance()
        {
            _StartDateTime = DateTime.Now;
        }


    }
}
