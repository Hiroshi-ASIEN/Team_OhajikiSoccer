using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager_AI : MonoBehaviour
{
    public enum TURN_STATE
    {
        PLAY_TURN = 0,
        ACTIVE_TURN,
        STADIUMSWAP_INTERVAL,
        BURST_TURN,

        GAME_END,
        TURN_INTERVAL,

        TURN_TYPE_MAX
    };

    static public TurnManager_AI Instance { get; private set; }  // シングルトン化

    //エネミー
    //[SerializeField] private EnemyAI[] m_Enemies;
    private AITeamObject m_AITeam;

    [Header("ゲーム全体タイマー")]
    [SerializeField] private Timer_AI m_Timer;

    [Header("ターン計測タイマー")]
    [SerializeField] private Timer m_TurnTimer;

    [Header("試合終盤判定時間")]
    [SerializeField] private float m_FinalPhase;

    [Header("時間設定")]
    [Header("時間設定")]
    [SerializeField] private float m_PlayTurnTime = 10.0f; // 操作ターン
    [SerializeField] private float m_ActiveTurnTime = 5.0f;   // 駒動作ターン
    [SerializeField] private float m_BurstTurnTime = 20.0f;    // リアルタイムモード
    [SerializeField] private float m_TurnIntervalTime = 3.0f;    // ターン間の待機時間
    [SerializeField] private float m_StadiumSwapIntervalTime = 5.0f;    // ステージ切り替え時の待機時間

    [SerializeField] private CreatePlayer m_CreatePlayer;

    private bool m_Active = false;
    private bool m_Burst = false;   // バーストタイム中か

    // Start is called before the first frame update
    void Start()
    {
        m_Timer.SetMaxTimer(m_PlayTurnTime);
        m_Timer.TimerStart();
        Debug.Log("操作フェーズ開始");

        //foreach (var e in m_Enemies)
        //{
        //    e.PrepareMove();
        //    Debug.Log("Prepare");
        //}

        m_AITeam = GameObject.FindFirstObjectByType<AITeamObject>();
        m_AITeam.AssignItemChasers();
        m_AITeam.PrepareAllEnemies();
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
            PhaseSwitch(false, false, m_PlayTurnTime);

            //foreach(var e in m_Enemies)
            //{
            //    e.FacePlayer();
            //    e.PrepareMove();
            //}

            m_AITeam.FaceAllEnemies();
            m_AITeam.AssignItemChasers();
            m_AITeam.PrepareAllEnemies();

            Debug.Log("操作フェーズ");
        }
        else if (!m_Active)
        {
            // 操作フェーズ終了時

            if (m_Burst)
            {
                // バーストフェーズに切替
                PhaseSwitch(true, true, m_BurstTurnTime);
                Debug.Log("バーストタイム");

                return;
            }

            // 駒動作タイムに切替
            PhaseSwitch(true,false, m_ActiveTurnTime);
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

        //foreach(var enemy in m_Enemies)
        //{
        //    enemy.Move();
        //}

        m_AITeam.ExecuteAllEnemies();
        
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
