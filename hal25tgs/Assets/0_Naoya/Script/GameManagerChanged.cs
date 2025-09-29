//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


////変更履歴　2025/05/09  河上尚哉
////
////・継承元をMonoBehaviourPunCallbacksに変更
////
////
////・メンバ変数に二つのチームオブジェクトを追加
////　二つのチームオブジェクトに、TeamObject内のSetGameManagerを用いて
////　自身を登録する
////
////
////・同期をしながら、非マスター側のGameManagerの
////　インベントリのアイテムを使用する関数


//public class GameManager_S : MonoBehaviourPunCallbacks
//{
//    [SerializeField] private Timer m_GameTimer;
//    [SerializeField] private SceneChanger m_SceneChanger;


//    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//    //チームオブジェクトを二つ保有している（ヒエラルキーから登録する）
//    [Header("チームAのインスタンス　※要設定※")]
//    [SerializeField] private TeamObject m_TeamObjectA;
//    [Header("チームBのインスタンス　※要設定※")]
//    [SerializeField] private TeamObject m_TeamObjectB;
//    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<





//    private bool m_IsTimeUp = false;

//    private void Start()
//    {
//        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//        RegisterGameManagerToTeams();
//        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

//        AssignPlayersToTeams();
//        m_GameTimer.TimerStart();
//    }
//    private void Update()
//    {
//        GameTimeCount();
//    }
//    private void GameTimeCount()
//    {
//        if (m_IsTimeUp) return;

//        if (m_GameTimer.TimerEnd())
//        {
//            m_IsTimeUp = true;  // タイマー終了したらタイムアップ
//            GameEnd();
//        }
//    }

//    private void GameEnd()
//    {
//        if (!m_IsTimeUp) return;

//        m_SceneChanger.IsActive();  // シーン遷移起動
//    }


//    public TeamName AssignPlayersToTeams()
//    {
//        TeamName myTeamName = TeamName.None;
//        foreach (var player in PhotonNetwork.PlayerList)
//        {
//            if (player.CustomProperties.TryGetValue("Team", out object teamValue))
//            {
//                TeamName team = (TeamName)(int)teamValue; // intをTeamName列挙型に変換
//                Debug.Log($"プレイヤー {player.NickName} は {team} に所属");

//                // 例: チームごとにスポーン位置を分ける
//                if (player.IsLocal) // 自分自身の場合のみ適用
//                {
//                    myTeamName = (TeamName)(int)teamValue;  // 自分のチーム設定

//                    if (team == TeamName.TeamA)
//                        Debug.Log($"プレイヤー {player.NickName} のチーム：TeamA");
//                    else if (team == TeamName.TeamB)
//                        Debug.Log($"プレイヤー {player.NickName} のチーム：TeamB");
//                }
//            }
//            else
//            {
//                Debug.Log($"プレイヤー {player.NickName} のチーム情報が見つかりません");
//            }
//        }
//        return myTeamName;
//    }



//    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

//    /// <summary>
//    /// 保有しているチームに自身の参照を登録
//    /// </summary>
//    private void RegisterGameManagerToTeams()
//    {
//        if (m_TeamObjectA)
//            m_TeamObjectA.SetGameManager(this);
//        if (m_TeamObjectB)
//            m_TeamObjectB.SetGameManager(this);
//    }

//    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

//    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//    /// <summary>
//    /// 自分以外の参加者のゲームマネージャーへ、アイテムを使用したことを教える関数　
//    /// </summary>
//    /// <param name="teamObject">使用したチーム</param>
//    /// <param name="_itemIndex">使用したアイテムのインベントリ番号</param>
//    public void SendItemUsingToOtherTeam(TeamObject teamObject, int _itemIndex)
//    {
//        //もしも使用しているチームがAならば
//        if (teamObject == m_TeamObjectA)
//        {
//            //1を引数に、もう一つのモニター側のゲームマネージャーから実行する
//            photonView.RPC("UseItemRPC", RpcTarget.Others, 1, _itemIndex);
//        }
//        else
//        {
//            photonView.RPC("UseItemRPC", RpcTarget.Others, 2, _itemIndex);
//        }
//    }
//    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



//    /// <summary>
//    /// 相手参加者から呼び出される。相手が使用したアイテムをこちら世界でも使用する
//    /// </summary>
//    /// <param name="teamObject">使用したチーム</param>
//    /// <param name="_itemIndex">使用したアイテムのインベントリ番号</param>
//    [PunRPC]
//    private void UseItemRPC(int _teamNum, int _itemIndex)
//    {
//        if (_teamNum == 1)
//            //チームAで実行
//            if (m_TeamObjectA)
//                m_TeamObjectA.UseItemByGameManager(_itemIndex);

//            else
//            //チームBで実行
//            if (m_TeamObjectB)
//                m_TeamObjectB.UseItemByGameManager(_itemIndex);
//    }

//}
