using BusinessLogicLibrary.Enums;
using BusinessLogicLibrary.Responses;
using DocAppLibrary.Enum;

namespace BusinessLogicLibrary;

public class TimeLineService
{
    public static List<AppointmentDto> GenerateSlots(DateTime start, DateTime end, SlotDurationType scale)
    {
        var timeline = GenerateTimeline(start, end, scale);
        var slotDuration = Constants.Scale[scale];

        var result = new List<AppointmentDto>();
        foreach (var cell in timeline.Where(cell => start <= cell.Start && cell.End <= end))
        {
            for (var slotStart = cell.Start; slotStart < cell.End; slotStart = slotStart.AddMinutes(slotDuration))
            {
                var slotEnd = slotStart.AddMinutes(slotDuration);

                var slot = new AppointmentDto
                {
                    StartTime = slotStart,
                    EndTime = slotEnd,
                    Status = StatusType.Free
                };

                result.Add(slot);
            }
        }

        return result;
    }


    private static IEnumerable<TimeCell> GenerateTimeline(DateTime start, DateTime end, SlotDurationType scale)
    {
        var result = new List<TimeCell>();

        var incrementMorning = 1;
        var incrementAfternoon = 1;

        var days = (end.Date - start.Date).TotalDays;

        if (end > end.Date)
        {
            days += 1;
        }

        if (scale == SlotDurationType.Shifts)
        {
            incrementMorning = Constants.Shifts.MorningShiftEnds - Constants.Shifts.MorningShiftStarts;
            incrementAfternoon = Constants.Shifts.AfternoonShiftEnds - Constants.Shifts.AfternoonShiftStarts;
        }

        for (var i = 0; i < days; i++)
        {
            var day = start.Date.AddDays(i);
            for (var x = Constants.Shifts.MorningShiftStarts;
                 x < Constants.Shifts.MorningShiftEnds;
                 x += incrementMorning)
            {
                var cell = new TimeCell
                {
                    Start = day.AddHours(x),
                    End = day.AddHours(x + incrementMorning)
                };

                result.Add(cell);
            }

            for (var x = Constants.Shifts.AfternoonShiftStarts;
                 x < Constants.Shifts.AfternoonShiftEnds;
                 x += incrementAfternoon)
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