using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TurnManager;

public class Swap : MonoBehaviour
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

    public class Stadiums
    {
        [SerializeField] public STADIUM_TYPE m_StadiumType; // ���̃X�^�W�A���^�C�v
        [SerializeField] public string m_PrefabName; // ���̃X�^�W�A���̃v���n�u��
    };


    [Header("�S�X�^�W�A���ݒ�")]
    [SerializeField] private Stadiums[] m_Stadiums;

    [Header("�X�^�W�A���o���ʒu")]
    [SerializeField] private Vector3 m_Position = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("���݂̃X�^�W�A��")]
    [SerializeField] private GameObject m_CurrentStadium;

    private string m_CreateObjectName;    // �X�^�W�A���؂�ւ��v���n�u�p�ϐ�

    [SerializeField] bool m_IsSwap = false;         // �؂�ւ��t���O

    private STADIUM_TYPE m_NextType = STADIUM_TYPE.LIGHTNING;                // ���ɐ؂�ւ���X�^�W�A���^�C�v
    private STADIUM_TYPE m_CurrentType;
    private TurnManager m_TurnManager;  // �^�[���}�l�[�W���[�擾�p

    public event Action<STADIUM_TYPE> OnStadiumChanged;  // �X�^�W�A���ؑ֎��ɔ�������C�x���g

    [SerializeField] private Swap m_StageSwap;

    public GameObject m_tagetPlayer; // �L�[�p�[�̍��W���擾

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;  // �}�X�^�[�N���C�A���g�łȂ���΂Ȃɂ����Ȃ�

        // �^�[���}�l�[�W���[���擾���āA�^�[�����ɔ�������C�x���g�ɃX�e�[�W�ؑ֊֐���o�^���Ă���
        m_TurnManager = TurnManager.Instance;
        m_TurnManager.OnTurnChanged += hSwap;

        m_StageSwap.OnStadiumChanged += KeeperFlyAway;

    }
    // Start is called before the first frame update
    void Start()
    {
        // ���݂̃X�^�W�A��(�ŏ��ɐݒ肳��Ă���X�^�W�A��)�𐶐����Ă���
        Instantiate(m_CurrentStadium, gameObject.transform.position, Quaternion.identity);
    }

    void OnDisable()
    {
        m_TurnManager.OnTurnChanged -= hSwap;
        m_StageSwap.OnStadiumChanged -= KeeperFlyAway;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsSwap) return;  // �؂�ւ��t���O��������Ζ߂�

        // ���̃X�^�W�A���𐶐����Ă���
        Instantiate(m_CurrentStadium, gameObject.transform.position, Quaternion.identity);
        // ���݂̃X�^�W�A����j��
        Destroy(gameObject);
    }

    // �ؑւ��邩�`�F�b�N�֐�
    private bool SwapCheck(TURN_STATE _state)
    {
        if (!PhotonNetwork.IsMasterClient) return false;  // �}�X�^�[�N���C�A���g�łȂ���΂Ȃɂ����Ȃ�

        if (!m_IsSwap) return false;   // �ؑփt���O��true�łȂ���ΐؑւȂ�
        if (_state != TURN_STATE.STADIUMSWAP_INTERVAL) return false;  // �X�e�[�W�ؑ�

        // ���̃^�C�v���Ԉ���Ă��邩�A���݂̃X�^�W�A���^�C�v�Ɠ����Ȃ�ؑւȂ�
        if (m_NextType >= STADIUM_TYPE.STADIUM_TYPE_MAX || m_NextType == m_CurrentType) return false;

        return true;
    }

    private void hSwap(TURN_STATE _state)
    {
        if (!SwapCheck(_state)) return; //�`�F�b�N����false�Ȃ�ؑւȂ�

        for (int i = 0; i < m_Stadiums.Length; i++)
        {
            if (m_NextType == m_Stadiums[i].m_StadiumType)
            {
                m_CreateObjectName = m_Stadiums[i].m_PrefabName; // �����p�ϐ��Ɏ��̃X�^�W�A���̃v���n�u�o�^
                m_CurrentType = m_NextType;                          // ���݂̃X�^�W�A���������̃^�C�v�ɕύX
                break;
            }

            if (i == m_Stadiums.Length) return;   // ������Ȃ������ꍇ�͐ؑւȂ�
        }

        // ���݂̃X�^�W�A�����폜����p�ϐ��Ɉڂ�
        GameObject destroyObject = m_CurrentStadium;

        // ���̃X�^�W�A���𐶐����Č��݂̃X�^�W�A���ɓo�^
        m_CurrentStadium = PhotonNetwork.Instantiate(m_CreateObjectName, m_Position, Quaternion.identity);

        // ��قǂ܂Ŏg���Ă����X�^�W�A�����폜
        PhotonNetwork.Destroy(destroyObject);

        return;
    }

    public void SetNextStadium(STADIUM_TYPE _type)
    {
        m_NextType = _type;
        m_IsSwap = true;
    }

    public void RandomSetNextStadium()
    {
        int type = UnityEngine.Random.Range(0, (int)STADIUM_TYPE.STADIUM_TYPE_MAX);
        m_NextType = (STADIUM_TYPE)type;
        m_IsSwap = true;
    }

    private void KeeperFlyAway(Swap.STADIUM_TYPE _type)
    { 
        if (_type == Swap.STADIUM_TYPE.DOUBLEGOAL && m_tagetPlayer != null)
        {
            Vector3 newPosition = m_tagetPlayer.transform.position + new Vector3(0f, 0f, 20f);
            m_tagetPlayer.transform.position = newPosition;
        }
    }
}