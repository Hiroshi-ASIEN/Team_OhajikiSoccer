using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviourPun
{
    [SerializeField] private ItemName m_ItemName;
    [SerializeField] private Timer m_Timer;
    public ItemName GetItemName()
    {
       return   m_ItemName;
    }
    private void Start()
    {
        m_Timer.TimerStart();
    }

    private void Update()
    {
        if (m_Timer.TimerEnd())
        { 
        Destroy(this.gameObject);
        }
    }
}
