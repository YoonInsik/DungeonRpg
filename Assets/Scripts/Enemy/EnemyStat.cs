using System;
using UnityEngine;

namespace SHS
{ 
    [CreateAssetMenu(fileName = "EnemyStat", menuName = "Enemy/EnemyStat")]
    public class EnemyStat : ScriptableObject
    {
        public EnemyType enemytype;
        public float hp;
        public float speed;
        public int damage;

        public EnemyStat(EnemyType enemytype, float hp, float speed, int damage)
        {
            this.enemytype = enemytype;
            this.hp = hp;
            this.speed = speed;
            this.damage = damage;
        }
    }
}