using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;
using static Photon.Pun.UtilityScripts.PunTeams;

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
    [SerializeField] private UnityEngine.UI.Button m_TeamA_Button;    // �`�[���I���{�^��
    [SerializeField] private UnityEngine.UI.Button m_TeamB_Button;    // �`�[���I���{�^��
    [SerializeField] private GameObject m_Character1_Button;
    [SerializeField] private GameObject m_Character2_Button;
    [SerializeField] private GameObject m_Character3_Button;
    [SerializeField] private GameObject m_Character4_Button;
    [SerializeField] private GameObject m_Ready_Button;

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
        m_Character1_Button.SetActive(true);
        m_Character2_Button.SetActive(true);
        m_Character3_Button.SetActive(true);
        m_Character4_Button.SetActive(true);

    }

    public void SelectTeamB()
    {
        SetPlayerTeam(TeamName.TeamB);
        m_Character1_Button.SetActive(true);
        m_Character2_Button.SetActive(true);
        m_Character3_Button.SetActive(true);
        m_Character4_Button.SetActive(true);

    }

    public void SetPlayerTeam(TeamName _teamName)
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

        ScoreManager.Instance.SetSoloTeamNeme(_teamName);
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
        int memberCount = GetTeamMemberCount(TeamName.TeamA);
        m_TeamACountText.text = $"{memberCount}/{m_TeamMember_Max}";
        if (memberCount > 0)
        {
            m_TeamA_Button.interactable = false;
        }

        memberCount = GetTeamMemberCount(TeamName.TeamB);
        m_TeamBCountText.text = $"{memberCount}/{m_TeamMember_Max}";
        if (memberCount > 0)
        {
            m_TeamB_Button.interactable = false;
        }
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

    public void SelectCharacter1()
    {
        SelectCharacter(Character.Character1);
        m_Ready_Button.SetActive(true);
    }
    public void SelectCharacter2()
    {
        SelectCharacter(Character.Character2);
        m_Ready_Button.SetActive(true);
    }
    public void SelectCharacter3()
    {
        SelectCharacter(Character.Character3);
        m_Ready_Button.SetActive(true);
    }
    public void SelectCharacter4()
    {
        SelectCharacter(Character.Character4);
        m_Ready_Button.SetActive(true);
    }
    private void SelectCharacter(Character _character)
    {
        ScoreManager.Instance.SetMultiCharacter(_character);
    }
    
    public void ExitTeamClear()
    {
        PhotonView photonview = GetComponent<PhotonView>();

        photonview.RPC("SelectTeamClear", RpcTarget.All);
    }

    [PunRPC]
    public void SelectTeamClear()
    {
        Hashtable props = new Hashtable
        {
            [TEAM_KEY] = null,
            [READY_KEY] = null
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            player.CustomProperties.Remove(TEAM_KEY);
            player.CustomProperties.Remove(READY_KEY);

            Debug.Log("�ޏo");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"�v���C���[ {otherPlayer.NickName} �����[������ޏo���܂���");

        // �`�[���Ə��������폜�i�����ڏ�j
        if (otherPlayer.CustomProperties.ContainsKey(TEAM_KEY) || otherPlayer.CustomProperties.ContainsKey(READY_KEY))
        {
            otherPlayer.CustomProperties.Remove(TEAM_KEY);
            otherPlayer.CustomProperties.Remove(READY_KEY);

            Debug.Log($"{otherPlayer.NickName} �̃v���p�e�B���폜���܂���");
        }

        // �`�[���l���J�E���g�X�V
        UpdateTeamCounts();

        // ������ԃ`�F�b�N���Ď��s�i�K�v�ɉ����āj
        CheckAllPlayersReady();
    }

}
