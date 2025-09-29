using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_S : MonoBehaviour
{
    [SerializeField] private Timer m_GameTimer;
    [SerializeField] private SceneChanger m_SceneChanger;

    private bool m_IsTimeUp = false;

    private void Start()
    {
        AssignPlayersToTeams();
        m_GameTimer.TimerStart();
    }
    private void Update()
    {
        GameTimeCount();
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

    private void GameEnd()
    {
        if (!m_IsTimeUp) return;

        m_SceneChanger.IsActive();  // シーン遷移起動
    }


    public TeamName AssignPlayersToTeams()
    {
        TeamName myTeamName=TeamName.None;
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
}
