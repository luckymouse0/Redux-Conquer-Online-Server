using Redux.Enum;

namespace Redux.Database.Domain
{
    public class DbAssociate
    {
        public virtual uint RelationshipUID { get; set; }
        public virtual uint UID { get; set; }
        public virtual uint AssociateID { get; set; }
        public virtual byte Type { get; set; }
        public virtual string Name { get; set; }
    }
}