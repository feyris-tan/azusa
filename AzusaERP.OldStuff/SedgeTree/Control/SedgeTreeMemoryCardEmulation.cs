using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AzusaERP.OldStuff;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;

namespace moe.yo3explorer.azusa.SedgeTree.Control
{
    class SedgeTreeMemoryCardEmulation
    {
        private static SedgeTreeMemoryCardEmulation singleton;

        private SedgeTreeMemoryCardEmulation() { }

        private Bloodline data;
        private AzusaContext context;

        public static SedgeTreeMemoryCardEmulation GetInstance()
        {
            if (singleton == null)
            {
                singleton = new SedgeTreeMemoryCardEmulation();
                singleton.context = AzusaContext.GetInstance();
                singleton.LoadData();
            }
            return singleton;
        }

        public Bloodline GetData()
        {
            return data;
        }

        private void LoadData()
        {
            context.Splash.SetLabel("Frage Stammbaumversion ab...");
            Nullable<int> version = context.DatabaseDriver.SedgeTree_GetLatestVersion();

            if (version == null)
            {
                context.Splash.SetLabel("Lade Stammbaumdaten...");
                byte[] rawDdata = context.DatabaseDriver.SedgeTree_GetDataByVersion(version.Value);
                
                MemoryStream memoryStream = new MemoryStream(rawDdata, false);
                BinaryFormatter bf = new BinaryFormatter();
                data = (Bloodline)bf.Deserialize(memoryStream);
                memoryStream.Close();
            }
            else
            {
                data = new Bloodline();
            }
        }

        public void SaveData()
        {
            if (data._guid == Guid.Empty)
            {
                data._guid = Guid.NewGuid();
            }
            data.author = Environment.UserName;
            data.author_machine_name = Environment.MachineName;
            data._last_edited = DateTime.Now;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, data);
            ms.Flush();

            context.DatabaseDriver.SedgeTree_InsertVersion(ms.ToArray());
        }
    }
}
