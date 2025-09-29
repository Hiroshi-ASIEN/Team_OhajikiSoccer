using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//======================================================================
// �A�C�e��������
// 
// �q�G�����L�[�ɏ풓�����A�^�[���}�l�[�W���[�ɓo�^�����Ă��������B
// �^�[���}�l�[�W���[���疈�^�[�� AddTurn()�֐����ĂԂ��Ƃœ��삷��B   �^�[���̏�Ԃɒ��ӁI
// �������ˑ��֌W���t�]���������̂ł���΁A
// �^�[���}�l�[�W���[���ɁA�݌v�^�[�����ƁA�^�[�����o�߂����^�C�~���O��
// �擾�ł���֐���p�ӂ��ė~�����B
//
//
// ���^�[�����ω������Ƃ��Ɏ��s�����C�x���g���A�^�[���}�l�[�W���[�ɂ���炵���̂ŁA�����ŁI
//======================================================================

public class ItemGenerateManagerAI : MonoBehaviourPunCallbacks
{
    //�݌v�^�[����
    private int m_totalTurnNum = 1;

    [Header("���^�[���Ɉ�x�������邩")]
    [SerializeField] private int m_generateTurn = 3;

    [Header("�����ʒu�I�u�W�F�N�g")]
    [SerializeField] private GameObject m_spawnPosObjectA;
    [SerializeField] private GameObject m_spawnPosObjectB;

    [Header("�����A�C�e���ꗗ�iResorce�t�@�C���ɂ��邱�Ɓj")]
    [SerializeField] List<GameObject> m_ItemPrefabList = new List<GameObject>();//�A�C�e���{�b�N�X�̃v���n�u�����i����A�����ڂ𓝈ꂷ��΂����̂��I�j

    
    //���������A�C�e���̎Q�Ɓi�Ƃ�ꂽ���ǂ����𔻒肷��j
    private ItemObject m_itemObjectUp = null;
    private ItemObject m_itemObjectDown = null;

    //������3�^�[���o���Ă��Ƃ��Ȃ�������A���Α��H���ȁH���̂܂܂ł������B
    //���Ⴀ�A���ꂽ���ǂ�����m���Ă��Ȃ��Ƃ����Ȃ��ȁB�@���͂�����


    private void Awake()
    {
        TurnManager.Instance.OnTurnChanged += AddTurn;
    }

    private void OnDestroy()
    {
        TurnManager.Instance.OnTurnChanged -= AddTurn;
    }

    private void FixedUpdate()
    {
        //�擾���ꂽ���`�F�b�N����

        if(m_itemObjectUp)
        {
            if(m_itemObjectUp.GetIsGot())
            {
                m_itemObjectUp = null;
            }
        }
        if (m_itemObjectDown)
        {
            if (m_itemObjectDown.GetIsGot())
            {
                m_itemObjectDown = null;
            }
        }
    }


    //�^�[�����o�߂�������ċ�����iTurnManager����g�p�j
    public void AddTurn(TurnManager.TURN_STATE _state)
    {
        if (_state != TurnManager.TURN_STATE.PLAY_TURN)
        {
            Debug.Log(_state);
            Debug.Log("�^�[����Ԃ�PLAY_TURN�ł͂Ȃ��̂ŃA�C�e���������s���܂���");
            return;
        }

        //����������}�X�^�[�ł͌Ă΂Ȃ�
        if (PhotonNetwork.IsMasterClient == false)
        {
            Debug.LogWarning("�}�X�^�[�N���C�A���g�ł͂Ȃ��̂ŃA�C�e���������s���܂���");
            return;
        }

        m_totalTurnNum++;

        //�w��̔{���łȂ��Ȃ�΂Ȃɂ����Ȃ�
        if((m_totalTurnNum % m_generateTurn) != 0)
        {
            Debug.Log(m_totalTurnNum);
            Debug.Log("�^�[�������w��̔{���łȂ��̂ŃA�C�e���𐶐����܂���");
            return;
        }


        //�ȉ�����
        GenerateItem();
    }

    private void GenerateItem()
    {
        if (m_spawnPosObjectA == null)
        {
            Debug.LogWarning("�q�I�u�W�F�N�g���Ȃ��A�A�C�e���������o���܂���");
            return;
        }
        if (m_spawnPosObjectB == null)
        {
            Debug.LogWarning("�q�I�u�W�F�N�g���Ȃ��A�A�C�e���������o���܂���");
            return;
        }
        if (m_ItemPrefabList.Count <= 0)
        {
            Debug.LogWarning("�A�C�e���̃v���n�u���o�^����Ă��炸�A�����ł��܂���");
            return;
        }
        if(m_itemObjectUp && m_itemObjectDown)
        {
            Debug.Log("�A�C�e�����v���C���[�Ɏ擾���ꂸ�Ɏc���Ă���̂ŁA�������܂���");
            return;
        }


        //�v���n�u���烉���_���Ő���
        //�܂��͔ԍ��������_���Ō��߂�...
        int index =
            UnityEngine.Random.Range(0, m_ItemPrefabList.Count + 1);

        //�Q�[���I�u�W�F�N�g������
        GameObject prefab = m_ItemPrefabList[index];

        //�ʒu���擾����
        Vector3 pos;
        int random = Random.Range(0, 2);

        //�܂��͂������݂Ȃ���
        if(m_itemObjectUp)
            random = 1;
        else if(m_itemObjectDown)
            random = 0;



        if (random == 0)
        {
            pos = m_spawnPosObjectA.transform.position;
            GameObject item = SingleMultiUtility.Instantiate(prefab.name, pos, Quaternion.identity);

            if(TryGetComponent<ItemObject>(out var itemObj))
            {
                m_itemObjectUp = itemObj;
            }
        }
        else
        {
            pos = m_spawnPosObjectB.transform.position;
            GameObject item = SingleMultiUtility.Instantiate(prefab.name, pos, Quaternion.identity);

            if (TryGetComponent<ItemObject>(out var itemObj))
            {
                m_itemObjectUp = itemObj;
            }
        }

        Debug.Log("�A�C�e�����������݂܂����I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I�I");
    }
}
