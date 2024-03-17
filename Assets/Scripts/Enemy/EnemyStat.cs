using System;
using UnityEngine;

namespace SHS
{ 
    [CreateAssetMenu(fileName = "EnemyStat", menuName = "Enemy/EnemyStat")]
    public class EnemyStat : ScriptableObject
    {
        public float hp;
        public float speed;

        public EnemyStat(float hp, float speed)
        {
            this.hp = hp;
            this.speed = speed;
        }
    }
}