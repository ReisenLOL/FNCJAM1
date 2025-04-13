using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bremsengine
{
    public enum BremseFaction
    {
        None = 0,
        Player = 1,
        Enemy = 2
    }
    public interface IFaction
    {
        public BremseFaction Faction { get; protected set; }
        public bool IsOfFaction(BremseFaction f)
        {
            if (f == Faction)
            {
                return true;
            }
            return false;
        }
        public bool CompareFaction(BremseFaction f)
        {
            if (f == BremseFaction.None)
            {
                return true;
            }
            return IsOfFaction(f);
        }
        public void SetFaction(BremseFaction f) => Faction = f;
    }
}
