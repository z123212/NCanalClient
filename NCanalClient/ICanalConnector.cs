using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCanalClient
{ 
    using Protos;

    /// <summary>
    /// nanal连接接口类
    /// </summary>
    public interface ICanalConnector
    {

        /// <summary>
        /// 链接对应的canal server
        /// </summary>
        void Connect();

        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();
        /// <summary>
        /// 检查有效性,检查下链接是否合法 几种case下链接不合法: 1.
        /// </summary>
        bool CheckValid();
        /// <summary>
        /// 订阅
        /// </summary>
        void Subscribe();
        /// <summary>
        /// 取消订阅
        /// </summary>
        void UnSubscribe();
        /// <summary>
        /// 获取数据，自动进行确认，该方法返回的条件：尝试拿batchSize条记录，有多少取多少，不会阻塞等待
        /// </summary>
        Message Get(int batchSize);
        /// <summary>
        /// 获取数据，自动进行确认
        ///该方法返回的条件：
        ///a.拿够batchSize条记录或者超过timeout时间
        ///b.如果timeout=0，则阻塞至拿到batchSize记录才返回
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        Message Get(int batchSize, long timeout, TimeUnit unit);

        /// <summary>
        ///不指定 position 获取事件，该方法返回的条件: 尝试拿batchSize条记录，有多少取多少，不会阻塞等待
        ///canal 会记住此 client 最新的position。 
        ///如果是第一次 fetch，则会从 canal 中保存的最老一条数据开始输出。
        /// </summary>
        Message GetWithoutAck(int batchSize);
        /// <summary>
        ///不指定 position 获取事件.
        ///该方法返回的条件：
        ///a.拿够batchSize条记录或者超过timeout时间
        ///b.如果timeout=0，则阻塞至拿到batchSize记录才返回
        ///canal 会记住此 client 最新的position。 
        ///如果是第一次 fetch，则会从 canal 中保存的最老一条数据开始输出。
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        Message GetWithoutAck(int batchSize, long timeout, TimeUnit unit);

        /// <summary>
        ///进行 batch id 的确认。确认之后，小于等于此 batchId 的 Message 都会被确认。
        /// </summary>
        void Ack(long batchId);
        /// <summary>
        ///回滚到未进行 ack 的地方，指定回滚具体的batchId
        /// </summary>
        void Rollback(long batchId);
        /// <summary>
        /// 回滚到未进行 ack 的地方，下次fetch的时候，可以从最后一个没有 ack 的地方开始拿
        /// </summary>
        void Rollback();
    }
}
