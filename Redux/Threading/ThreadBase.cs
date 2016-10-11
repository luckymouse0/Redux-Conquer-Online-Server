using System.Threading;

namespace Redux.Threading
{
    public abstract class ThreadBase
    {
        #region Variables
        protected Thread Thread;
        private bool IsClosed { get; set; }

        protected abstract void OnInit();
        protected abstract bool OnProcess();
        protected abstract void OnDestroy();
        #endregion
        #region Methods
        private void ThreadProc()
        {
            OnInit();
            while (!IsClosed)
            {
                if (!OnProcess())
                {
                    break;
                }

                Thread.Sleep(Common.MINIMUM_THREAD_SLEEP_MS);
            }
            OnDestroy();
        }
        public bool CreateThread(bool run = true)
        {
            if (Thread == null)
            {
                IsClosed = false;
                Thread = new Thread(ThreadProc);
                if (run)
                {
                    Thread.Start();
                }

                return true;
            }
            return false;
        }
        public bool CloseThread()
        {
            IsClosed = true;

            if (Thread != null && Thread.IsAlive)
            {
                Thread.Join();
            }

            return true;
        }

        public bool StartThread()
        {
            if (Thread == null) return false;

            Thread.Start();
            return true;
        }

        #endregion
    }
}
