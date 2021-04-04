using System;

namespace moe.yo3explorer.azusa.OfflineReaders.VgmDb.Entity
{
    class Track
    {
        public Track(string name, int len)
        {
            Name = name;
            Length = new TimeSpan(0, 0, len);
        }

        public Track(Tuple<string, int, int, string, int> data)
        {
            DiscName = data.Item1;
            DiscNo = data.Item2 + 1;
            TrackNo = data.Item3 + 1;
            Name = data.Item4;
            Length = new TimeSpan(0, 0, data.Item5);
        }

        public string DiscName { get; private set; }
        public int DiscNo { get; private set; }
        public int TrackNo { get; private set; }
        public string Name { get; private set; }
        public TimeSpan Length { get; private set; }

        public override string ToString()
        {
            return String.Format("{0}.{1} {2} ({3})", DiscNo, TrackNo, Name, Length);
        }
    }
}
