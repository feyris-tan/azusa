using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using com.dyndns_server.yo3explorer.yo3explorer.Dreamcast;
using Tsubasa.IO;

namespace com.dyndns_server.yo3explorer.yo3explorer.Archive
{
    public static class GDROMReader
    {
        public static Stream BuildGdRomStream(FileInfo gdi)
        {
            string[] script = File.ReadAllLines(gdi.FullName);
            string[] split;
            string filename;
            split = script[3].Split(' ');
            uint gd_data_lba = Convert.ToUInt32(split[1]);
            uint last_track_end = 45000;
            uint this_track_begin;
            uint diff;

            byte totalTracks = Convert.ToByte(script[0]);
            // high density stuff
            switch (totalTracks)
            {
                case 3:
                    //single data track
                    split = script[3].Split(' ');
                    filename = split[4];
                    filename = Path.Combine(gdi.Directory.FullName, filename);
                    FileInfo isoinfo = new FileInfo(filename);
                    Stream singleTrackGdRom = new GDRomStream(isoinfo);
                    return singleTrackGdRom;
                default:
                    //multi data track with cdda
                    List<Stream> files = new List<Stream>();
                    FileStream temp;
                    for (int i = 3; i < Convert.ToInt32(script[0]) + 1; i++)
                    {
                        split = script[i].Split(' ');
                        this_track_begin = Convert.ToUInt32(split[1]);
                        if (this_track_begin > last_track_end)
                        {
                            diff = (this_track_begin - last_track_end);
                            DummySectorStream dss = new DummySectorStream(diff);
                            files.Add(dss);
                            System.Diagnostics.Debug.Print("GD-ROM: " + diff + " block gap at " + this_track_begin);
                        }
                        filename = split[4];
                        filename = Path.Combine(gdi.Directory.FullName, filename);
                        temp = new FileInfo(filename).OpenRead();
                        files.Add(temp);
                        last_track_end = this_track_begin + ((uint)temp.Length / 2352);
                    }
                    ConcatStream seq = new ConcatStream(files);
                    return new GDRomStream(seq);
            }
        }
    }
}
