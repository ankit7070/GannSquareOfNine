using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Timers;
using GannLibrary;

namespace GannSchedulerService
{
    public partial class GannService : ServiceBase
    {
        Timer timer;
        public GannService()
        {
            InitializeComponent();
            timer = new System.Timers.Timer();
            double inter = (double)GetNextInterval();
            timer.Interval = inter;
            timer.Elapsed += new ElapsedEventHandler(Tweet);
            
        }

        protected override void OnStart(string[] args)
        {
            timer.AutoReset = true;
            timer.Enabled = true;
            ServiceLog.WriteErrorLog("Daily Gann service started");
        }

        protected override void OnStop()
        {
            timer.AutoReset = false;
            timer.Enabled = false;
            ServiceLog.WriteErrorLog("Daily Gann service stopped");
        }

        private double GetNextInterval()
        {
            var timeString = ConfigurationManager.AppSettings["StartTime"];
            DateTime t = DateTime.Parse(timeString);
            TimeSpan ts = new TimeSpan();
            int x;
            ts = t - System.DateTime.Now;
            if (ts.TotalMilliseconds < 0)
            {
                ts = t.AddDays(1) - System.DateTime.Now;//Here you can increase the timer interval based on your requirments.   
            }
            return ts.TotalMilliseconds;
        }
        private void SetTimer()
        {
            try
            {
                double inter = (double)GetNextInterval();
                timer.Interval = inter;
                timer.Start();
            }
            catch (Exception ex)
            {
                ServiceLog.WriteErrorLog(ex);
            }
        }

        private void Tweet(object sender, System.Timers.ElapsedEventArgs e)
        {
            string tweetBuy = CreateBuyTweet(10800); //To-Do get price
            string tweetSell = CreateSellTweet(10800);
            ServiceLog.WriteErrorLog(tweetBuy);
            ServiceLog.WriteErrorLog(tweetSell);
            timer.Stop();
            System.Threading.Thread.Sleep(1000000);
            SetTimer();
        }

        private string CreateBuyTweet(double cmp)
        {
            Gann gannStudy = new Gann(cmp, 2);
            StringBuilder sb = new StringBuilder();
            sb.Append("#NIFTY FUTURE BUY ABOVE '");
            sb.Append(gannStudy.BuyAt);
            sb.Append("' T1: '");
            sb.Append(gannStudy.BuyTargetOne);
            sb.Append("' T2: '");
            sb.Append(gannStudy.BuyTargetTwo);
            sb.Append("' T3: '");
            sb.Append(gannStudy.BuyTargetThree);
            sb.Append("' T4: '");
            sb.Append(gannStudy.BuyTargetFour);
            sb.Append("' T5: '");
            sb.Append(gannStudy.BuyTargetFive);
            sb.Append("' #AlgoTrading by @NiftyBees #BankNifty");

            return sb.ToString();

        }

        private string CreateSellTweet(double cmp)
        {
            Gann gannStudy = new Gann(cmp, 2);
            StringBuilder sb = new StringBuilder();
            sb.Append("#NIFTY FUTURE SELL ABOVE '");
            sb.Append(gannStudy.SellAt);
            sb.Append("' T1: '");
            sb.Append(gannStudy.SellTargetOne);
            sb.Append("' T2: '");
            sb.Append(gannStudy.SellTargetTwo);
            sb.Append("' T3: '");
            sb.Append(gannStudy.SellTargetThree);
            sb.Append("' T4: '");
            sb.Append(gannStudy.SellTargetFour);
            sb.Append("' T5: '");
            sb.Append(gannStudy.SellTargetFive);
            sb.Append("' #AlgoTrading by @NiftyBees #BankNifty");

            return sb.ToString();

        }
    }
}
