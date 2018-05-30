using System;

namespace JobsAdmin.Handler
{
    internal class Recurrence
    {
        private Recurrence()
        {}

        private int Value { get; set; }
        private DayOfWeek? DayOfTheWeek { get; set; }

        public RecurrenceType Type { get; private set; }
        public DateTime NextRunAt { get; private set; }

        public void ReSchedule()
        {
            switch (Type)
            {
                case RecurrenceType.Minutes:
                    NextRunAt = DateTime.Now.AddMinutes(Value);
                    break;
                case RecurrenceType.Daily:
                    NextRunAt = NextRunAt.AddDays(1);
                    break;
                case RecurrenceType.Weekly:
                    NextRunAt = NextRunAt.AddDays(7);
                    break;
                case RecurrenceType.Monthly:
                    NextRunAt = NextRunAt.AddMonths(1);
                    break;
            }
        }

        private static int GetValueInRange(int value, int min, int max)
        {
            return value < min
                ? min
                : (value > max ? max : value);
        }

        public static Recurrence BuildFromMinutes(int minutes)
        {
            var recurrence = new Recurrence() { Type = RecurrenceType.Minutes, Value = minutes < 1 ? 1 : minutes };
            recurrence.NextRunAt = DateTime.Now.AddMinutes(recurrence.Value);
            return recurrence;
        }

        public static Recurrence BuildDaily(int atHour)
        {
            var recurrence = new Recurrence()
            {
                Type = RecurrenceType.Daily,
                Value = GetValueInRange(atHour, 0, 23)
            };

            recurrence.NextRunAt = DateTime.Today.AddHours(recurrence.Value);

            if (DateTime.Now.Hour >= recurrence.Value)
                recurrence.NextRunAt = recurrence.NextRunAt.AddDays(1);

            return recurrence;
        }

        public static Recurrence BuildWeekly(DayOfWeek day, int atHour)
        {
            var recurrence = new Recurrence()
            {
                Type = RecurrenceType.Weekly,
                Value = GetValueInRange(atHour, 0, 23),
                DayOfTheWeek = day
            };

            recurrence.NextRunAt = DateTime.Today.AddHours(atHour);

            while (recurrence.NextRunAt.DayOfWeek != day)
                recurrence.NextRunAt = recurrence.NextRunAt.AddDays(1);

            if (DateTime.Now > recurrence.NextRunAt)
                recurrence.NextRunAt = recurrence.NextRunAt.AddDays(7);

            return recurrence;
        }

        public static Recurrence BuildMonthly(int atHour)
        {
            var recurrence = new Recurrence()
            {
                Type = RecurrenceType.Monthly,
                Value = GetValueInRange(atHour, 0, 23)
            };

            recurrence.NextRunAt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, recurrence.Value, 0, 0);

            if (DateTime.Now > recurrence.NextRunAt)
                recurrence.NextRunAt = recurrence.NextRunAt.AddMonths(1);

            return recurrence;
        }
    }
}
