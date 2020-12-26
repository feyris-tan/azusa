using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Utilities.Ps1BatchImport
{

    class Game
    {
        public List<GameDisc> GameDiscs { get; set; }
        public string Name { get; set; }
        public byte[] Cover { get; set; }

        public GameDisc GetDiscByName(string filename)
        {
            GameDisc firstOrDefault = GameDiscs.FirstOrDefault(x => x.Name.Equals(filename));
            if (firstOrDefault == null)
            {
                firstOrDefault = new GameDisc();
                firstOrDefault.Name = filename;
                GameDiscs.Add(firstOrDefault);
            }
            return firstOrDefault;
        }

        public GameDisc GetByUnixMinute(long lookFor)
        {
            foreach (GameDisc gameDisc in GameDiscs)
            {
                if (gameDisc.CueFile.LastWriteTime.ToUnixMinute() == lookFor)
                    return gameDisc;
                if (gameDisc.BinFile.LastWriteTime.ToUnixMinute() == lookFor)
                    return gameDisc;
                if (gameDisc.Md5File.LastWriteTime.ToUnixMinute() == lookFor)
                    return gameDisc;
            }

            return null;
        }

        public string GuessSku()
        {
            GameDisc gameDisc = GameDiscs.First();
            return gameDisc.GuessSku();
        }
    }
}
