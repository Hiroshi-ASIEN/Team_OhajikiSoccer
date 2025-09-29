using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class TeamSelection : MonoBehaviourPunCallbacks
{
    [Header("チーム人数上限")]
    [SerializeField] private int m_TeamMember_Max;    // チーム人数上限

    [SerializeField] private SceneChanger m_SceneChanger;

    private const string TEAM_KEY = "Team"; // チーム情報のキー
    private const string READY_KEY = "Ready";

    [SerializeField] private TextMeshProUGUI m_StatusText;
    [SerializeField] private TextMeshProUGUI m_TeamACountText;
    [SerializeField] private TextMeshProUGUI m_TeamBCountText;
    [SerializeField] private TextMeshProUGUI m_ReadyText;

    private bool isReady = false;

    private void Start()
    {
        UpdateTeamCounts();

    }
    public void ToggleReady()
    {
        isReady = !isReady;
        m_ReadyText.text = isReady ? "Cancel" : "Ready";

        Hashtable playerProperties = new Hashtable();
        playerProperties[READY_KEY] = isReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        Debug.Log($"SetCustomProperties: {READY_KEY} = {isReady}");

        OnPlayerPropertiesUpdate(PhotonNetwork.LocalPlayer, playerProperties);
    }

    private void CheckAllPlayersReady()
    {
        int totalPlayers = 0;
        int readyPlayers = 0;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue(TEAM_KEY, out object teamValue))
            {
                totalPlayers++;
                Debug.Log($"プレイヤー {player.NickName} はチーム {teamValue}");

                if (player.CustomProperties.TryGetValue(READY_KEY, out object readyValue) && (bool)readyValue)
                {
                    readyPlayers++;
                    Debug.Log($"プレイヤー {player.NickName} は準備完了");
                }
            }
            else
            {
                Debug.Log($"プレイヤー {player.NickName} はチーム未選択");
            }
        }

        Debug.Log($"現在の参加者数: {totalPlayers}, 準備完了: {readyPlayers}");


        if (GetTeamMemberCount(TeamName.TeamA) < 1 || GetTeamMemberCount(TeamName.TeamB) < 1)
        {
            Debug.Log("対戦相手がいません。");
            return;
        }

        if (totalPlayers > 0 && totalPlayers == readyPlayers)
        {
            Debug.Log("Game Start!");
            m_SceneChanger.IsActive();
        }
        else
        {
            Debug.Log("他の参加者のチェック待機中");
        }
    }

    public void SelectTeamA()
    {
        SetPlayerTeam(TeamName.TeamA);
    }

    public void SelectTeamB()
    {
        SetPlayerTeam(TeamName.TeamB);
    }

    private void SetPlayerTeam(TeamName _teamName)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(TEAM_KEY))
        {
//            m_StatusText.text = $"YouTeam : None"; // UIに反映
            Debug.Log("すでにチームを選択済み！");
            return;
        }

        int teamCount = GetTeamMemberCount(_teamName);   // チーム人数確認
        if (teamCount >= m_TeamMember_Max)
        {
            m_StatusText.text = $"{_teamName} : MemberMax!"; // UIに反映
            return; // 人数上限なら処理しない
        }

        Hashtable playerProperties = new Hashtable();
        playerProperties[TEAM_KEY] = (int)_teamName;    // 列挙型をintに変換
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        Debug.Log($"SetCustomProperties: {TEAM_KEY} = {(int)_teamName}");

        if (_teamName == TeamName.TeamA)
        {
//            m_StatusText.text = $"Team : Tokyo"; // UIに反映
            Debug.Log($"チーム選択: {_teamName}");
        }
        else if (_teamName == TeamName.TeamB)
        {
//            m_StatusText.text = $"Team : Urawa"; // UIに反映
            Debug.Log($"チーム選択: {_teamName}");
        }
        OnPlayerPropertiesUpdate(PhotonNetwork.LocalPlayer, playerProperties);
    }

    public int GetTeamMemberCount(TeamName team)
    {
        int count = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue(TEAM_KEY, out object teamValue) &&
                (TeamName)(int)teamValue == team)
            {
                count++;
            }
        }
        return count;
    }

    [PunRPC]
    private void UpdateTeamCounts()
    {
        m_TeamACountText.text = $"{GetTeamMemberCount(TeamName.TeamA)}/{m_TeamMember_Max}";
        m_TeamBCountText.text = $"{GetTeamMemberCount(TeamName.TeamB)}/{m_TeamMember_Max}";
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log($"OnPlayerPropertiesUpdate called for {targetPlayer.NickName}");

        if (changedProps.ContainsKey(TEAM_KEY))
        {
//            photonView.RPC("UpdateTeamCounts", RpcTarget.All);
            UpdateTeamCounts();
            Debug.Log($"{targetPlayer.NickName} が {changedProps[TEAM_KEY]} に参加");
        }

        if (changedProps.ContainsKey(READY_KEY))
        {
            Debug.Log($"{targetPlayer.NickName} の Ready 状態が {changedProps[READY_KEY]} に変更");
            CheckAllPlayersReady();
        }
    }
}
