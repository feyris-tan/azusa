using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ehentaiDumper.JsonThingies
{
    public class GrabSiteMessage
    {
        public GrabSiteMessage()
        {
            job_data = new GrabSiteJobData();
            job_data.ident = Guid.NewGuid().ToString();
            job_data.concurrency = 1;
            job_data.started_at = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            job_data.max_content_length = -1;
            job_data.video = false;
            type = "stdout";
        }

        public string type;
        public GrabSiteJobData job_data;
        public string message;

        public ArraySegment<byte> toArraySegment()
        {
            string level1 = JsonConvert.SerializeObject(this);
            byte[] level2 = Encoding.UTF8.GetBytes(level1);
            ArraySegment<byte> level3 = new ArraySegment<byte>(level2);
            return level3;
        }
    }

    public class GrabSiteJobData
    {
        public string ident, url;
        public int started_at;
        public int max_content_length;
        public bool supress_ignore_reports, video, scrape;
        public int concurrency;
        public long bytes_downloaded, items_queued, items_downloaded;
        public int delay_min, delay_max, r1xx, r2xx, r3xx, r4xx;
        public int r5xx, runk;
    }
}
