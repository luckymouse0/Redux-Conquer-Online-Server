using System;
namespace Redux.Space
{
    public interface ILocatableObject
    {
        Point Location { get; set; }
        Map Map { get; set; }
        uint UID { get; set; }
    }
}
