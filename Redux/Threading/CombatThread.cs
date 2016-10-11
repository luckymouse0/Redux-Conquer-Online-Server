using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Managers;

namespace Redux.Threading
{
    public class CombatThread : ThreadBase
    {

        private const int TIMER_OFFSET_LIMIT = 10;
        private const int THREAD_SPEED = 100;
        private const int INTERVAL_EVENT = 60;

        private long _nextTrigger;
        protected override void OnInit()
        {
             _nextTrigger = Common.Clock + THREAD_SPEED;
        }
        protected override bool OnProcess()
        {
            var curr = Common.Clock;
            if (curr >= _nextTrigger)
            {
                _nextTrigger += THREAD_SPEED;

                var offset = (curr - _nextTrigger) / Common.MS_PER_SECOND;
                if (Math.Abs(offset) > TIMER_OFFSET_LIMIT)
                {
                    _nextTrigger = curr + THREAD_SPEED;
                }

                PlayerManager.CombatManager_Tick();
            }

            return true;
        }
        protected override void OnDestroy()
        {
        }
    }
}
