namespace Redux.Enum
{
    public enum ChatType : ushort
    {
        /// <summary>
        /// [2000]
        /// </summary>
        Talk = 2000,

        /// <summary>
        /// [2001]
        /// </summary>
        Whisper,

        /// <summary>
        /// [2002]
        /// </summary>
        Action,

        /// <summary>
        /// [2003]
        /// </summary>
        Team,

        /// <summary>
        /// [2004]
        /// </summary>
        Syndicate,

        /// <summary>
        /// [2005]
        /// </summary>
        System = Talk + 5,

        /// <summary>
        /// [2006]
        /// </summary>
        Family,

        /// <summary>
        /// [2007]
        /// </summary>
        Talk2,

        /// <summary>
        /// [2008]
        /// </summary>
        Yelp,

        /// <summary>
        /// [2009]
        /// </summary>
        Friend,

        /// <summary>
        /// [2010]
        /// </summary>
        Global = Talk + 10,

        /// <summary>
        /// [2011]
        /// </summary>
        GM,

        /// <summary>
        /// [2013]
        /// </summary>
        Ghost,

        /// <summary>
        /// [2014]
        /// </summary>
        Serve,

        /// <summary>
        /// [2021]
        /// </summary>
        World = Talk + 21,

        /// <summary>
        /// [2100]
        /// </summary>
        Register = Talk + 100,

        /// <summary>
        /// [2101]
        /// </summary>
        Entrance,
        Shop,
        PetTalk,
        CryOut,
        Webpage = Talk + 105,
        NewMessage,
        Task,
        SynWarFirst,
        SynWarNext,
        LeaveWord = Talk + 110,
        SynAnnounce,
        MessageBox,
        Reject,
        SynTenet,

        MsgBoardTrade = Talk + 201,
        MsgBoardFriend,
        MsgBoardTeam,
        MsgBoardSyndicate,
        MsgBoardOther,
        MsgBoardSystem,

        Broadcast = Talk + 500
    }
}
