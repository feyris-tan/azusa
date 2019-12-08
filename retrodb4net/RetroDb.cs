using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace retrodb4net
{
    class RetroDb : 
        IEnumerator<KeyValuePair<string,Dictionary<object,object>>>, 
        IEnumerable<KeyValuePair<string,Dictionary<object,object>>>
    {
        private static ulong MAGIC_NO = 18652425620701522;

        private RetroDb() { }

        public static RetroDb LoadFrom(byte[] buffer)
        {
            return LoadFrom(new MemoryStream(buffer, false));
        }

        public static RetroDb LoadFrom(FileInfo fi)
        {
            return LoadFrom(fi.OpenRead());
        }

        public static RetroDb LoadFrom(Stream str)
        {
            RetroDb result = new RetroDb();
            result.data = new List<Dictionary<object, object>>();

            BinaryReader br = new BinaryReader(str);
            if (br.ReadUInt64() != MAGIC_NO)
                throw new RetroDbException("wrong magic!");

            br.ReadUInt64();    //always seems to be zero, so discard it.
            
            while (true)
            {
                object o = br.ReadRMSGPack();
                if (o == null)
                    break;

                Dictionary<object,object> dict = o as Dictionary<object,object>;
                if (dict != null)
                {
                    result.data.Add(dict);
                }
            }
            return result;
        }

        private List<Dictionary<object, object>> data;


        public int Size => data.Count;
        private int pointer;

        public void Dispose()
        {
            Reset();
        }

        public bool MoveNext()
        {
            pointer++;
            if (pointer >= data.Count)
                return false;
            return true;
        }

        public void Reset()
        {
            pointer = 0;
        }

        private KeyValuePair<string,Dictionary<object, object>> GetCurrent()
        {
            Dictionary<object, object> victim = data[pointer];
            return new KeyValuePair<string, Dictionary<object, object>>(victim["name"].ToString(), victim);
        }

        public IEnumerator<KeyValuePair<string, Dictionary<object, object>>> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public KeyValuePair<string, Dictionary<object, object>> Current
        {
            get
            {
                return GetCurrent();
            }
        }

        object IEnumerator.Current => GetCurrent();
    }
}