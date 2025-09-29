using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviourPunCallbacks
{
    private const string TEAM_KEY = "Team";

    private void Start()
    {
        AssignPlayerToTeam();
    }

    private void AssignPlayerToTeam()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(TEAM_KEY, out object teamValue))
        {
            TeamName team = (TeamName)(int)teamValue; // int から 列挙型に変換
            Debug.Log($"プレイヤー {PhotonNetwork.NickName} は {team} に配置");

            if (team == TeamName.TeamA)
            {
                transform.position = new Vector3(-5, 0, 0); // TeamA のスポーン位置
            }
            else if (team == TeamName.TeamB)
            {
                transform.position = new Vector3(5, 0, 0); // TeamB のスポーン位置
            }
        }
        else
        {
            Debug.LogWarning("チームが未設定のため、デフォルトで TeamA に配置");
            transform.position = new Vector3(-5, 0, 0);
        }
    }


}