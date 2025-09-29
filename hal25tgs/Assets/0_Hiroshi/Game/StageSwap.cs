using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TurnManager;

public class StageSwap : MonoBehaviourPun
{
    public enum STADIUM_TYPE
    {
        NORMAL = 0, // 基本
        LIGHTNING,  // 雷
        DOUBLEGOAL, // ゴール２倍
        HOLE,       // 穴
        FROST,      // 氷
        WARP,       // ワープ

        STADIUM_TYPE_MAX,
    }

    [Serializable]
    public class Stadiums
    {
        [SerializeField] public STADIUM_TYPE m_StadiumType; // このスタジアムタイプ
        [SerializeField] public string m_PrefabName; // このスタジアムのプレハブ名
    };

    [Header("全スタジアム設定")]
    [SerializeField] private Stadiums[] m_Stadiums;

    [Header("スタジアム出現位置")]
    [SerializeField] private Vector3 m_Position = new Vector3(0.0f, -5.0f, 0.0f);

    [Header("現在のスタジアム")]
    [SerializeField] private GameObject m_CurrentStadium;

    private string m_CreateObjectName;    // スタジアム切り替えプレハブ用変数

    [SerializeField] bool m_IsSwap = false;         // 切り替えフラグ

    private STADIUM_TYPE m_NextType=STADIUM_TYPE.LIGHTNING;                // 次に切り替えるスタジアムタイプ
    private STADIUM_TYPE m_CurrentType;
    private TurnManager m_TurnManager;  // ターンマネージャー取得用

    // スタジアム切替時に発生するイベント。登録した関数は引数にSTADIUM_TYPEを受け取れる
    public event Action<STADIUM_TYPE> OnStadiumChanged;  

    [SerializeField] private bool m_TestFrag = false;

    private Dictionary<Keeper, Vector3> m_OldKeeperPositions = new Dictionary<Keeper, Vector3>(); // キーパーたちの元の位置を記憶する変数

    bool m_IsStageSwap = false; // キーパー移動フラグ

    [SerializeField] private FadeInOut m_Fade;
    private bool m_FadeFrag = false;

    // Start is called before the first frame update
    void Start()
    {
        // 現在のスタジアム(最初に設定されているスタジアム)を生成しておく
        //        Instantiate(m_CurrentStadium, gameObject.transform.position, Quaternion.identity);
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient) return;  // マスタークライアントでなければなにもしない
        }

        // ターンマネージャーを取得して、ターン時に発生するイベントにステージ切替関数を登録しておく
        m_TurnManager = TurnManager.Instance;
        m_TurnManager.OnTurnChanged += Swap;
        m_TurnManager.OnTurnChanged += FadeStageSwapInterval;

        // スコアマネージャーで、点が入った時にステージ切り替え（ランダム）フラグをtrueにする
        ScoreManager.Instance.OnScoreChanged += m_TurnManager.StageSwap;
        ScoreManager.Instance.OnScoreChanged += RandomSetNextStadium;

        // ダブルゴールに変わった時に、キーパーの位置をスタジアム外に移動する
        OnStadiumChanged += InvokeKeeperFlyaway;        

    }

    private void OnDisable()
    {
        if (!PhotonNetwork.IsMasterClient) return;  // マスタークライアントでなければなにもしない
        m_TurnManager.OnTurnChanged -= Swap;
        m_TurnManager.OnTurnChanged -= FadeStageSwapInterval;
        ScoreManager.Instance.OnScoreChanged -= m_TurnManager.StageSwap;

        OnStadiumChanged -= InvokeKeeperFlyaway;
    }
    private void Update()
    {
        //ここで、m_CurrentStageがnullの場合、
        //Tagで現在のステージを検索して格納するプログラムが欲しいかな
        if (m_CurrentStadium ==null)
        {
            m_CurrentStadium = GameObject.FindGameObjectWithTag("Stadium");
        }



        if (m_TestFrag)
        {
            m_TurnManager.StageSwap();
//        RandomSetNextStadium();
  //          Swap(TURN_STATE.STADIUMSWAP_INTERVAL);
            m_TestFrag = false;
        }




    }

    // 切替するかチェック関数
    private bool SwapCheck(TURN_STATE _state)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient) return false;  // マスタークライアントでなければなにもしない
        }


        if (!m_IsSwap) return false;   // 切替フラグがtrueでなければ切替ない
        if (_state != TURN_STATE.STADIUMSWAP_INTERVAL) return false;  // ステージ切替
        Debug.Log(112);
        RandomSetNextStadium();

        // 次のタイプが間違っているか、現在のスタジアムタイプと同じなら何もしない もう一度抽選
        if (m_NextType >= STADIUM_TYPE.STADIUM_TYPE_MAX || m_NextType == m_CurrentType)// return false;
        {
            RandomSetNextStadium();
        }

        Debug.Log("ステージ切替可能です。");
        return true;
    }

    private void Swap(TURN_STATE _state)
    {
        Debug.Log("return前");
        if (!SwapCheck(_state)) return; //チェックしてfalseなら切替ない
        Debug.Log("return後");

        for (int i = 0; i < m_Stadiums.Length; i++)
        {
            if (m_NextType == m_Stadiums[i].m_StadiumType)
            {
                m_CreateObjectName = m_Stadiums[i].m_PrefabName; // 生成用変数に次のスタジアムのプレハブ登録
                break;
            }

            if (i == m_Stadiums.Length) return;   // 見つからなかった場合は切替ない
        }

        // 現在のスタジアムを削除する用変数に移す
        GameObject destroyObject = m_CurrentStadium;
        m_CurrentStadium = null;

        // 次のスタジアムを生成して現在のスタジアムに登録
//        m_CurrentStadium = PhotonNetwork.Instantiate(m_CreateObjectName, m_Position, Quaternion.identity);
        m_CurrentStadium = SingleMultiUtility.Instantiate(m_CreateObjectName, m_Position, Quaternion.identity);


        // 先ほどまで使っていたスタジアムを削除
        //        PhotonNetwork.Destroy(destroyObject);
        destroyObject.tag = "Untagged";
        SingleMultiUtility.Destroy(destroyObject);

        // 現在のスタジアムタイプを更新,スタジアム切替イベント発生
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("StadiumSync", RpcTarget.All, m_NextType);
        }
        else
        {
            StadiumSync(m_NextType);
        }

        Debug.Log(m_CreateObjectName.ToString() + "を生成しました。");
    }

    // スタジアム切替設定関数 ========================
    // これをどこかで呼ぶ
    public void SetNextStadium(STADIUM_TYPE _type)
    { 
        m_NextType = _type;
        m_IsSwap = true;

        Debug.Log(m_NextType.ToString() + "を生成しようとしています。");
    }

    // スタジアム切替ランダム設定関数 ========================
    // これをどこかで呼ぶ
    public void RandomSetNextStadium()
    {
        int type = UnityEngine.Random.Range(0, (int)STADIUM_TYPE.STADIUM_TYPE_MAX);

        if (m_Stadiums.Length < (int)STADIUM_TYPE.STADIUM_TYPE_MAX - 1)
        {
            type = UnityEngine.Random.Range(0, m_Stadiums.Length);
        }
        m_NextType = (STADIUM_TYPE)type;
        m_IsSwap = true;

        Debug.Log(m_NextType.ToString() + "を生成しようとしています。");
    }

    // 現在のスタジアム取得 ========================
    public STADIUM_TYPE GetCurrentStadium()
    {
        return m_CurrentType;
    }

    // 現在のスタジアムタイプ同期用
    [PunRPC]
    private void StadiumSync(STADIUM_TYPE _type)
    {
        m_CurrentType = _type;
        m_NextType = STADIUM_TYPE.STADIUM_TYPE_MAX;
        OnStadiumChanged?.Invoke(m_CurrentType);    // スタジアム切替イベント発生
    }

    private void InvokeKeeperFlyaway(StageSwap.STADIUM_TYPE _type)
    {
        if (PhotonNetwork.IsConnected)
        {
            KeeperFlyAway(_type);
            photonView.RPC("KeeperFlyAway", RpcTarget.All, _type);
        }
        else
        {
            KeeperFlyAway(_type);
        }
    }

    // スタジアムがダブルゴール時にキーパーの位置を変更する関数
    [PunRPC]
    private void KeeperFlyAway(StageSwap.STADIUM_TYPE _type)
    {
        m_IsStageSwap = true;

        // 両チームのキーパーのpositionを取得
        Keeper[] keepers = FindObjectsByType<Keeper>(FindObjectsSortMode.None); 

        foreach (Keeper keeper in keepers)
        {
            if (_type == StageSwap.STADIUM_TYPE.DOUBLEGOAL)
            {
                if (!m_OldKeeperPositions.ContainsKey(keeper))
                {
                    // キーパーの元のpositionを保存
                    m_OldKeeperPositions[keeper] = keeper.transform.position;
                }

                // キーパーのposition.zに-100を加算
                Vector3 newPosition = keeper.transform.position + new Vector3(0f, 0f, -100f);
                keeper.transform.position = newPosition;
                //Debug.Log($"Keeper moved to {newPosition}");
            }
            else
            {
                // ダブルゴール以外なら元に戻す
                if (m_OldKeeperPositions.ContainsKey(keeper))
                {
                    Vector3 oldPos = m_OldKeeperPositions[keeper];
                    keeper.transform.position = oldPos;
                    Debug.Log($"Keeper returned to {oldPos}");
                }
            }
        }

        m_IsStageSwap = false;
    }


    private void FadeStageSwapInterval(TURN_STATE _state)
    {
        if (_state == TURN_STATE.STADIUMSWAP_INTERVAL)
        {
            m_FadeFrag = true;
            m_Fade.SetFadeMode(FadeInOut.FadeMode.Out);
            m_Fade.FadeStart();
        }
        else if (_state == TURN_STATE.PLAY_TURN && m_FadeFrag == true)
        {
            m_FadeFrag = false;
            m_Fade.SetFadeMode(FadeInOut.FadeMode.In);
            m_Fade.FadeStart();
        }

    }
}
