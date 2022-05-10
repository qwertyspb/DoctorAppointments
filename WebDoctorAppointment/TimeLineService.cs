using System;
using System.Collections.Generic;
using System.Linq;
using WebDoctorAppointment.Models.ApiModels;

namespace WebDoctorAppointment;

public class TimeLineService
{
    public static DoctorShift DoctorShift = new DoctorShift
    {
        MorningShiftStarts = 8,
        MorningShiftEnds = 12,
        AfternoonShiftStarts = 13,
        AfternoonShiftEnds = 17
    };

    public static List<AppointmentSlot> GenerateSlots(DateTime start, DateTime end, string scale)
    {
        var timeline = GenerateTimeline(start, end, scale);
        var slotDuration = scale switch
        {
            "15min" => 15,
            "30min" => 30,
            "hours" => 60,
            _ => 60
        };

        var result = new List<AppointmentSlot>();
        foreach (var cell in timeline.Where(cell => start <= cell.Start && cell.End <= end))
        {
            for (var slotStart = cell.Start; slotStart < cell.End; slotStart = slotStart.AddMinutes(slotDuration))
            {
                var slotEnd = slotStart.AddMinutes(slotDuration);

                var slot = new AppointmentSlot
                {
                    Start = slotStart,
                    End = slotEnd,
                    Status = "free"
                };

                result.Add(slot);
            }
        }

        return result;
    }


    private static List<TimeCell> GenerateTimeline(DateTime start, DateTime end, string scale)
    {
        var result = new List<TimeCell>();

        var incrementMorning = 1;
        var incrementAfternoon = 1;

        var days = (end.Date - start.Date).TotalDays;

        if (end > end.Date)
        {
            days += 1;
        }

        if (scale == "shifts")
        {
            incrementMorning = DoctorShift.MorningShiftEnds - DoctorShift.MorningShiftStarts;
            incrementAfternoon = DoctorShift.AfternoonShiftEnds - DoctorShift.AfternoonShiftStarts;
        }

        for (var i = 0; i < days; i++)
        {
            var day = start.Date.AddDays(i);
            for (var x = DoctorShift.MorningShiftStarts; x < DoctorShift.MorningShiftEnds; x += incrementMorning)
            {
                var cell = new TimeCell
                {
                    Start = day.AddHours(x),
                    End = day.AddHours(x + incrementMorning)
                };

                result.Add(cell);
            }
            for (var x = DoctorShift.AfternoonShiftStarts; x < DoctorShift.AfternoonShiftEnds; x += incrementAfternoon)
            {
                var cell = new TimeCell
                {
                    Start = day.AddHours(x),
                    End = day.AddHours(x + incrementAfternoon)
                };

                result.Add(cell);
            }
        }

        return result;
    }
}