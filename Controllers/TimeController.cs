using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace RestApp.Controllers
{
    public class TimeIntervals
    {
        public List<List<string>> Time { get; set; }
    }
    public class RemainingClockTime
    {
        public string HorasJornada { get; set; }
        public string HoraFin { get; set; }
        public string HorasTrabajadas { get; set; }
        public string HorasRestantes { get; set; }
        public bool Overtime { get; set; }
    }

    public class Response
    {
        public string UtcNow { get; set; }
        public List<RemainingClockTime> RemainingClockTime { get; set; }
    }

    [ApiController]
    [Route("time")]
    public class TimeController : ControllerBase
    {
        private static readonly string _formatTimeSpan = "hh\\:mm";
        private static readonly string _formatDateTime = "H:mm";
        private static readonly List<KeyValuePair<string, TimeSpan>> _timeSpans = new List<KeyValuePair<string, TimeSpan>>
        {
            new KeyValuePair<string, TimeSpan>(
                "08:30",
                TimeSpan.ParseExact("08:30", _formatTimeSpan, null)
            ),
            new KeyValuePair<string, TimeSpan>(
                "08:00",
                TimeSpan.ParseExact("08:00", _formatTimeSpan, null)
            ),
            new KeyValuePair<string, TimeSpan>(
                "06:00",
                TimeSpan.ParseExact("06:00", _formatTimeSpan, null)
            ),
        };

        [HttpPost]
        public Response Post([FromBody] TimeIntervals intervals)
        {
            TimeSpan total = new TimeSpan();
            DateTime now = DateTime.UtcNow.AddHours(1);

            foreach (List<string> interval in intervals.Time)
            {
                if (interval.Count > 0)
                {
                    TimeSpan left = TimeSpan.ParseExact(interval[0], _formatTimeSpan, null);
                    TimeSpan right = interval.Count > 1
                        ? TimeSpan.ParseExact(interval[1], _formatTimeSpan, null)
                        : TimeSpan.FromTicks(now.Ticks);

                    total += right - left;
                }
            }

            TimeSpan _total = new TimeSpan(total.Hours, total.Minutes, total.Seconds);
            List<RemainingClockTime> remainingClockTimes = new List<RemainingClockTime>();

            foreach (KeyValuePair<string, TimeSpan> timeSpan in _timeSpans)
            {
                TimeSpan remaining = timeSpan.Value - _total;
                bool isOvertime = _total >= timeSpan.Value;
                string remainingSymbol = isOvertime ? "-" : string.Empty;
                remainingClockTimes.Add(new RemainingClockTime()
                {
                    HorasJornada = timeSpan.Key,
                    HoraFin = (now + remaining).ToString(_formatDateTime),
                    HorasTrabajadas = _total.ToString(_formatTimeSpan),
                    HorasRestantes = $"{remainingSymbol}{remaining.ToString(_formatTimeSpan)}",
                    Overtime = isOvertime
                });
            }

            return new Response()
            {
                UtcNow = now.ToString(_formatDateTime),
                RemainingClockTime = remainingClockTimes
            };
        }
    }
}
