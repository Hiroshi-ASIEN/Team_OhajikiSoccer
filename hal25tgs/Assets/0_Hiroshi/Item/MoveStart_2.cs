using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStart_2 : MonoBehaviourPunCallbacks
{
    private bool m_Move = false;    // プレイヤーが動くか

    [SerializeField] private CreatePlayer m_CreatePlayer;
    [SerializeField] private TurnManager m_TurnManager;
    Timer m_TMTimer;

    GameObject[] m_Players;
    [SerializeField] private bool m_Burst = false;   // バーストタイム中か

    // Start is called before the first frame update
    void Start()
    {
        m_TMTimer = m_TurnManager.GetTurnTimer();    // TMで使用中のタイマー取得
    }

    private void FixedUpdate()
    {
        if (!m_TurnManager.IsBurstTimeActive())
        {
            if (m_Burst) m_TurnManager.BurstTimeStart();
            m_Burst = false;
        }
    }

    // ターン制時の動作
    private void TurnTime()
    {
        // プレイヤー動作時間中　＋　プレイヤー発射してない
        if (!m_TurnManager.IsPlayTimeTurn() && !m_Move)
        {
            for (int i = 0; i < m_CreatePlayer.m_Players.Length; i++)
            {
                // プレイヤー生成したスクリプトからプレイヤームーブ取得
                PlayerMove playerMove = m_CreatePlayer.m_Players[i].GetComponent<PlayerMove>();
                playerMove.Move();
            }
            m_Move = true;  // プレイヤー発射
        }
        // 矢印操作時間中　＋　プレイヤー発射済み
        if (m_TurnManager.IsPlayTimeTurn() && m_Move)
        {
            m_Move = false; // プレイヤー発射可能にする
        }
    }

}
