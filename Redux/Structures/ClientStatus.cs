using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Redux.Enum;

namespace Redux.Structures
{
    public class ClientStatus
    {
        public ClientStatus(Enum.ClientStatus _type, int _value, long _timeout)
        {
            Type = _type;
            Value = _value;
            Timeout = _timeout;
        }
        public Enum.ClientStatus Type;
        public int Value;
        public long Timeout;
    }
}
