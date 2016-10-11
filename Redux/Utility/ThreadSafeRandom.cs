using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Utility
{
    public class ThreadSafeRandom : Random
    {
        private object _syncRoot;
        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }

        public override int Next()
        {
            lock (SyncRoot)
                return base.Next();
        }
        public override int Next(int maxVal)
        {
            lock (SyncRoot)
                return base.Next(maxVal);
        }
        public override int Next(int minVal, int maxVal)
        {
            lock (SyncRoot)
                return base.Next(minVal, maxVal);
        }
        public override void NextBytes(byte[] buffer)
        {
            lock(SyncRoot)                
                base.NextBytes(buffer);
        }
        public override double NextDouble()
        {
            lock (SyncRoot) 
                return base.NextDouble();
        }
    }
}
