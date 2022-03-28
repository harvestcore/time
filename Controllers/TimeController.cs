using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace RestApp.Controllers;

public record TimeIntervals(List<List<string>> Time, int ExtraHours, List<string> WorkingHours);
public record RemainingClockTime(string HorasJornada, string HoraFin, string HorasTrabajadas, string HorasRestantes, bool Overtime);
public record Response(string UtcNow, List<RemainingClockTime> RemainingClockTime);

[ApiController]
[Route("time")]
public class TimeController : ControllerBase
{
    private static readonly string _formatTimeSpan = "hh\\:mm";
    private static readonly string _formatDateTime = "H:mm";

    [HttpPost]
    public Response Post([FromBody] TimeIntervals intervals)
    {
        if (intervals == null || intervals.Time == null || intervals.Time.Count == 0)
        {
            return new Response(string.Empty, new());
        }

        TimeSpan total = new();

        int extraHours = intervals.ExtraHours < 0 ? 0 : intervals.ExtraHours;
        DateTime now = DateTime.UtcNow.AddHours(extraHours);

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

        TimeSpan _total = new(total.Hours, total.Minutes, total.Seconds);
        List<RemainingClockTime> remainingClockTimes = new();

        List<string> workingHoursIntervals = intervals.WorkingHours ?? new List<string>() { "08:30", "06:00" };

        foreach (string workingHours in workingHoursIntervals)
        {
            TimeSpan workingHourTS = TimeSpan.ParseExact(workingHours, _formatTimeSpan, null);
            TimeSpan remaining = workingHourTS - _total;
            bool isOvertime = _total >= workingHourTS;
            string remainingSymbol = isOvertime ? "-" : string.Empty;
            remainingClockTimes.Add(new RemainingClockTime(
                workingHours,
                (now + remaining).ToString(_formatDateTime),
                _total.ToString(_formatTimeSpan),
                $"{remainingSymbol}{remaining.ToString(_formatTimeSpan)}",
                isOvertime
            ));
        }

        return new Response(now.ToString(_formatDateTime), remainingClockTimes);
    }
}
