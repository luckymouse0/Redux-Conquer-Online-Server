using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redux.Utility
{
    public class ThreadSafeCounter
    {
        public int CounterStartValue;
        public int CounterMaxValue;
        private object _syncRoot;
        private int _counter;
        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                return _syncRoot;
            }
        }
        public ThreadSafeCounter(int start, int end)
        {
            CounterStartValue = start;
            CounterMaxValue = end;
            _counter = CounterStartValue;
        }
        public ThreadSafeCounter()
        { 
          CounterStartValue = 0;
          CounterMaxValue = int.MaxValue;
          _counter = CounterStartValue;
        }
        public int Counter
        {
            get
            {
                lock (SyncRoot)
                {
                    _counter++;
                    if (_counter > CounterMaxValue)
                        _counter = CounterStartValue;
                    return _counter;
                }
            }
        }        
    }
}
