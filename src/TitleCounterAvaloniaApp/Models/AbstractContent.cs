using System;

namespace tc.Models
{
    public abstract class AbstractContent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? LinkUrl { get; }
        public long? Time { get; set; }
        public DateOnly? DateRelease { get; set; }
        public float? GlobalScore { get; set; }
    }
}
