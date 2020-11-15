using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using moe.yo3explorer.azusa.Control.DatabaseIO.Drivers;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.Control.DatabaseIO.Migrations
{
    class Migration2to3
    {
        public static void Migrate()
        {
            AzusaContext context = AzusaContext.GetInstance();
            PostgresDriver driver = (PostgresDriver) context.DatabaseDriver;
            List<AttachmentMigrationCandidate> candidates = driver.GetAttachmentMigrationCandidates().ToList();
            driver.BeginTransaction();
            foreach (AttachmentMigrationCandidate candidate in candidates)
            {
                int mid = candidate.MediaId;
                if (candidate.CICM != null)
                    driver.InsertAttachment(MigrateElement(candidate.CICM, 11, mid));
                if (candidate.JedecId != null)
                    driver.InsertAttachment(MigrateElement(candidate.JedecId, 15, mid));
                if (candidate.MHddLog != null)
                    driver.InsertAttachment(MigrateElement(candidate.MHddLog, 12, mid));
                if (candidate.Priv != null)
                    driver.InsertAttachment(MigrateElement(candidate.Priv, 14, mid));
                if (candidate.ScsiInfo != null)
                    driver.InsertAttachment(MigrateElement(candidate.ScsiInfo, 13, mid));
            }

            driver.EndTransaction(true);
        }

        private static Attachment MigrateElement(byte[] buffer, int typeId, int MediaId)
        {
            Attachment attachment = new Attachment();
            attachment._Buffer = buffer;
            attachment._Complete = true;
            attachment._IsInDatabase = true;
            attachment._MediaId = MediaId;
            attachment._TypeId = typeId;
            return attachment;
        }
    }

    class AttachmentMigrationCandidate
    {
        public byte[] CICM;
        public byte[] MHddLog;
        public byte[] ScsiInfo;
        public byte[] Priv;
        public byte[] JedecId;
        public int MediaId;

        public bool ContainsData()
        {
            if (CICM != null)
                return true;
            if (MHddLog != null)
                return true;
            if (ScsiInfo != null)
                return true;
            if (Priv != null)
                return true;
            if (JedecId != null)
                return true;
            return false;
        }
    }
}
