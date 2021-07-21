using Mail_scheduler_service.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Mail_scheduler_service
{
    public partial class Service1 : ServiceBase 
    {
        private System.Timers.Timer _timer1;
        private string timeString;
        public int getCallType;

        [Obsolete]
        public Service1()
        {
            InitializeComponent();
            //Mail_sender.SendEmail();
            int strTime = Convert.ToInt32(ConfigurationSettings.AppSettings["callDuration"]);
            getCallType = Convert.ToInt32(ConfigurationSettings.AppSettings["CallType"]);
            if (getCallType == 1)
            {
                double inter = (double)GetNextInterval();
                _timer1 = new System.Timers.Timer();
                _timer1.Interval = inter;
                _timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
            else
            {
                _timer1 = new System.Timers.Timer();
                _timer1.Interval = strTime * 1000;
                _timer1.Elapsed += new ElapsedEventHandler(ServiceTimer_Tick);
            }
        }
        protected override void OnStart(string[] args)
        {
            _timer1.AutoReset = true; //  to do the action at every intervel we set its as True.otherwise false.
            _timer1.Enabled = true; //  for raising elapsed event at specific intervel we enable it true.
            Mail_sender.WriteErrorLog("Service started");
        }

        protected override void OnStop()
        {
            _timer1.AutoReset = false;  // false because we can't stop the process at every intervel.
            _timer1.Enabled = false;
            Mail_sender.WriteErrorLog("Service stopped");
        }
        [Obsolete]
        private double GetNextInterval()
        {
            timeString = ConfigurationSettings.AppSettings["StartTime"];
            DateTime t = DateTime.Parse(timeString);
            TimeSpan ts = new TimeSpan();
            //int x;
            ts = t - DateTime.Now;  
            if (ts.TotalMilliseconds < 0)
            {
                ts = t.AddSeconds(60) - DateTime.Now;  //Here you can increase the timer interval based on your requirments.   
            }
            return ts.TotalMilliseconds;
        }
        [Obsolete]
        private void SetTimer()
        {
            try
            {
                double inter = (double)GetNextInterval();
                _timer1.Interval = inter;
                _timer1.Start();
            }
            catch (Exception ex)
            {
                Mail_sender.WriteErrorLog(ex);
            }
        }

        [Obsolete]
        private void ServiceTimer_Tick(object sender,ElapsedEventArgs e)
        {
            Mail_sender.SendEmail();
            if (getCallType == 1)
            {
                _timer1.Start();//setting timer1.Enabled = false;
                //System.Threading.Thread.Sleep(10000);  // //we just waiting for 16 minute here because we know a specific timespan when we should continue.
                //SetTimer();
            }
        }
    }
}
