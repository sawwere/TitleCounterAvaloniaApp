using System.Collections.Generic;

namespace tc.Models
{
    public class TVSeries : FilmEntry
    {
        public Dictionary<int, int> Seasons { get; private set; }

        ///*public override void print() 
        //{
        //    base.print();
        //    foreach (var s in Seasons)
        //        Console.WriteLine(s.Key.ToString(), s.Value);
        //    return;
        //}*/

        //[JsonConstructor]
        //public TVSeries(Dictionary<int, int> seasons, string rus_name, List<string> genres,
        //    string name, string image_url, string list, double time, int score, int year, string status)
        //    : base(rus_name, genres, name, image_url, list, time, score, year, status)
        //{
        //    Seasons = seasons;
        //}
    }
}
