/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace NCanalClient
{
    public class ProtobufHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Serialize<T>(T t)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T DeSerialize<T>(string content)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        public static T DeSerialize<T>(byte[] values)
        {
            using (MemoryStream ms = new MemoryStream(values, 0, values.Length))
            {
                T t = Serializer.Deserialize<T>(ms);
                return t;
            }
        }

        public static byte[] Serialize2Byte<T>(T t)
        {
            var obj2Str = Serializer.GetProto<T>();
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize(ms, t);
                return ms.ToArray();
            }
        }

        public static byte[] Serialize2Byte(string value)
        {
            throw new NotImplementedException();
        }
    }
}
*/