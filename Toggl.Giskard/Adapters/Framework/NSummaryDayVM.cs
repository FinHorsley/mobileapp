using System;

namespace Toggl.Giskard.Adapters
{
    public class NSummaryDayVM
    {
        public NSummaryDayVM(DateTimeOffset dateTime)
        {
            DateTime = dateTime;
        }

        public DateTimeOffset DateTime { get; set; }
    }
}
