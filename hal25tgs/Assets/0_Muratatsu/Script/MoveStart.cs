using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStart : MonoBehaviourPunCallbacks
{
    bool m_IsMove = false;

    CreatePlayer m_CreatePlayer;

    GameObject m_Timer;
    TurnManager m_TurnManager;


    // Start is called before the first frame update
    void Start()
    {
        m_Timer = GameObject.Find("TurnManager");
        m_TurnManager = m_Timer.GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!m_Timer)
        {
            m_Timer = GameObject.Find("TurnManager");
        }
        if (!m_TurnManager)
        {
            m_TurnManager = m_Timer.GetComponent<TurnManager>();
        }

        if (!m_TurnManager.IsPlayTimeTurn() && !m_IsMove)
        {
            m_CreatePlayer = GetComponent<CreatePlayer>();
            for(int i = 0; i < m_CreatePlayer.m_Players.Length;i++)
            {
                PlayerMove playerMove = m_CreatePlayer.m_Players[i].GetComponent<PlayerMove>();
                playerMove.Move();
            }
            m_IsMove = true;
        }
        if(m_TurnManager.IsPlayTimeTurn() && m_IsMove)
        {
            m_IsMove = false;
        }
    }
}
