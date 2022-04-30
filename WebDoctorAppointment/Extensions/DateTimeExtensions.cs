using System;

namespace WebDoctorAppointment.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Округляет дату до указанной величины вверх
        /// </summary>
        /// <param name="dt">Округляемая дата</param>
        /// <param name="d">Величина округления</param>
        /// <returns>Округленная вверх дата</returns>
        public static DateTime RoundUp(this DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

    }
}