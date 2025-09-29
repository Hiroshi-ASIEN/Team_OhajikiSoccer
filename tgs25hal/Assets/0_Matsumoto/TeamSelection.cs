using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class TeamSelection : MonoBehaviourPunCallbacks
{
    [Header("�`�[���l�����")]
    [SerializeField] private int m_TeamMember_Max;    // �`�[���l�����

    [SerializeField] private SceneChanger m_SceneChanger;

    private const string TEAM_KEY = "Team"; // �`�[�����̃L�[
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
                Debug.Log($"�v���C���[ {player.NickName} �̓`�[�� {teamValue}");

                if (player.CustomProperties.TryGetValue(READY_KEY, out object readyValue) && (bool)readyValue)
                {
                    readyPlayers++;
                    Debug.Log($"�v���C���[ {player.NickName} �͏�������");
                }
            }
            else
            {
                Debug.Log($"�v���C���[ {player.NickName} �̓`�[�����I��");
            }
        }

        Debug.Log($"���݂̎Q���Ґ�: {totalPlayers}, ��������: {readyPlayers}");


        if (GetTeamMemberCount(TeamName.TeamA) < 1 || GetTeamMemberCount(TeamName.TeamB) < 1)
        {
            Debug.Log("�ΐ푊�肪���܂���B");
            return;
        }

        if (totalPlayers > 0 && totalPlayers == readyPlayers)
        {
            Debug.Log("Game Start!");
            m_SceneChanger.IsActive();
        }
        else
        {
            Debug.Log("���̎Q���҂̃`�F�b�N�ҋ@��");
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
//            m_StatusText.text = $"YouTeam : None"; // UI�ɔ��f
            Debug.Log("���łɃ`�[����I���ς݁I");
            return;
        }

        int teamCount = GetTeamMemberCount(_teamName);   // �`�[���l���m�F
        if (teamCount >= m_TeamMember_Max)
        {
            m_StatusText.text = $"{_teamName} : MemberMax!"; // UI�ɔ��f
            return; // �l������Ȃ珈�����Ȃ�
        }

        Hashtable playerProperties = new Hashtable();
        playerProperties[TEAM_KEY] = (int)_teamName;    // �񋓌^��int�ɕϊ�
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);

        Debug.Log($"SetCustomProperties: {TEAM_KEY} = {(int)_teamName}");

        if (_teamName == TeamName.TeamA)
        {
//            m_StatusText.text = $"Team : Tokyo"; // UI�ɔ��f
            Debug.Log($"�`�[���I��: {_teamName}");
        }
        else if (_teamName == TeamName.TeamB)
        {
//            m_StatusText.text = $"Team : Urawa"; // UI�ɔ��f
            Debug.Log($"�`�[���I��: {_teamName}");
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
            Debug.Log($"{targetPlayer.NickName} �� {changedProps[TEAM_KEY]} �ɎQ��");
        }

        if (changedProps.ContainsKey(READY_KEY))
        {
            Debug.Log($"{targetPlayer.NickName} �� Ready ��Ԃ� {changedProps[READY_KEY]} �ɕύX");
            CheckAllPlayersReady();
        }
    }
}
