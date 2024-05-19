using SHS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHS 
{
    public class EnemyAttackBullet : MonoBehaviour
    {
        Player_HpManager pd;

        public int damamge;

        private void Start()
        {
            pd = Player_HpManager.instance;
        }

        [Header("공격 세팅")]
        [SerializeField] LayerMask player_layer;
        [SerializeField] float attack_radius = 1f;

        private void Update()
        {
            if (Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y), attack_radius, player_layer))
            {
                pd.Damaged(damamge);
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attack_radius);
        }

    }
}