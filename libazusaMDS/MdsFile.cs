using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using moe.yo3explorer.azusa.mds.Model;
using System.Collections.ObjectModel;

namespace moe.yo3explorer.azusa.mds
{
    public class MdsFile
    {
        private MdsFile()
        { }

        public static MdsFile Load(FileInfo fi)
        {
            return Load(File.ReadAllBytes(fi.FullName));
        }

        public static MdsFile Load(byte[] buffer)
        {
            if (buffer == null)
                return null;

            MdsFile mds = new MdsFile();

            int pointer = 0;
            mds.header = new Header(buffer, pointer);
            pointer += 88;
            if (!mds.header.Verify())
            {
                throw new InvalidDataException();
            }

            List<TrackBlock> tracks = new List<TrackBlock>();
            
            pointer = (int)mds.header.SessionBlocksOffset;
            mds.sessionBlocks = new SessionBlock[mds.header.NumberOfSessions];
            for (int i = 0; i < mds.header.NumberOfSessions; i++)
            {
                mds.sessionBlocks[i] = new SessionBlock(buffer, pointer);
                pointer += 24;   
            }
            for (int i = 0; i < mds.header.NumberOfSessions; i++)
            {
                SessionBlock currentSession = mds.sessionBlocks[i];
                pointer = (int)currentSession.TrackBlockOffset;
                for (int j = currentSession.FirstTrack; j <= currentSession.LastTrack; j++)
                {
                    tracks.Add(new TrackBlock(buffer, pointer));
                    pointer += 80;
                }
            }
            mds.trackBlocks = tracks.AsReadOnly();
            pointer = (int)mds.header.DiscStructureOffset;
            mds.copyright = new CopyrightInfo(buffer, pointer);
            pointer += 4;
            mds.manufacturing = new ManufacturingData(buffer, pointer);
            pointer += 2048;
            mds.physical = new PhysicalInfo(buffer, pointer);
            pointer += 2048;
            if (mds.physical.NumberOfLayers == 1)
            {
                mds.SecondLayer = new SecondLayer();
                mds.SecondLayer.Copyright = new CopyrightInfo(buffer, pointer);
                pointer += 4;
                mds.SecondLayer.Manufacturing = new ManufacturingData(buffer, pointer);
                pointer += 2048;
                mds.SecondLayer.Physical = new PhysicalInfo(buffer, pointer);
                pointer += 2048;
            }
            mds.BufferSize = buffer.Length;
            return mds;
        }

        Header header;
        SessionBlock[] sessionBlocks;
        ReadOnlyCollection<TrackBlock> trackBlocks;
        CopyrightInfo copyright;
        ManufacturingData manufacturing;
        PhysicalInfo physical;

        public Header Header
        {
            get { return header; }
        }

        public SessionBlock FirstSession
        {
            get { return sessionBlocks[0]; }
        }

        public ReadOnlyCollection<TrackBlock> Tracks
        {
            get { return trackBlocks; }
        }

        public CopyrightInfo CopyrightInfo
        {
            get { return copyright; }
        }

        public ManufacturingData ManufacturingData
        {
            get { return manufacturing; }
        }

        public PhysicalInfo PhysicalInfo
            {
                get {
                    return physical;
                }
            }
            public SecondLayer SecondLayer { get; private set; }
        public int BufferSize { get; private set; }
    }
}
