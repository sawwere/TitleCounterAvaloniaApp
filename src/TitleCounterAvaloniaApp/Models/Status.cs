using System.Dynamic;

namespace tc.Models
{
    /// <summary>
    /// Use for mapping integers to entry status and vice verse
    /// </summary>
    public class Status
    {
        public enum ContentStatus { COMPLETED = 1, BACKLOG, RETIRED, IN_PROGRESS };
        public static readonly string[] LocalizedStatuses = [
            Assets.Resources.EntryStatus_Completed,
            Assets.Resources.EntryStatus_Backlog,
            Assets.Resources.EntryStatus_Retired,
            Assets.Resources.EntryStatus_InProgress
            ];

        public static readonly string[] Statuses = [
            "completed",
            "backlog",
            "retired",
            "in progress"
            ];

        public static string Get(int index) {
            return Statuses[index];
        }

        public static string GetLocalized(int index)
        {
            return LocalizedStatuses[index];
        }
    }
}
