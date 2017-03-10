using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCanalClient
{
    public class BufferPool
    {
        public static int DEFAULT_BUFFERLENGTH = 1024 * 1024 * 2;

        public BufferPool(int count, int bufferlength)
        {
            for (int i = 0; i < count; i++)
            {
                mPools.Push(new byte[bufferlength]);
            }
            mBufferLength = bufferlength;
        }

        private static BufferPool mSingle;

        public static BufferPool Single
        {
            get
            {
                if (mSingle == null)
                    mSingle = new BufferPool(20, DEFAULT_BUFFERLENGTH);
                return mSingle;
            }

        }

        private int mBufferLength = 1024;

        private System.Collections.Concurrent.ConcurrentStack<byte[]> mPools = new System.Collections.Concurrent.ConcurrentStack<byte[]>();

        public byte[] Pop()
        {
            byte[] result = null;
            if (mPools.TryPop(out result))
            {
                return result;
            }
            else
            {
                return new byte[mBufferLength];
            }
        }

        public void Push(byte[] data)
        {
            mPools.Push(data);
        }
    }
}
