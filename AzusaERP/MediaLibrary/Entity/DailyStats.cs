using System;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    class DailyStats
    {
        private DateTime date;
        private int totalProducts, totalMedia, missingCover, missingGraph, undumped, missingScreenshots;
        private DateTime dateAdded;

        public DateTime Date
        {
            get => date;
            set => date = value;
        }

        public int TotalProducts
        {
            get => totalProducts;
            set => totalProducts = value;
        }

        public int TotalMedia
        {
            get => totalMedia;
            set => totalMedia = value;
        }

        public int MissingCover
        {
            get => missingCover;
            set => missingCover = value;
        }

        public int MissingGraph
        {
            get => missingGraph;
            set => missingGraph = value;
        }

        public int Undumped
        {
            get => undumped;
            set => undumped = value;
        }

        public int MissingScreenshots
        {
            get => missingScreenshots;
            set => missingScreenshots = value;
        }

        public DateTime DateAdded
        {
            get => dateAdded;
            set => dateAdded = value;
        }
    }
}
