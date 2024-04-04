using System;
using UnityEngine;

namespace SHS
{ 
    [CreateAssetMenu(fileName = "EnemyStat", menuName = "Enemy/EnemyStat")]
    public class EnemyStat : ScriptableObject
    {
        public int enemy_id;
        public float hp;
        public float speed;

        public EnemyStat(int id, float hp, float speed)
        {
            this.enemy_id = id;
            this.hp = hp;
            this.speed = speed;
        }
    }
}