using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Web.Caching;
using System.Web;

namespace DrivingBarefoot.WebTimers
{
    public delegate void RemovedDelegate();

    /// <summary>
    /// WebTimer class implements a basic recurring Timer that can be instantiated in an ASP.Net page.  
    /// 
    /// WebTimer may be overridden to implement variations of the basic timer which fires in x minute intervals(see DailyTimer)
    /// 
        /// <example>
        /// Example from Global.asax - Application_Start()
        ///     DailyTimer daily = new DailyTimer();
        ///     daily.Time = 14;
        ///     daily.RemovedEvent += GetOutTimerEvents.DailyEventHandler;
        ///     daily.Start();
        ///
        ///     WebTimer fiveMinute = new WebTimer(5);
        ///     fiveMinute.RemovedEvent += GetOutTimerEvents.FiveMinuteTimerHandler;
        ///     fiveMinute.Start();
        ///
        ///     WebTimer thirtyMinute = new WebTimer(30);
        ///     thirtyMinute.RemovedEvent += GetOutTimerEvents.ThirtyMinuteTimerHandler;
        ///     thirtyMinute.Start();
        /// 
        ///     From: GetOutTimerEvents.cs
        ///     public static void DailyEventHandler()
        ///     {
        ///         DBUtil db = new DBUtil();
        ///         db.CreateTimerEventLogEntry("Daily timer event handler fired");
        ///     }
        /// </example>
    /// </summary>
    /// 
    public class WebTimer
    {
        protected Cache httpCache;

        private Guid id = Guid.NewGuid();

        private long _interval;

        protected DateTime expiration;

        /// <summary>
        /// Interval between firing timer events in minutes
        /// </summary>
        public virtual long Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// Event which will be fired when the timer expires
        /// </summary>
        public event RemovedDelegate RemovedEvent;

        public WebTimer()
        {

        }

        public WebTimer(long interval)
        {
            Interval = interval;
        }

        public virtual void Start()
        {
            if (HttpContext.Current != null)
                httpCache = HttpContext.Current.Cache;

            expiration = DateTime.Now.AddMinutes(_interval);

            CacheItemRemovedCallback onRemove = CacheItemRemoved;
            
            // here we piggy back on the built in Caching engine which uses its own timers to expire items in cache
            // when this entry expires we'll fire whatever event is tied to this timer
            httpCache.Add(id.ToString(), this, null, expiration, TimeSpan.Zero, CacheItemPriority.Normal, onRemove);
        }

        public event ElapsedEventHandler Elapsed;

        public static void CacheItemRemoved(string key, object value, CacheItemRemovedReason callBackReason)
        {
            if (callBackReason == CacheItemRemovedReason.Expired)
            {
                // fire delegates assigned to this timer
                WebTimer timer = (WebTimer)value;
                timer.RemovedEvent();
                timer.Start();
            }
        }

        public static void TestElapsed()
        {
            Console.WriteLine("testelapsed");
        }
    }
}
