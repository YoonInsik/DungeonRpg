using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    Animator m_ani;

    // Start is called before the first frame update
    void Start()
    {
        m_ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoinDrop()
    {
        m_ani.SetTrigger("goon");
    }

}
