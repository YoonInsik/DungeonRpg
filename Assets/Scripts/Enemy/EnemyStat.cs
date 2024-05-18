using System;
using UnityEngine;

namespace SHS
{ 
    [CreateAssetMenu(fileName = "EnemyStat", menuName = "Enemy/EnemyStat")]
    public class EnemyStat : ScriptableObject
    {
        public EnemyType enemytype;
        public float hp;
        public float hp_scaling;
        public float speed;
        public int damage;
        public int damage_scaling;

        public EnemyStat(EnemyType enemytype, float hp, float hp_scaling, float speed, int damage, int damage_scaling)
        {
            this.enemytype = enemytype;
            this.hp = hp;
            this.hp_scaling = hp_scaling;
            this.speed = speed;
            this.damage = damage;
            this.damage_scaling = damage_scaling;
        }
    }
}