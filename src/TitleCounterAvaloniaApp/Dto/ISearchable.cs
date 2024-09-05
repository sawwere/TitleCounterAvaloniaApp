namespace tc.Dto
{
    public interface ISearchable
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string? LinkUrl { get; }
        public long? Time { get; set; }
        public string? DateRelease { get; set; }
        public float? GlobalScore { get; set; }
    }
}
