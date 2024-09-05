using System;
using System.Runtime.InteropServices;

namespace tc.Models
{
    public enum TitleStatus { COMPLETED = 1, BACKLOG, RETIRED, IN_PROGRESS };
    [StructLayout(LayoutKind.Sequential)]
    public abstract class Entry
    {
        public AbstractContent Content { get; set; }

        public long Id { get; set; }

        public long UserId { get; set; }

        public string CustomTitle { get; set; } = null!;

        public string Status { get; set; }

        public DateOnly? DateCompleted { get; set; }

        public string? Note { get; set; }

        public long? Score { get; set; }
    }
}
