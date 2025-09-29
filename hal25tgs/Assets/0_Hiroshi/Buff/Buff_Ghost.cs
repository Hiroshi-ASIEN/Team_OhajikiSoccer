using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Ghost : Buff_Base
{
    [SerializeField] private GameManager_S m_GameManager;
    public override void BuffActivate()
    {
        Debug.Log("�v���C�^�[�o�t�F�ђ�");
        // ��̓I�Ȍ��ʂ����̉��ɋL�q
        GhostSwitch(true);
    }

    public override void BuffDeactivate()
    {
        GhostSwitch(false);
    }

    // �v���C���[�̓����蔻��ђʉ��X�C�b�`
    private void GhostSwitch(bool _switch)
    {
        for (int t = (int)TeamName.TeamA; t <= (int)TeamName.TeamB; t++)
        {
            TeamObject team = m_GameManager.GetTeamObject((TeamName)t);

            if (team == null)
            {
                Debug.LogError("m_Team �� null");
                return;
            }

            var players = team.GetPlayersArray();

            if (players == null)
            {
                Debug.Log(((TeamName)t).ToString());

                Debug.LogError("GetPlayersArray() �� null");
                return;
            }
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    Debug.LogError($"players[{i}] �� null");
                    continue;
                }

                // �v���C���[�̓����蔻��������̒l�ɂ���
                if (_switch)
                {
                    players[i].gameObject.layer = (int)LayerNum.Ghost;
                }
                else if (!_switch)
                {
                    players[i].gameObject.layer = (int)LayerNum.Player;
                }
            }
        }
    }

}
