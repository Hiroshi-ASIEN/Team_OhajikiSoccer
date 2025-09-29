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
            TeamName team = (TeamName)(int)teamValue; // int ���� �񋓌^�ɕϊ�
            Debug.Log($"�v���C���[ {PhotonNetwork.NickName} �� {team} �ɔz�u");

            if (team == TeamName.TeamA)
            {
                transform.position = new Vector3(-5, 0, 0); // TeamA �̃X�|�[���ʒu
            }
            else if (team == TeamName.TeamB)
            {
                transform.position = new Vector3(5, 0, 0); // TeamB �̃X�|�[���ʒu
            }
        }
        else
        {
            Debug.LogWarning("�`�[�������ݒ�̂��߁A�f�t�H���g�� TeamA �ɔz�u");
            transform.position = new Vector3(-5, 0, 0);
        }
    }


}