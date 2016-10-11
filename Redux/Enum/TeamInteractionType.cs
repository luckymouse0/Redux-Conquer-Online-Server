namespace Redux.Enum
{
    public enum TeamInteractType : byte
    {
        Create = 0,
        RequestJoin = 1,
        LeaveTeam = 2,
        AcceptInvite = 3,
        RequestInvite = 4,
        AcceptJoin = 5,
        Dismiss = 6,
        Kick = 7,
        ForbidNewMembers = 8,
        AllowNewMembers = 9,
        ForbidMoney = 10,
        AllowMoney = 11,
        ForbidItems = 12,
        AllowItems = 13,
    }
}