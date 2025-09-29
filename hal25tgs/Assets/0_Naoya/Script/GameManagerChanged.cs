//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


////�ύX�����@2025/05/09  �͏㏮��
////
////�E�p������MonoBehaviourPunCallbacks�ɕύX
////
////
////�E�����o�ϐ��ɓ�̃`�[���I�u�W�F�N�g��ǉ�
////�@��̃`�[���I�u�W�F�N�g�ɁATeamObject����SetGameManager��p����
////�@���g��o�^����
////
////
////�E���������Ȃ���A��}�X�^�[����GameManager��
////�@�C���x���g���̃A�C�e�����g�p����֐�


//public class GameManager_S : MonoBehaviourPunCallbacks
//{
//    [SerializeField] private Timer m_GameTimer;
//    [SerializeField] private SceneChanger m_SceneChanger;


//    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//    //�`�[���I�u�W�F�N�g���ۗL���Ă���i�q�G�����L�[����o�^����j
//    [Header("�`�[��A�̃C���X�^���X�@���v�ݒ聦")]
//    [SerializeField] private TeamObject m_TeamObjectA;
//    [Header("�`�[��B�̃C���X�^���X�@���v�ݒ聦")]
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
//            m_IsTimeUp = true;  // �^�C�}�[�I��������^�C���A�b�v
//            GameEnd();
//        }
//    }

//    private void GameEnd()
//    {
//        if (!m_IsTimeUp) return;

//        m_SceneChanger.IsActive();  // �V�[���J�ڋN��
//    }


//    public TeamName AssignPlayersToTeams()
//    {
//        TeamName myTeamName = TeamName.None;
//        foreach (var player in PhotonNetwork.PlayerList)
//        {
//            if (player.CustomProperties.TryGetValue("Team", out object teamValue))
//            {
//                TeamName team = (TeamName)(int)teamValue; // int��TeamName�񋓌^�ɕϊ�
//                Debug.Log($"�v���C���[ {player.NickName} �� {team} �ɏ���");

//                // ��: �`�[�����ƂɃX�|�[���ʒu�𕪂���
//                if (player.IsLocal) // �������g�̏ꍇ�̂ݓK�p
//                {
//                    myTeamName = (TeamName)(int)teamValue;  // �����̃`�[���ݒ�

//                    if (team == TeamName.TeamA)
//                        Debug.Log($"�v���C���[ {player.NickName} �̃`�[���FTeamA");
//                    else if (team == TeamName.TeamB)
//                        Debug.Log($"�v���C���[ {player.NickName} �̃`�[���FTeamB");
//                }
//            }
//            else
//            {
//                Debug.Log($"�v���C���[ {player.NickName} �̃`�[����񂪌�����܂���");
//            }
//        }
//        return myTeamName;
//    }



//    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

//    /// <summary>
//    /// �ۗL���Ă���`�[���Ɏ��g�̎Q�Ƃ�o�^
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
//    /// �����ȊO�̎Q���҂̃Q�[���}�l�[�W���[�ցA�A�C�e�����g�p�������Ƃ�������֐��@
//    /// </summary>
//    /// <param name="teamObject">�g�p�����`�[��</param>
//    /// <param name="_itemIndex">�g�p�����A�C�e���̃C���x���g���ԍ�</param>
//    public void SendItemUsingToOtherTeam(TeamObject teamObject, int _itemIndex)
//    {
//        //�������g�p���Ă���`�[����A�Ȃ��
//        if (teamObject == m_TeamObjectA)
//        {
//            //1�������ɁA������̃��j�^�[���̃Q�[���}�l�[�W���[������s����
//            photonView.RPC("UseItemRPC", RpcTarget.Others, 1, _itemIndex);
//        }
//        else
//        {
//            photonView.RPC("UseItemRPC", RpcTarget.Others, 2, _itemIndex);
//        }
//    }
//    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



//    /// <summary>
//    /// ����Q���҂���Ăяo�����B���肪�g�p�����A�C�e���������琢�E�ł��g�p����
//    /// </summary>
//    /// <param name="teamObject">�g�p�����`�[��</param>
//    /// <param name="_itemIndex">�g�p�����A�C�e���̃C���x���g���ԍ�</param>
//    [PunRPC]
//    private void UseItemRPC(int _teamNum, int _itemIndex)
//    {
//        if (_teamNum == 1)
//            //�`�[��A�Ŏ��s
//            if (m_TeamObjectA)
//                m_TeamObjectA.UseItemByGameManager(_itemIndex);

//            else
//            //�`�[��B�Ŏ��s
//            if (m_TeamObjectB)
//                m_TeamObjectB.UseItemByGameManager(_itemIndex);
//    }

//}
