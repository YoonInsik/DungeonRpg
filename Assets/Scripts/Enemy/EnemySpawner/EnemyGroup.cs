using System;
using UnityEngine;

namespace SHS
{
    [Serializable]
    public class EnemyGroup
    {
        public int enemyscale;
        public EnemyStat enemystat;
        public bool use_customradius;
        public Vector2 customradius;

        public EnemyGroup(int enemyscale, EnemyStat enemystat, bool _usecr, Vector2 _cr)
        {
            this.enemyscale = enemyscale;
            this.enemystat = enemystat;
            this.use_customradius = _usecr;
            this.customradius = _cr;
        }

    }
}