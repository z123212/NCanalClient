using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.alibaba.otter.canal.protocol
{
    [global::System.Serializable, global::ProtoBuf.ProtoContract(Name = @"RowData")]
    public partial class RowData : ProtoBuf.IExtensible
    {
        /// <summary>
        /// 字段信息，增量数据(修改前,删除前)
        /// </summary>
        [global::ProtoBuf.ProtoMember(1, Name = @"beforeColumns", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public List<Column> BeforeColumns { get; set; }


        /// <summary>
        /// 字段信息，增量数据(修改后,新增后)
        /// </summary>
        [global::ProtoBuf.ProtoMember(2, Name = @"afterColumns", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public List<Column> AfterColumns { get; set; }
        /// <summary>
        /// 预留扩展
        /// </summary>
        [global::ProtoBuf.ProtoMember(3, Name = @"props", DataFormat = global::ProtoBuf.DataFormat.Default)]
        public List<Pair> Props { get; set; }

        private global::ProtoBuf.IExtension extensionObject;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
}
