using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using UnityEngine.UIElements;


//�ύX�����@2025/05/09  �͏㏮��
//
//�E�p������MonoBehaviourPunCallbacks�ɕύX
//
//
//�E�����o�ϐ��ɓ�̃`�[���I�u�W�F�N�g��ǉ�
//�@��̃`�[���I�u�W�F�N�g�ɁATeamObject����SetGameManager��p����
//�@���g��o�^����
//
//
//�E���������Ȃ���A��}�X�^�[����GameManager��
//�@�C���x���g���̃A�C�e�����g�p����֐�


public class GameManager_S : MonoBehaviourPunCallbacks
{
    [SerializeField] private Timer m_GameTimer;
    [SerializeField] private SceneChanger m_SceneChanger;
    [SerializeField] private GameObject m_Ball;
    [SerializeField] private TurnManager m_TurnManagerPrefab;
    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    //�`�[���I�u�W�F�N�g���ۗL���Ă���i�q�G�����L�[����o�^����j���������ɓo�^���悤��
    [Header("�`�[��A�̃C���X�^���X�@���v�ݒ聦")]
    [SerializeField] private TeamObject m_TeamObjectA;
    [Header("�`�[��B�̃C���X�^���X�@���v�ݒ聦")]
    [SerializeField] private TeamObject m_TeamObjectB;
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<


    [Header("UI")]
    [SerializeField] Image m_TeamAUI;
    [SerializeField] Image m_TeamBUI;
    [SerializeField] List<Sprite> m_Sprite;

    GameObject m_MyTeamObject;
    GameObject m_EnemyTeamObject;
    Vector3 m_TeamObjectPos = Vector3.zero;
    GameObject[] m_TeamObjects = null;

    ScoreManager m_ScoreManager;

    private bool m_IsTimeUp = false;

    private void Start()
    {
        m_TeamObjects = new GameObject[2];

        // �`�[���}�l�[�W���[����
//       m_MyTeamObject = SingleMultiUtility.Instantiate("TeamManager", m_TeamObjectPos, Quaternion.identity);
        if (PhotonNetwork.IsConnected)  // �l�b�g���[�N�ɐڑ����Ă��鎞�̓t�H�g���Ő���
        {
            //���ꂾ��Player���Q������������TeamManager�������
//            m_MyTeamObject = PhotonNetwork.Instantiate("TeamManager", m_TeamObjectPos, Quaternion.identity);
              m_MyTeamObject = SingleMultiUtility.InstantiateForClient("TeamManager", m_TeamObjectPos, Quaternion.identity);
        }
        else    // �ڑ����Ă��Ȃ����͎������������iAI�͕ʂŁj
        {
            GameObject prefab = Resources.Load<GameObject>("TeamManager");
            m_MyTeamObject = GameObject.Instantiate(prefab, m_TeamObjectPos, Quaternion.identity);
            // �V���O���v���C����Resources����ǂݍ���
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        RegisterGameManagerToTeams();
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        //Test : GameManager�Ɏ�����Team��ݒ�
        //TODO : �����Team���o�^

        TeamName teamName = AssignPlayersToTeams();
        if (teamName == TeamName.TeamA)
        {
            m_TeamObjectA = m_MyTeamObject.GetComponent<TeamObject>();
        }
        else
        {
            m_TeamObjectB = m_MyTeamObject.GetComponent<TeamObject>();
        }
        m_ScoreManager = ScoreManager.Instance;
        m_ScoreManager.ResetScores();

        if (!TurnManager.Instance)
        {
            Debug.Log("�Ȃ���1");
            SingleMultiUtility.InstantiateForClient(m_TurnManagerPrefab.name, this.transform.position, Quaternion.identity);

            // �^�[���}�l�[�W���[�ݒ�E�N��
            TurnManager.Instance.TurnStart();
            TurnManager.Instance.OnGameEnd += GameEnd;
            m_GameTimer.TimerStart();

            TurnManager.Instance.OnTurnChanged += ScoreCheck;
            return;
        }
        // �^�[���}�l�[�W���[�ݒ�E�N��
        TurnManager.Instance.TurnStart();
        TurnManager.Instance.OnGameEnd += GameEnd;
        TurnManager.Instance.OnTurnChanged += ScoreCheck;

        m_GameTimer.TimerStart();

        Debug.Log("�����");
    }

    private void FixedUpdate()
    {
        if (!TurnManager.Instance)
        {
            SingleMultiUtility.InstantiateForClient(m_TurnManagerPrefab.name, this.transform.position, Quaternion.identity);

            // �^�[���}�l�[�W���[�ݒ�E�N��
            TurnManager.Instance.TurnStart();
            TurnManager.Instance.OnGameEnd += GameEnd;
            m_GameTimer.TimerStart();

            m_ScoreManager = ScoreManager.Instance;
            m_ScoreManager.ResetScores();
            TurnManager.Instance.OnTurnChanged += ScoreCheck;
            Debug.Log("�Ȃ�������");
        }

    }

    private void OnDisable()
    {
        TurnManager.Instance.OnGameEnd -= GameEnd;
        TurnManager.Instance.OnTurnChanged -= ScoreCheck;
    }

    private void Update()
    {
        //��肭�쓮���ĂȂ�
        //TeamA���G�̏ꍇ
        if (!m_TeamObjectA)
        {
            m_TeamObjects = GameObject.FindGameObjectsWithTag("TeamObject");
            Debug.Log(m_TeamObjects);
            for (int i = 0; i < m_TeamObjects.Length; i++)
            {
                //���肪��������TeamManager���擾
                if (!m_TeamObjects[i].GetComponent<PhotonView>().IsMine)
                {
                    m_TeamObjectA = m_TeamObjects[i].GetComponent<TeamObject>();
                }
            }
        }
        //TeamB���G�̏ꍇ
        else if (!m_TeamObjectB)
        {
            m_TeamObjects = GameObject.FindGameObjectsWithTag("TeamObject");
            Debug.Log(m_TeamObjects);
            for (int i = 0; i < m_TeamObjects.Length; i++)
            {
                //���肪��������TeamManager���擾
                if (!m_TeamObjects[i].GetComponent<PhotonView>().IsMine)
                {
                    m_TeamObjectB = m_TeamObjects[i].GetComponent<TeamObject>();
                }
            }
        }

        SetTeamUI();

//        GameTimeCount();
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

    private void ScoreCheck(TurnManager.TURN_STATE _state)
    {
        if (_state != TurnManager.TURN_STATE.PLAY_TURN) return;

        if (m_ScoreManager.GetMaxScore() <= m_ScoreManager.GetScore(TeamName.TeamA)
            || m_ScoreManager.GetMaxScore() <= m_ScoreManager.GetScore(TeamName.TeamB))
        {
            GameEnd();
        }
    }

    private void GameEnd()
    {
        //        if (!m_IsTimeUp) return;
        TurnManager.Instance.OnGameEnd -= GameEnd;
        TurnManager.Instance.OnTurnChanged -= ScoreCheck;
        m_SceneChanger.IsActive();  // �V�[���J�ڋN��
    }


    public TeamName AssignPlayersToTeams()
    {
        TeamName myTeamName = TeamName.None;

        if(!PhotonNetwork.IsConnected)
        {
            Debug.Log("�����ł�" + ScoreManager.Instance.m_SoloTeamName);
            return ScoreManager.Instance.m_SoloTeamName;
        }

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



    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    /// <summary>
    /// �ۗL���Ă���`�[���Ɏ��g�̎Q�Ƃ�o�^
    /// </summary>
    private void RegisterGameManagerToTeams()
    {
        if (m_TeamObjectA)
            m_TeamObjectA.SetGameManager(this);
        if (m_TeamObjectB)
            m_TeamObjectB.SetGameManager(this);
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    /// <summary>
    /// �����ȊO�̎Q���҂̃Q�[���}�l�[�W���[�ցA�A�C�e�����g�p�������Ƃ�������֐��@
    /// </summary>
    /// <param name="teamObject">�g�p�����`�[��</param>
    /// <param name="_itemIndex">�g�p�����A�C�e���̃C���x���g���ԍ�</param>
    public void SendItemUsingToOtherTeam(TeamObject teamObject, int _itemIndex)
    {
        Debug.Log("SendItemUsingToOtherTeam�֐����Ă΂�܂���");

        //�������g�p���Ă���`�[����A�Ȃ��
        if (teamObject == m_TeamObjectA)
        {
            Debug.Log(photonView);

            //1�������ɁA������̃��j�^�[���̃Q�[���}�l�[�W���[������s����
            photonView.RPC("UseItemRPC", RpcTarget.Others, 1, _itemIndex);
        }
        else
        {
            photonView.RPC("UseItemRPC", RpcTarget.Others, 2, _itemIndex);
        }
    }
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<



    /// <summary>
    /// ����Q���҂���Ăяo�����B���肪�g�p�����A�C�e���������琢�E�ł��g�p����
    /// </summary>
    /// <param name="teamObject">�g�p�����`�[��</param>
    /// <param name="_itemIndex">�g�p�����A�C�e���̃C���x���g���ԍ�</param>
    [PunRPC]
    private void UseItemRPC(int _teamNum, int _itemIndex)
    {
        Debug.Log("UseItemRPC�֐����Ă΂�܂���");


        if (_teamNum == 1)
            //�`�[��A�Ŏ��s
            if (m_TeamObjectA)
                m_TeamObjectA.UseItemByGameManager(_itemIndex);

            else
            //�`�[��B�Ŏ��s
            if (m_TeamObjectB)
                m_TeamObjectB.UseItemByGameManager(_itemIndex);
    }


    [PunRPC]
    private void TestItemRPC()
    {

    }


    //�������`�[��A���ǂ����i�������݂̎��ʕ��@�̓X�}�[�g�ł͂Ȃ��B���Ӂj
    public bool IsTeamA(TeamObject teamObject)
    {
        if(m_TeamObjectA)
        {
            if (m_TeamObjectA == teamObject)
                return true;
        }
        if(m_TeamObjectB)
        {
            if (m_TeamObjectB != teamObject)
                return true;
        }
        return false;
    }

    public TeamObject GetTeamObject(TeamName _teamName)
    {
        if (_teamName == TeamName.TeamA)
        {
            return m_TeamObjectA;
        }
        else if (_teamName == TeamName.TeamB)
        {
            return m_TeamObjectB;
        }

        Debug.Log("�`�[����񂪂���܂���B");
        return null;
    }

    public GameObject GetBallObject()
    {
        return m_Ball;
    }
    public void SetTeamUI()
    {
        //UI�ݒ�
        //�\���p
        if (!PhotonNetwork.IsConnected)
        {
            if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
            {
                Character character = m_TeamObjectA.GetCharacter();

                int num = (int)character;
                m_TeamAUI.sprite = m_Sprite[num];

            }
            else
            {
                Character character = m_TeamObjectB.GetCharacter();

                int num = (int)character;
                m_TeamBUI.sprite = m_Sprite[num];

            }
        }
        else
        {
            if(PhotonNetwork.IsMasterClient)
            {//�������}�X�^�[��TeamA��I��ł��鎞
                if(ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
                {
                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    ScoreManager.Instance.m_CharacterA = teamACharacter;
                    int num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];

                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    ScoreManager.Instance.m_CharacterB = teamBCharacter;
                    num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];
                }
                else if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamB)
                {
                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    ScoreManager.Instance.m_CharacterB = teamBCharacter;
                    int num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];

                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    ScoreManager.Instance.m_CharacterA = teamACharacter;
                    num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];

                }

            }
            else
            {
                //�}�X�^�[�ł͂Ȃ��ꍇ
                if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamB)
                {
                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    int num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];

                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];
                }
                else if(ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
                {
                    Character teamACharacter = m_TeamObjectA.GetCharacter();
                    int num = (int)teamACharacter;
                    m_TeamAUI.sprite = m_Sprite[num];

                    Character teamBCharacter = m_TeamObjectB.GetCharacter();
                    num = (int)teamBCharacter;
                    m_TeamBUI.sprite = m_Sprite[num];
                }


            }



        }
    }


}
