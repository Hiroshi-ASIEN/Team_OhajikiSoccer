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
            m_IsTimeUp = true;  // �^�C�}�[�I��������^�C���A�b�v
            GameEnd();
        }
    }

    private void GameEnd()
    {
        if (!m_IsTimeUp) return;

        m_SceneChanger.IsActive();  // �V�[���J�ڋN��
    }


    public TeamName AssignPlayersToTeams()
    {
        TeamName myTeamName=TeamName.None;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.TryGetValue("Team", out object teamValue))
            {
                TeamName team = (TeamName)(int)teamValue; // int��TeamName�񋓌^�ɕϊ�
                Debug.Log($"�v���C���[ {player.NickName} �� {team} �ɏ���");

                // ��: �`�[�����ƂɃX�|�[���ʒu�𕪂���
                if (player.IsLocal) // �������g�̏ꍇ�̂ݓK�p
                {
                    myTeamName = (TeamName)(int)teamValue;  // �����̃`�[���ݒ�

                    if (team == TeamName.TeamA)
                        Debug.Log($"�v���C���[ {player.NickName} �̃`�[���FTeamA");
                    else if (team == TeamName.TeamB)
                        Debug.Log($"�v���C���[ {player.NickName} �̃`�[���FTeamB");
                }
            }
            else
            {
                Debug.Log($"�v���C���[ {player.NickName} �̃`�[����񂪌�����܂���");
            }
        }
        return myTeamName;
    }
}
