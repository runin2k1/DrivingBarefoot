using System;
using System.Collections.Generic;
using System.Text;

namespace DrivingBarefoot.WebTimers
{
    /// <summary>
    /// A WebTimer subclass which can be initialized to fire an event once per day at a specified hour
    /// </summary>
    public class DailyTimer : WebTimer
    {
        private int _time;

        public int Time
        {
            set { 
                _time = value;

                if (_time < 0 || _time > 23)
                    throw new ArgumentException("time must be between 0 and 23");
            }
        }

        /// <summary>
        /// Hour from 0-23 indicating the time the event should fire(local to server time)
        /// </summary>
        public override long Interval
        {
            get
            {
                return base.Interval;
            }
            set
            {
                throw new InvalidOperationException("set Time property, interval is determined based on the time set");
            }
        }
        public DailyTimer()
        {

        }

        public DailyTimer(int time)
        {
            Time = time;
        }

        public override void Start()
        {
            CalculateTimeToFire();

            base.Start();
        }
        private void CalculateTimeToFire()
        {
            DateTime now = DateTime.Now.AddMinutes(1);

            DateTime fireTime = new DateTime(now.Year, now.Month, now.Day, _time, 0, 0);

            if (fireTime < now)
                fireTime = fireTime.AddDays(1); // set for tomorrow since Time already passed today

            TimeSpan minutes = fireTime.Subtract(now);
            base.Interval = (int)minutes.TotalMinutes;
        }
    }
}
