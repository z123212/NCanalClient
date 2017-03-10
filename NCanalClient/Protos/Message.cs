using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.alibaba.otter.canal.protocol;

namespace NCanalClient.Protos
{
    [Serializable]
    public class Message
    {
        public Message()
        {

        }
        public Message(long id, List<Entry> entries)
        {
            this.Id = id;
            this.Entries = entries;
        }



        private static long SerialVerionUID = 1234034768477580009L;

        /// <summary>
        /// 消息集合
        /// </summary>
        public List<Entry> Entries { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

    }
}
