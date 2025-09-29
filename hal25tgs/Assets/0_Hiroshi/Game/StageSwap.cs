using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TurnManager;

public class StageSwap : MonoBehaviourPun
{
    public enum STADIUM_TYPE
    {
        NORMAL = 0, // ��{
        LIGHTNING,  // ��
        DOUBLEGOAL, // �S�[���Q�{
        HOLE,       // ��
        FROST,      // �X
        WARP,       // ���[�v

        STADIUM_TYPE_MAX,
    }

    [Serializable]
    public class Stadiums
    {
        [SerializeField] public STADIUM_TYPE m_StadiumType; // ���̃X�^�W�A���^�C�v
        [SerializeField] public string m_PrefabName; // ���̃X�^�W�A���̃v���n�u��
    };

    [Header("�S�X�^�W�A���ݒ�")]
    [SerializeField] private Stadiums[] m_Stadiums;

    [Header("�X�^�W�A���o���ʒu")]
    [SerializeField] private Vector3 m_Position = new Vector3(0.0f, -5.0f, 0.0f);

    [Header("���݂̃X�^�W�A��")]
    [SerializeField] private GameObject m_CurrentStadium;

    private string m_CreateObjectName;    // �X�^�W�A���؂�ւ��v���n�u�p�ϐ�

    [SerializeField] bool m_IsSwap = false;         // �؂�ւ��t���O

    private STADIUM_TYPE m_NextType=STADIUM_TYPE.LIGHTNING;                // ���ɐ؂�ւ���X�^�W�A���^�C�v
    private STADIUM_TYPE m_CurrentType;
    private TurnManager m_TurnManager;  // �^�[���}�l�[�W���[�擾�p

    // �X�^�W�A���ؑ֎��ɔ�������C�x���g�B�o�^�����֐��͈�����STADIUM_TYPE���󂯎���
    public event Action<STADIUM_TYPE> OnStadiumChanged;  

    [SerializeField] private bool m_TestFrag = false;

    private Dictionary<Keeper, Vector3> m_OldKeeperPositions = new Dictionary<Keeper, Vector3>(); // �L�[�p�[�����̌��̈ʒu���L������ϐ�

    bool m_IsStageSwap = false; // �L�[�p�[�ړ��t���O

    [SerializeField] private FadeInOut m_Fade;
    private bool m_FadeFrag = false;

    // Start is called before the first frame update
    void Start()
    {
        // ���݂̃X�^�W�A��(�ŏ��ɐݒ肳��Ă���X�^�W�A��)�𐶐����Ă���
        //        Instantiate(m_CurrentStadium, gameObject.transform.position, Quaternion.identity);
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient) return;  // �}�X�^�[�N���C�A���g�łȂ���΂Ȃɂ����Ȃ�
        }

        // �^�[���}�l�[�W���[���擾���āA�^�[�����ɔ�������C�x���g�ɃX�e�[�W�ؑ֊֐���o�^���Ă���
        m_TurnManager = TurnManager.Instance;
        m_TurnManager.OnTurnChanged += Swap;
        m_TurnManager.OnTurnChanged += FadeStageSwapInterval;

        // �X�R�A�}�l�[�W���[�ŁA�_�����������ɃX�e�[�W�؂�ւ��i�����_���j�t���O��true�ɂ���
        ScoreManager.Instance.OnScoreChanged += m_TurnManager.StageSwap;
        ScoreManager.Instance.OnScoreChanged += RandomSetNextStadium;

        // �_�u���S�[���ɕς�������ɁA�L�[�p�[�̈ʒu���X�^�W�A���O�Ɉړ�����
        OnStadiumChanged += InvokeKeeperFlyaway;        

    }

    private void OnDisable()
    {
        if (!PhotonNetwork.IsMasterClient) return;  // �}�X�^�[�N���C�A���g�łȂ���΂Ȃɂ����Ȃ�
        m_TurnManager.OnTurnChanged -= Swap;
        m_TurnManager.OnTurnChanged -= FadeStageSwapInterval;
        ScoreManager.Instance.OnScoreChanged -= m_TurnManager.StageSwap;

        OnStadiumChanged -= InvokeKeeperFlyaway;
    }
    private void Update()
    {
        //�����ŁAm_CurrentStage��null�̏ꍇ�A
        //Tag�Ō��݂̃X�e�[�W���������Ċi�[����v���O�������~��������
        if (m_CurrentStadium ==null)
        {
            m_CurrentStadium = GameObject.FindGameObjectWithTag("Stadium");
        }



        if (m_TestFrag)
        {
            m_TurnManager.StageSwap();
//        RandomSetNextStadium();
  //          Swap(TURN_STATE.STADIUMSWAP_INTERVAL);
            m_TestFrag = false;
        }




    }

    // �ؑւ��邩�`�F�b�N�֐�
    private bool SwapCheck(TURN_STATE _state)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient) return false;  // �}�X�^�[�N���C�A���g�łȂ���΂Ȃɂ����Ȃ�
        }


        if (!m_IsSwap) return false;   // �ؑփt���O��true�łȂ���ΐؑւȂ�
        if (_state != TURN_STATE.STADIUMSWAP_INTERVAL) return false;  // �X�e�[�W�ؑ�
        Debug.Log(112);
        RandomSetNextStadium();

        // ���̃^�C�v���Ԉ���Ă��邩�A���݂̃X�^�W�A���^�C�v�Ɠ����Ȃ牽�����Ȃ� ������x���I
        if (m_NextType >= STADIUM_TYPE.STADIUM_TYPE_MAX || m_NextType == m_CurrentType)// return false;
        {
            RandomSetNextStadium();
        }

        Debug.Log("�X�e�[�W�ؑ։\�ł��B");
        return true;
    }

    private void Swap(TURN_STATE _state)
    {
        Debug.Log("return�O");
        if (!SwapCheck(_state)) return; //�`�F�b�N����false�Ȃ�ؑւȂ�
        Debug.Log("return��");

        for (int i = 0; i < m_Stadiums.Length; i++)
        {
            if (m_NextType == m_Stadiums[i].m_StadiumType)
            {
                m_CreateObjectName = m_Stadiums[i].m_PrefabName; // �����p�ϐ��Ɏ��̃X�^�W�A���̃v���n�u�o�^
                break;
            }

            if (i == m_Stadiums.Length) return;   // ������Ȃ������ꍇ�͐ؑւȂ�
        }

        // ���݂̃X�^�W�A�����폜����p�ϐ��Ɉڂ�
        GameObject destroyObject = m_CurrentStadium;
        m_CurrentStadium = null;

        // ���̃X�^�W�A���𐶐����Č��݂̃X�^�W�A���ɓo�^
//        m_CurrentStadium = PhotonNetwork.Instantiate(m_CreateObjectName, m_Position, Quaternion.identity);
        m_CurrentStadium = SingleMultiUtility.Instantiate(m_CreateObjectName, m_Position, Quaternion.identity);


        // ��قǂ܂Ŏg���Ă����X�^�W�A�����폜
        //        PhotonNetwork.Destroy(destroyObject);
        destroyObject.tag = "Untagged";
        SingleMultiUtility.Destroy(destroyObject);

        // ���݂̃X�^�W�A���^�C�v���X�V,�X�^�W�A���ؑփC�x���g����
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("StadiumSync", RpcTarget.All, m_NextType);
        }
        else
        {
            StadiumSync(m_NextType);
        }

        Debug.Log(m_CreateObjectName.ToString() + "�𐶐����܂����B");
    }

    // �X�^�W�A���ؑ֐ݒ�֐� ========================
    // ������ǂ����ŌĂ�
    public void SetNextStadium(STADIUM_TYPE _type)
    { 
        m_NextType = _type;
        m_IsSwap = true;

        Debug.Log(m_NextType.ToString() + "�𐶐����悤�Ƃ��Ă��܂��B");
    }

    // �X�^�W�A���ؑփ����_���ݒ�֐� ========================
    // ������ǂ����ŌĂ�
    public void RandomSetNextStadium()
    {
        int type = UnityEngine.Random.Range(0, (int)STADIUM_TYPE.STADIUM_TYPE_MAX);

        if (m_Stadiums.Length < (int)STADIUM_TYPE.STADIUM_TYPE_MAX - 1)
        {
            type = UnityEngine.Random.Range(0, m_Stadiums.Length);
        }
        m_NextType = (STADIUM_TYPE)type;
        m_IsSwap = true;

        Debug.Log(m_NextType.ToString() + "�𐶐����悤�Ƃ��Ă��܂��B");
    }

    // ���݂̃X�^�W�A���擾 ========================
    public STADIUM_TYPE GetCurrentStadium()
    {
        return m_CurrentType;
    }

    // ���݂̃X�^�W�A���^�C�v�����p
    [PunRPC]
    private void StadiumSync(STADIUM_TYPE _type)
    {
        m_CurrentType = _type;
        m_NextType = STADIUM_TYPE.STADIUM_TYPE_MAX;
        OnStadiumChanged?.Invoke(m_CurrentType);    // �X�^�W�A���ؑփC�x���g����
    }

    private void InvokeKeeperFlyaway(StageSwap.STADIUM_TYPE _type)
    {
        if (PhotonNetwork.IsConnected)
        {
            KeeperFlyAway(_type);
            photonView.RPC("KeeperFlyAway", RpcTarget.All, _type);
        }
        else
        {
            KeeperFlyAway(_type);
        }
    }

    // �X�^�W�A�����_�u���S�[�����ɃL�[�p�[�̈ʒu��ύX����֐�
    [PunRPC]
    private void KeeperFlyAway(StageSwap.STADIUM_TYPE _type)
    {
        m_IsStageSwap = true;

        // ���`�[���̃L�[�p�[��position���擾
        Keeper[] keepers = FindObjectsByType<Keeper>(FindObjectsSortMode.None); 

        foreach (Keeper keeper in keepers)
        {
            if (_type == StageSwap.STADIUM_TYPE.DOUBLEGOAL)
            {
                if (!m_OldKeeperPositions.ContainsKey(keeper))
                {
                    // �L�[�p�[�̌���position��ۑ�
                    m_OldKeeperPositions[keeper] = keeper.transform.position;
                }

                // �L�[�p�[��position.z��-100�����Z
                Vector3 newPosition = keeper.transform.position + new Vector3(0f, 0f, -100f);
                keeper.transform.position = newPosition;
                //Debug.Log($"Keeper moved to {newPosition}");
            }
            else
            {
                // �_�u���S�[���ȊO�Ȃ猳�ɖ߂�
                if (m_OldKeeperPositions.ContainsKey(keeper))
                {
                    Vector3 oldPos = m_OldKeeperPositions[keeper];
                    keeper.transform.position = oldPos;
                    Debug.Log($"Keeper returned to {oldPos}");
                }
            }
        }

        m_IsStageSwap = false;
    }


    private void FadeStageSwapInterval(TURN_STATE _state)
    {
        if (_state == TURN_STATE.STADIUMSWAP_INTERVAL)
        {
            m_FadeFrag = true;
            m_Fade.SetFadeMode(FadeInOut.FadeMode.Out);
            m_Fade.FadeStart();
        }
        else if (_state == TURN_STATE.PLAY_TURN && m_FadeFrag == true)
        {
            m_FadeFrag = false;
            m_Fade.SetFadeMode(FadeInOut.FadeMode.In);
            m_Fade.FadeStart();
        }

    }
}
