using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using UnityEngine.UIElements;


//変更履歴　2025/05/09  河上尚哉
//
//・継承元をMonoBehaviourPunCallbacksに変更
//
//
//・メンバ変数に二つのチームオブジェクトを追加
//　二つのチームオブジェクトに、TeamObject内のSetGameManagerを用いて
//　自身を登録する
//
//
//・同期をしながら、非マスター側のGameManagerの
//　インベントリのアイテムを使用する関数


public class GameManager_S : MonoBehaviourPunCallbacks
{
    [SerializeField] private Timer m_GameTimer;
    [SerializeField] private SceneChanger m_SceneChanger;
    [SerializeField] private GameObject m_Ball;
    [SerializeField] private TurnManager m_TurnManagerPrefab;
    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    //チームオブジェクトを二つ保有している（ヒエラルキーから登録する）←生成時に登録しようか
    [Header("チームAのインスタンス　※要設定※")]
    [SerializeField] private TeamObject m_TeamObjectA;
    [Header("チームBのインスタンス　※要設定※")]
    [SerializeField] private TeamObject m_TeamObjectB;
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


    [Header("UI")]
    [SerializeField] Image m_TeamAUI;
    [SerializeField] Image m_TeamBUI;
    [SerializeField] List<Sprite> m_Sprite;

    GameObject m_MyTeamObject;
    GameObject m_EnemyTeamObject;
    Vector3 m_TeamObjectPos = Vector3.zero;
    GameObject[] m_TeamObjects = null;

    ScoreManager m_ScoreManager;

    private bool m_IsTimeUp = false;

    private void Start()
    {
        m_TeamObjects = new GameObject[2];

        // チームマネージャー生成
//       m_MyTeamObject = SingleMultiUtility.Instantiate("TeamManager", m_TeamObjectPos, Quaternion.identity);
        if (PhotonNetwork.IsConnected)  // ネットワークに接続している時はフォトンで生成
        {
            //これだとPlayerが参加した数だけTeamManagerが作られる
//            m_MyTeamObject = PhotonNetwork.Instantiate("TeamManager", m_TeamObjectPos, Quaternion.identity);
              m_MyTeamObject = SingleMultiUtility.InstantiateForClient("TeamManager", m_TeamObjectPos, Quaternion.identity);
        }
        else    // 接続していない時は自分だけ生成（AIは別で）
        {
            GameObject prefab = Resources.Load<GameObject>("TeamManager");
            m_MyTeamObject = GameObject.Instantiate(prefab, m_TeamObjectPos, Quaternion.identity);
            // シングルプレイ時はResourcesから読み込み
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        RegisterGameManagerToTeams();
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        //Test : GameManagerに自分のTeamを設定
        //TODO : 相手のTeamも登録

        TeamName teamName = AssignPlayersToTeams();
        if (teamName == TeamName.TeamA)
        {
            m_TeamObjectA = m_MyTeamObject.GetComponent<TeamObject>();
        }
        else
        {
            m_TeamObjectB = m_MyTeamObject.GetComponent<TeamObject>();
        }
        m_ScoreManager = ScoreManager.Instance;
        m_ScoreManager.ResetScores();

        if (!TurnManager.Instance)
        {
            Debug.Log("ないよ1");
            SingleMultiUtility.InstantiateForClient(m_TurnManagerPrefab.name, this.transform.position, Quaternion.identity);

            // ターンマネージャー設定・起動
            TurnManager.Instance.TurnStart();
            TurnManager.Instance.OnGameEnd += GameEnd;
            m_GameTimer.TimerStart();

            TurnManager.Instance.OnTurnChanged += ScoreCheck;
            return;
        }
        // ターンマネージャー設定・起動
        TurnManager.Instance.TurnStart();
        TurnManager.Instance.OnGameEnd += GameEnd;
        TurnManager.Instance.OnTurnChanged += ScoreCheck;

        m_GameTimer.TimerStart();

        Debug.Log("あるよ");
    }

    private void FixedUpdate()
    {
        if (!TurnManager.Instance)
        {
            SingleMultiUtility.InstantiateForClient(m_TurnManagerPrefab.name, this.transform.position, Quaternion.identity);

            // ターンマネージャー設定・起動
            TurnManager.Instance.TurnStart();
            TurnManager.Instance.OnGameEnd += GameEnd;
            m_GameTimer.TimerStart();

            m_ScoreManager = ScoreManager.Instance;
            m_ScoreManager.ResetScores();
            TurnManager.Instance.OnTurnChanged += ScoreCheck;
            Debug.Log("なかったよ");
        }

    }

    private void OnDisable()
    {
        TurnManager.Instance.OnGameEnd -= GameEnd;
        TurnManager.Instance.OnTurnChanged -= ScoreCheck;
    }

    private void Update()
    {
        //上手く作動してない
        //TeamAが敵の場合
        if (!m_TeamObjectA)
        {
            m_TeamObjects = GameObject.FindGameObjectsWithTag("TeamObject");
            Debug.Log(m_TeamObjects);
            for (int i = 0; i < m_TeamObjects.Length; i++)
            {
                //相手が生成したTeamManagerを取得
                if (!m_TeamObjects[i].GetComponent<PhotonView>().IsMine)
                {
                    m_TeamObjectA = m_TeamObjects[i].GetComponent<TeamObject>();
                }
            }
        }
        //TeamBが敵の場合
        else if (!m_TeamObjectB)
        {
            m_TeamObjects = GameObject.FindGameObjectsWithTag("TeamObject");
            Debug.Log(m_TeamObjects);
            for (int i = 0; i < m_TeamObjects.Length; i++)
            {
                //相手が生成したTeamManagerを取得
                if (!m_TeamObjects[i].GetComponent<PhotonView>().IsMine)
                {
                    m_TeamObjectB = m_TeamObjects[i].GetComponent<TeamObject>();
                }
            }
        }

        SetTeamUI();

//        GameTimeCount();
    }
    private void GameTimeCount()
    {
        if (m_IsTimeUp) return;

        if (m_GameTimer.TimerEnd())
        {
            m_IsTimeUp = true;  // タイマー終了したらタイムアップ
            GameEnd();
        }
    }

    private void ScoreCheck(TurnManager.TURN_STATE _state)
    {
        if (_state != TurnManager.TURN_STATE.PLAY_TURN) return;

        if (m_ScoreManager.GetMaxScore() <= m_ScoreManager.GetScore(TeamName.TeamA)
            || m_ScoreManager.GetMaxScore() <= m_ScoreManager.GetScore(TeamName.TeamB))
        {
            GameEnd();
        }
    }

    private void GameEnd()
    {
        //        if (!m_IsTimeUp) return;
        TurnManager.Instance.OnGameEnd -= GameEnd;
        TurnManager.Instance.OnTurnChanged -= ScoreCheck;
        m_SceneChanger.IsActive();  // シーン遷移起動
    }


    public TeamName AssignPlayersToTeams()
    {
        TeamName myTeamName = TeamName.None;

        if(!PhotonNetwork.IsConnected)
        {
            Debug.Log("ここでは" + ScoreManager.Instance.m_SoloTeamName);
            return ScoreManager.Instance.m_SoloTeamName;
        }

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("Team", out object teamValue))
            {
                TeamName team = (TeamName)(int)teamValue; // intをTeamName列挙型に変換
                Debug.Log($"プレイヤー {player.NickName} は {team} に所属");

                // 例: チームごとにスポーン位置を分ける
                if (player.IsLocal) // 自分自身の場合のみ適用
                {
                    myTeamName = (TeamName)(int)teamValue;  // 自分のチーム設定

                    if (team == TeamName.TeamA)
                        Debug.Log($"プレイヤー {player.NickName} のチーム：TeamA");
                    else if (team == TeamName.TeamB)
                        Debug.Log($"プレイヤー {player.NickName} のチーム：TeamB");
                }
            }
            else
            {
                Debug.Log($"プレイヤー {player.NickName} のチーム情報が見つかりません");
            }
        }
        return myTeamName;
    }



    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    /// <summary>
    /// 保有しているチームに自身の参照を登録
    /// </summary>
    private void RegisterGameManagerToTeams()
    {
        if (m_TeamObjectA)
            m_TeamObjectA.SetGameManager(this);
        if (m_TeamObjectB)
            m_TeamObjectB.SetGameManager(this);
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    /// <summary>
    /// 自分以外の参加者のゲームマネージャーへ、アイテムを使用したことを教える関数　
    /// </summary>
    /// <param name="teamObject">使用したチーム</param>
    /// <param name="_itemIndex">使用したアイテムのインベントリ番号</param>
    public void SendItemUsingToOtherTeam(TeamObject teamObject, int _itemIndex)
    {
        Debug.Log("SendItemUsingToOtherTeam関数が呼ばれました");

        //もしも使用しているチームがAならば
        if (teamObject == m_TeamObjectA)
        {
            Debug.Log(photonView);

            //1を引数に、もう一つのモニター側のゲームマネージャーから実行する
            photonView.RPC("UseItemRPC", RpcTarget.Others, 1, _itemIndex);
        }
        else
        {
            photonView.RPC("UseItemRPC", RpcTarget.Others, 2, _itemIndex);
        }
    }
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



    /// <summary>
    /// 相手参加者から呼び出される。相手が使用したアイテムをこちら世界でも使用する
    /// </summary>
    /// <param name="teamObject">使用したチーム</param>
    /// <param name="_itemIndex">使用したアイテムのインベントリ番号</param>
    [PunRPC]
    private void UseItemRPC(int _teamNum, int _itemIndex)
    {
        Debug.Log("UseItemRPC関数が呼ばれました");


        if (_teamNum == 1)
            //チームAで実行
            if (m_TeamObjectA)
                m_TeamObjectA.UseItemByGameManager(_itemIndex);

            else
            //チームBで実行
            if (m_TeamObjectB)
                m_TeamObjectB.UseItemByGameManager(_itemIndex);
    }


    [PunRPC]
    private void TestItemRPC()
    {

    }


    //自分がチームAかどうか（正直現在の識別方法はスマートではない。注意）
    public bool IsTeamA(TeamObject teamObject)
    {
        if(m_TeamObjectA)
        {
            if (m_TeamObjectA == teamObject)
                return true;
        }
        if(m_TeamObjectB)
        {
            if (m_TeamObjectB != teamObject)
                return true;
        }
        return false;
    }

    public TeamObject GetTeamObject(TeamName _teamName)
    {
        if (_teamName == TeamName.TeamA)
        {
            return m_TeamObjectA;
        }
        else if (_teamName == TeamName.TeamB)
        {
            return m_TeamObjectB;
        }

        Debug.Log("チーム情報がありません。");
        return null;
    }

    public GameObject GetBallObject()
    {
        return m_Ball;
    }
    public void SetTeamUI()
    {
        //UI設定
        //ソロ用
        if (!PhotonNetwork.IsConnected)
        {
            if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
            {
                Character character = m_TeamObjectA.GetCharacter();

                int num = (int)character;
                m_TeamAUI.sprite = m_Sprite[num];

            }
            else
            {
                Character character = m_TeamObjectB.GetCharacter();

                int num = (int)character;
                m_TeamBUI.sprite = m_Sprite[num];

            }
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            {//自分がマスターでTeamAを選んでいる時
                if(ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
                {
                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    ScoreManager.Instance.m_CharacterA = teamACharacter;
                    int num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];

                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    ScoreManager.Instance.m_CharacterB = teamBCharacter;
                    num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];
                }
                else if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamB)
                {
                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    ScoreManager.Instance.m_CharacterB = teamBCharacter;
                    int num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];

                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    ScoreManager.Instance.m_CharacterA = teamACharacter;
                    num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];

                }

            }
            else
            {
                //マスターではない場合
                if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamB)
                {
                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    int num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];

                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];
                }
                else if(ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
                {
                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    int num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];

                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];
                }


            }



        }
    }


}
