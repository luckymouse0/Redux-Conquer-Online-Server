using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbAccount
    {
        public virtual uint UID { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string EMail { get; set; }
        public virtual int EmailStatus { get; set; }
        public virtual string Question { get; set; }
        public virtual string Answer { get; set; }
        public virtual PlayerPermission Permission { get; set; }
        public virtual uint Token { get; set; }
        public virtual uint Timestamp { get; set; }
    }
}

