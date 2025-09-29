using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TurnManager;
using static UnityEngine.ParticleSystem;

// セットターンを同期関数にする
// セットターン内でタイマー起動行う
// セットターンを呼ぶ関数をクライアントマネージャーだけがタイマーにイベント登録
// イベントで全プレイヤーの同期関数を呼んでターン遷移

public class TurnManager : MonoBehaviourPun
{
    // ターンのタイプ
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

    static public TurnManager Instance { get; private set; }  // シングルトン化

    [Header("ゲーム全体タイマー")]
    [SerializeField] private Timer m_GameTimer;
    // ゲーム全体タイマー終了　かつ　操作ターンイベント発生したら　ゲーム終了

    [Header("ターン計測タイマー")]
    [SerializeField] private Timer m_TurnTimer;

    [Header("試合終盤判定時間")]
    [SerializeField] private float m_FinalPhase;    // 終盤として扱う時間

    [Header("時間設定")]
    [SerializeField] private float m_PlayTurnTime = 10.0f; // 操作ターン
    [SerializeField] private float m_ActiveTurnTime = 5.0f;   // 駒動作ターン
    [SerializeField] private float m_BurstTurnTime = 20.0f;    // リアルタイムモード
    [SerializeField] private float m_TurnIntervalTime = 0.0f;    // ターン間の待機時間
    [SerializeField] private float m_StadiumSwapIntervalTime = 1.0f;    // ステージ切り替え時の待機時間

    [SerializeField] private int m_TurnCount = 0;

    private TeamObject m_Team;

    private bool m_NextBurst = false;   // 次ターンバーストタイムにするか

    private bool m_StageSwap = false;   // ステージ切り替え発生したか
    private bool m_GameEnd = false;   // ゲーム終了できるか

    private TURN_STATE m_CurrentTurn = TURN_STATE.TURN_INTERVAL;   // 現在のターンタイプ
    private TURN_STATE m_NextTurn = TURN_STATE.PLAY_TURN;   // 次のターンタイプ

    // イベント一覧
    // ======================================================================================================
    public event Action<TURN_STATE> OnTurnChanged;  // ターン切替時に発生するイベント
    public event Action OnFinalPhase;   // ゲーム終盤に差し掛かった時に発生するイベント
    public event Action OnGameEnd;   // ゲーム終了時発生するイベント
    // ======================================================================================================

    [SerializeField] private bool m_BurstTest = false;
    private bool m_Active = false;  // 消す予定

    [SerializeField] AITeamObject m_AITeam;

    private float m_ErrorCheckTimer = 1.0f;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("既に TurnManager が存在しています。重複したものを破棄します。");
            Destroy(this.gameObject);
            return;
        }


        OnTurnChanged += TurnCount; // ターン切替イベント発生時に操作ターンであればターン数加算関数実行
    }
    private void Start()
    {
        /// <summary>
        /// ターン制御開始
        /// ゲームマネージャー側で、ゲームが開始されたら実行
        /// </summary>
//        TurnStart();

        if (!PhotonNetwork.IsConnected)
        {
            m_AITeam = GameObject.FindFirstObjectByType<AITeamObject>();
            m_AITeam.AssignItemChasers();
            m_AITeam.PrepareAllEnemies();
        }
    }
    private void FixedUpdate()
    {
        //        TurnTimerCheck();
        if (m_Team != null)
        { 
GameObject obj = GameObject.FindWithTag("TeamObject");
            m_Team =obj.GetComponent<TeamObject>();
        }
    }

    private void TurnTimerCheck()
    {
        if (TimerError())
        {
            TurnChanged();
//            SetNextTurn(m_CurrentTurn, 3.0f);
        }
    }

    private bool TimerError()
    {
        if (m_ErrorCheckTimer == m_TurnTimer.GetNowTime())
        {
            return true;
        }

        m_ErrorCheckTimer=m_TurnTimer.GetNowTime();

        return false;
    }
    /// <summary>
    /// イベント起動・追加・削除系
    /// </summary>
        // ターン管理タイマー終了イベント発生時ターン切替関数組み込み
    private void SetTurnLoop()
    {
        if (PhotonNetwork.IsConnected)
        {
            // マスタークライアントでなければ何もしない
            if (!PhotonNetwork.IsMasterClient) return;
        }

        m_TurnTimer.OnTimerEnd += TurnChanged;  // ターン用タイマーの終了イベントにターン切り替え関数追加
        m_GameTimer.OnEventTime += FinalPhaseEvent;     // ゲーム全体タイマーの終盤時間発生イベントに、終盤突入イベント起動関数追加
    }

// 終盤突入イベント起動関数呼出用関数
    private void FinalPhaseEvent()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("PhotonFinalPhaseEvent", RpcTarget.All);
        }
        else
        {
            PhotonFinalPhaseEvent();
        }
    }

    // 終盤突入イベント起動
    [PunRPC]
    private void PhotonFinalPhaseEvent()
    {
        OnFinalPhase?.Invoke();
        Debug.Log("ゲーム終盤イベント発生！");
    }

    // ゲーム全体タイマー終了イベント発生時にこのクラスからイベントを発生させる
    private void SetGameTimer()
    {
        m_GameTimer.TimerStart();
        m_GameTimer.StopTimer();
        m_GameTimer.OnTimerEnd += GameTimerEnd;
    }

    private void OnDisable()
    {
        m_TurnTimer.OnTimerEnd -= TurnChanged;
        m_GameTimer.OnTimerEnd -= GameTimerEnd;
        m_GameTimer.OnEventTime -= FinalPhaseEvent;

        OnTurnChanged -= TurnCount;
    }

    // ターン切り替え同期・非同期関数呼び出し用関数
    private void TurnChanged()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            photonView.RPC("AllPlayerTurnChanged", RpcTarget.All);
        }
        else
        {
            AllPlayerTurnChanged();
        }
    }

    /// <summary>
    /// ターン切替
    /// 操作 → 駒移動orリアルタイム → 操作 ループ
    /// </summary>
    [PunRPC]
    private void AllPlayerTurnChanged()
    {
        if (m_CurrentTurn >= TURN_STATE.PLAY_TURN &&
            m_CurrentTurn <= TURN_STATE.BURST_TURN)
        {
            // 現在ターンを次ターンに更新する。（現在をインターバルにする）
            ChangedPlayerTurn();// 次ターンをインターバル以外にする
            StopGameTimer();    // 全体タイマー停止
        }
        else
        {
            // 現在がインターバルのとき
            /*
            if (m_StageSwap)
            {
                // ステージ切替フラグtrueなら"現在はステージ切替インターバル中"
                m_StageSwap = false;    // フラグをfalseにする
                SetTurn();                 // 現在ターンを次ターンに更新する。（現在をインターバルにする）
                SetNextTurn(TURN_STATE.PLAY_TURN, m_TurnIntervalTime);  // 次ターンを駒操作ターンにする
                return;
            }*/

            SetTurn();  // 現在ターンを次ターンに更新する。（現在をインターバル以外にする）
            SetNextTurn(TURN_STATE.TURN_INTERVAL, m_TurnIntervalTime);            // 次ターンをインターバルにする
            RestartGameTimer(); // 全体タイマー再開
        }
    }

    // 次ターンがインターバルかステージ切り替えのとき
    private void ChangedPlayerTurn()
    {
        if (m_StageSwap)
        {
            // ステージ切り替えフラグがtrueなら、
            // 通常インターバルをはさんでステージ切り替えインターバルにする
            SetTurn();  // 次のターンを現在のターンに切替
            SetNextTurn(TURN_STATE.STADIUMSWAP_INTERVAL, m_StadiumSwapIntervalTime);
            m_StageSwap = false;
        }
        // 現在が操作ターンのとき
        else if (m_CurrentTurn == TURN_STATE.PLAY_TURN)
        {
            // 現在をインターバルターンにする
            SetTurn();  // 次のターンを現在のターンに切替

            // リアルタイムモードフラグtrueなら次ターンをリアルタイムモードに
            if (m_NextBurst)
            {
                Debug.Log("1");
                SetNextTurn(TURN_STATE.BURST_TURN, m_BurstTurnTime);
                m_NextBurst = false;
                Debug.Log("2");
            }
            else
            {
                // そうでなければ次ターンを駒動作ターンに
                SetNextTurn(TURN_STATE.ACTIVE_TURN, m_ActiveTurnTime);
            }

        }
        else
        {
            // ゲーム全体タイマーが終了していて、駒動作ターンが終了したら終了
            if (m_GameEnd)
            {
                // ターン切替イベント発生
                OnGameEnd?.Invoke();
                GameEnd();
                return;
            }
            else
            {
                // 駒操作ターンでなければ、次ターンを駒操作タ
                // ーンに
                SetTurn();  // 次のターンを現在のターンに切替
                SetNextTurn(TURN_STATE.PLAY_TURN, m_PlayTurnTime);
            }
        }
    }

    // 次のターンの設定
    private void SetNextTurn(TURN_STATE _nextState, float _nextTime)
    {
        m_NextTurn = _nextState;    // 次のターンを設定

        // ターン切替イベント発生
        OnTurnChanged?.Invoke(m_CurrentTurn);
        m_TurnTimer.TimerStart();   // タイマー起動

        m_TurnTimer.SetMaxTimer(_nextTime); // 次のターンの時間を設定
    }

    // 次のターン設定を現在のターンに反映して起動
    [PunRPC]
    private void SetTurn()
    {
        m_CurrentTurn = m_NextTurn;  // 現在のターンを切替
        Debug.Log("現在のターン:" + m_CurrentTurn.ToString());



        if (m_CurrentTurn == TURN_STATE.ACTIVE_TURN)
        {
            if (m_Team == null)
            {
                Debug.LogError("m_Team が null");
                return;
            }

            var players = m_Team.GetPlayersArray();
            if (players == null)
            {
                Debug.LogError("GetPlayersArray() が null");
                return;
            }

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    Debug.LogError($"players[{i}] が null");
                    continue;
                }

                PlayerMove playerMove = players[i].GetComponent<PlayerMove>();
                if (playerMove == null)
                {
                    Debug.LogError($"players[{i}] に PlayerMove コンポーネントが付いていません");
                    continue;
                }

                // プレイヤー駒側で、イベント発生時に実行させたい
                // バーストイベント発生時：バーストフラグをプレイヤーに付与し、バーストイベント発生時にtrueにして、trueのときはプレイヤー操作後すぐ実行
                // バースト以外イベント発生時にはfalseにしておくとか？

                playerMove.Move();
                if (!PhotonNetwork.IsConnected)
                {
                    m_AITeam.ExecuteAllEnemies();
                }
            }
        }
        else if(m_CurrentTurn == TURN_STATE.PLAY_TURN)
        {
            if (!PhotonNetwork.IsConnected)
            {
                Debug.Log("AISet");
                m_AITeam.FaceAllEnemies();
                m_AITeam.AssignItemChasers();
                m_AITeam.PrepareAllEnemies();
            }
        }


        //
        // 一番下の関数消したら消す予定
        switch (m_CurrentTurn)
        {
            case TURN_STATE.PLAY_TURN:
                m_Active = false;
                m_NextBurst = false;
                break;

            case TURN_STATE.ACTIVE_TURN:
                m_Active = true;
                m_NextBurst = false;
                break;

            case TURN_STATE.BURST_TURN:
                m_Active = true;
                m_NextBurst = true;
                break;
        }
    }

    // ターン管理を終了させる
    private void GameEnd()
    {
        m_CurrentTurn = TURN_STATE.GAME_END;
        m_NextTurn = TURN_STATE.GAME_END;
        m_NextBurst = false;

        m_TurnTimer.StopTimer();
        Debug.Log(m_CurrentTurn.ToString());
    }

    // ターン制御開始用初期化
    [PunRPC]
    private void TurnInit()
    {
        m_CurrentTurn = TURN_STATE.TURN_INTERVAL;
        m_NextTurn = TURN_STATE.TURN_INTERVAL;
        // 最初にタイマー起動しないようにしておく
        m_TurnTimer.StopTimer();
        // 次のターン
        SetNextTurn(TURN_STATE.TURN_INTERVAL, m_TurnIntervalTime);
        SetTurn();  // ここでタイマー起動する
                    //               TurnChanged();
        SetNextTurn(TURN_STATE.PLAY_TURN, m_PlayTurnTime);
        SetTurnLoop();  // タイマー終了イベント発生時にターン切替がループするよう設定
        SetGameTimer();

        m_TurnCount = 0;

        m_AITeam = GameObject.FindFirstObjectByType<AITeamObject>();
        m_AITeam.AssignItemChasers();
        m_AITeam.PrepareAllEnemies();

        Debug.Log("ターン初期化完了");
    }

    private void TurnCount(TURN_STATE _state)
    {
        if (_state != TURN_STATE.PLAY_TURN) return;
        m_TurnCount++;
    }

    private void StopGameTimer()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("PhotonSyncGameTimer", RpcTarget.All, m_GameTimer.GetNowTime());
                photonView.RPC("PhotonStopGameTimer", RpcTarget.All);
            }
        }
        else
        {
            PhotonStopGameTimer();
        }
    }

    private void RestartGameTimer()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("PhotonSyncGameTimer", RpcTarget.All, m_GameTimer.GetNowTime());
                photonView.RPC("PhotonRestartGameTimer", RpcTarget.All);
            }
        }
        else
        {
            PhotonRestartGameTimer();
        }
    }
    // 外部から設定系 ===================================================

    /// <summary>
    /// ターン制御開始
    /// ゲームマネージャー側で、ゲームが開始されたら実行
    /// </summary>
    public void TurnStart()
    {
        Debug.Log("TurnStart起動");
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            photonView.RPC("TurnInit", RpcTarget.All);
        }
        else
        {
            TurnInit();


        }

        Debug.Log("ターンスタート");
    }

    // スコア上限などでゲームが終了になった際、呼び出すと次に操作ターンになった際、
    // ゲーム全体タイマーを終了させるフラグ
    // ターン管理も終了させる
    public void GameTimerEnd()
    {
        m_GameEnd = true;
    }


    // 次駒操作ターン終了時にバーストタイム起動（ターン制一時廃止）
    public void BurstTimeStart()
    {
        m_NextBurst = true;
        Debug.Log("burst active");
    }

    public void StageSwap()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            { 
                photonView.RPC("PunStageSwap",RpcTarget.All);
            }
        }
        else
        {
            PunStageSwap();
        }
    }

    [PunRPC]
    public void PunStageSwap()
    {
        m_StageSwap = true;
    }

    public void SetTeamObject(TeamObject _teamObject)
    {
        m_Team = _teamObject;
    }


    // 取得系 ===================================================

    // 現在のターンの状態取得
    public TURN_STATE GetCurrentTurnState()
    {
        return m_CurrentTurn;
    }

    // 次のターンの状態取得
    public TURN_STATE GetNextTurnState()
    {
        /*
        if (m_CurrentTurn == TURN_STATE.ACTIVE_TURN)
        {
            // 現在ターンが駒動作ターンで、
            // バーストタイムフラグがtrueだったら次ターンはバーストタイムだと教える
            if (m_NextBurst) return TURN_STATE.BURST_TURN;
        }*/
        return m_NextTurn;
    }

    // 次の駒動作ターンがバーストタイムになるかどうか
    public bool IsNextTurnBurst()
    {
        return m_NextBurst;
    }
    // 現在のターンの残り時間取得
    public Timer GetTurnTimer()
    {
        return m_TurnTimer;
    }
    // 現在のゲーム全体の残り時間取得
    public Timer GetGameTimer()
    {
        return m_GameTimer;
    }

    public int GetTurnCount()
    { 
        return m_TurnCount;
    }

    

    // Photon同期系 ===================================================
    [PunRPC]
    void SyncTurn(int turn)
    {
        m_CurrentTurn = (TURN_STATE)turn;
    }

    [PunRPC]
    void PhotonSyncGameTimer(float _time)
    {
        m_GameTimer.SyncTime(_time);
    }
    [PunRPC]
    void PhotonStopGameTimer()
    {
        m_GameTimer.StopTimer();
    }
    [PunRPC]
    void PhotonRestartGameTimer()
    { 
        m_GameTimer.ReStartTimer();
    }

    /// <summary>
    /// 以下消す予定
    /// </summary>
    /// <returns></returns>

    private void TurnSwitch()
    {
        if (!m_TurnTimer.TimerEnd()) return;

        if (m_Active)
        {
            // 駒移動フェーズ終了時
            // 操作タイムに切替
            PhaseSwitch(false, false, m_PlayTurnTime);

        }
        else if (!m_Active)
        {
            // 操作フェーズ終了時

            if (m_NextBurst)
            {
                // バーストフェーズに切替
                PhaseSwitch(true, true, m_BurstTurnTime);

                return;
            }

            // 駒動作タイムに切替
            PhaseSwitch(true, false, m_ActiveTurnTime);
        }
    }

    // リアルタイムモードとターンモード変更
    private void PhaseSwitch(bool _active, bool _burst, float _time)
    {
        m_Active = _active;
        m_NextBurst = _burst;
        m_TurnTimer.SetMaxTimer(_time);
        m_TurnTimer.TimerStart();   // タイマー起動

        // 操作タイムに切替の場合はここで終了
        if (!m_Active) return;

        //for (int i = 0; i < m_CreatePlayer.m_Players.Length; i++)
        //{
        //    // プレイヤー生成したスクリプトからプレイヤームーブ取得
        //    PlayerMove playerMove = m_CreatePlayer.m_Players[i].GetComponent<PlayerMove>();
        //    playerMove.Move();
        //}


        //        for(int i = 0; i < m_Team.GetPlayers().Count;i++)
        for (int i = 0; i < m_Team.GetPlayersArray().Length; i++)
        {
            //            PlayerMove playerMove = m_Team.GetPlayers()[i].GetComponent<PlayerMove>();
            PlayerMove playerMove = m_Team.GetPlayersArray()[i].GetComponent<PlayerMove>();
            playerMove.Move();
        }
    }
    // 矢印操作時間中か取得
    public bool IsPlayTimeTurn()
    {
        return !m_Active;
    }

    // 駒動作ターンか取得
    public bool IsActiveTimeTurn()
    {
        return m_Active;
    }

    public bool IsBurstTimeActive()
    {
        // バーストタイムかつ駒動作可能か
        return m_NextBurst && m_Active;
    }
    /*
    private void TurnSwap()
    {
        if (m_NextTurn > TURN_STATE.TURN_TYPE_MAX)
        {
            m_CurrentTurn++;
            m_NextTurn++;
            Debug.Log(m_CurrentTurn.ToString());
        }
        else
        {
            TURN_STATE state = m_CurrentTurn;
            m_CurrentTurn = m_NextTurn;
            m_NextTurn = state + 2;
        }
        // ターン切替イベント発生
        OnTurnChanged?.Invoke(m_CurrentTurn);
        m_TurnTimer.TimerStart();   // タイマー起動
    }
    */

    private void Update()
    {
        if (m_BurstTest)
        {
            m_NextBurst = true;
        }
        if (!m_NextBurst)
        {
            m_BurstTest = false;
        }
    }
}
