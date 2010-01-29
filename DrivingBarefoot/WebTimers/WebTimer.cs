using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.Web.Caching;
using System.Web;

namespace DrivingBarefoot.WebTimers
{
    public delegate void RemovedDelegate();

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
