using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SpriteColorControl : MonoBehaviour
{
    [SerializeField] Animator m_ani;

    public void Damaged_Red()
    {
        m_ani.SetTrigger("damaged_red");
    }
}
