using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager_AI_Item : MonoBehaviour
{
    [SerializeField] private Timer_AI m_Timer;
    [SerializeField] private EnemyAI_Item[] m_Enemies;

    [Header("時間設定")]
    [SerializeField] private float m_PlayPhaseTime;
    [SerializeField] private float m_ActivePhaseTime;
    [SerializeField] private float m_BurstPhaseTime;

    [SerializeField] private CreatePlayer m_CreatePlayer;

    private bool m_Active = false;
    private bool m_Burst = false;   // バーストタイム中か

    // Start is called before the first frame update
    void Start()
    {
        m_Timer.SetMaxTimer(m_PlayPhaseTime);
        m_Timer.TimerStart();
        Debug.Log("操作フェーズ開始");

        foreach (var e in m_Enemies)
        {
            e.PrepareMove();
            Debug.Log("Prepare");
        }
    }

    // Update is called once per frame
    void Update()
    {
        TurnSwitch();
    }

    private void TurnSwitch()
    {
        if (!m_Timer.TimerEnd()) return;

        if (m_Active)
        {
            // 駒移動フェーズ終了時
            // 操作タイムに切替
            PhaseSwitch(false, false, m_PlayPhaseTime);

            foreach(var e in m_Enemies)
            {
                e.FacePlayer();
                e.PrepareMove();
            }

            Debug.Log("操作フェーズ");
        }
        else if (!m_Active)
        {
            // 操作フェーズ終了時

            if (m_Burst)
            {
                // バーストフェーズに切替
                PhaseSwitch(true, true, m_BurstPhaseTime);
                Debug.Log("バーストタイム");

                return;
            }

            // 駒動作タイムに切替
            PhaseSwitch(true,false,m_ActivePhaseTime);
            Debug.Log("駒動作フェーズ");
        }
    }

    // 矢印操作時間中か取得
    public bool GetPlayTimeTurn()
    { 
        return !m_Active;
    }

    public Timer_AI GetUseTimer()
    {
        return m_Timer;
    }

    private void PhaseSwitch(bool _active,bool _burst,float _time)
    {
        m_Active = _active;
        m_Burst = _burst;
        m_Timer.SetMaxTimer(_time);
        m_Timer.TimerStart();   // タイマー起動

        // 操作タイムに切替の場合はここで終了
        if (!m_Active) return;

        foreach(var enemy in m_Enemies)
        {
            enemy.Move();
        }
        
    }

    public bool IsBurstTimeActive()
    {
        // バーストタイムかつ駒動作可能か
        return m_Burst && m_Active;
    }

    // バーストタイム起動（ターン制一時廃止）
    public void BurstTimeStart()
    {
        m_Burst = true;
    }
}
