using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vndbDumper.Model
{
    class VisualNovel
    {
        public int id;
        public string title;
        public string original;
        public string released;
        public string[] languages;
        public string[] orig_lang;
        public string[] platforms;
        public string aliases;
        public int? length;
        public string description;
        public VisualNovelLinks links;
        public string image;
        public bool image_nsfw;
        public VisualNovelAnime[] anime;
        public VisualNovelRelation[] relations;
        public object[][] tags;
        public double popularity;
        public double rating;
        public int votecount;
        public VisualNovelScreen[] screens;
        public VisualNovelStaff[] staff;
    }
}
