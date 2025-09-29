using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryPosAI
{
    None = -1,
    Left,
    Right,
}


public class ItemInventoryAI : MonoBehaviourPunCallbacks
{
    //�ۗL���Ă���A�C�e���@���X�g
    private List<ItemObject> m_itemList = new List<ItemObject>();

    //�ۗL���Ă���`�[��
    private TeamObject m_team = null;

    //�L�����o�X
    [Header("�L�����o�X�̃v���n�u����")]
    [SerializeField] private GameObject m_canvasPrefabLeft;
    [SerializeField] private GameObject m_canvasPrefabRight;

    [Header("�L�����o�X�̃C���X�^���X")]
   /* [SerializeField] */private GameObject m_canvas;
    Vector2 m_offset = new Vector2(0, 0);

    //�����̉�ʂň����Ă���`�[�����ǂ���
    private bool m_isOwner = false;


    //���͎G�ɍ��ォ�珇�ɂ���Ă���
    [Header("�A�C�e���̍ő�ۗL��")]
    /*[SerializeField]*/ private int m_itemMax = 3;
    //[Header("�A�C�e����V�K�œ��肵���ہA�㏑�����邩�ǂ���")]
    //[SerializeField] private bool m_isOverWriting = false;

    //[Header("�A�C�e���g�iCanvas���̃C���X�^���X�j")]
    //[SerializeField]
    private List<Image> m_itemFramesList;



    //���ݎg�p���̃A�C�e���̃X�e�[�g
    ITEM_STATE m_state = ITEM_STATE.JUDGING;
    //���ݎg�p���̃A�C�e���̃C���X�^���X
    ItemObject m_item;
    //���ݎg�p���̃A�C�e���̃C���f�b�N�X
    int m_itemIndex;


    void Start()
    {



    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //�^�[���̏�Ԃɂ���ĐF��ς��邩�@���t���[���̑���͌����Ȃ�
        //m_itemFramesList





        //�A�C�e���̌��ʑ҂�
        if (m_state ==ITEM_STATE.RUNNING)
        {
            //���ʂ��擾����
            if (m_item)
                m_state = m_item.GetItemState();


            if (m_state == ITEM_STATE.SUCCESS)
            {
                //�폜
                RemoveAndDestroyItem(m_item);

                //�������PC���ł��A�C�e���𔭓�����
                if (m_team)
                {
                    m_team.GetGameManager().SendItemUsingToOtherTeam(m_team, m_itemIndex);
                }
                else
                    Debug.LogWarning("�C���x���g���Ƀ`�[�����o�^����Ă��܂���");
            }
            else if(m_state==ITEM_STATE.FAILURE)
            {
                //���s
            }
        }
    }


    public void SetInventoryPositionAndInit(InventoryPos inventoryPos)
    {
        if (inventoryPos == InventoryPos.Left)
        {
            Debug.Log("�A�C�e���C���x���g���i���j�̃L�����o�X���������݂܂���");

            if (m_canvasPrefabLeft)
                m_canvas = Instantiate(m_canvasPrefabLeft);
        }
        else if(inventoryPos == InventoryPos.Right)
        {
            Debug.Log("�A�C�e���C���x���g���i�E�j�̃L�����o�X���������݂܂���");

            if (m_canvasPrefabRight)
                m_canvas = Instantiate(m_canvasPrefabRight);
        }


        if (!m_canvas)
            return;

        m_itemFramesList = new List<Image>();

        //�L�����o�X����Image��T���Ċi�[�i�Ђǂ��R�[�h�ɂȂ��Ă��܂����ȁj
        for (int i = 0; i < m_canvas.transform.childCount; i++)
        {
            if (m_canvas.transform.GetChild(i).gameObject.TryGetComponent<Image>(out var image))
            {
                m_itemFramesList.Add(image);
            }
        }


        //m_itemFramesList.Add;

        //�A�C�e���g�̐��ōő�ۗL�������߂悤
        m_itemMax = m_itemFramesList.Count;

        //�L�����o�X���q�I�u�W�F�N�g�ɂ���
        m_canvas.transform.SetParent(this.gameObject.transform, false);
    }


    /// <summary>
    /// �A�C�e����ǉ�����i�`�[������Ăяo�����j
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemObject item)
    {
        //���߂��邩�`�F�b�N
        if (m_itemList.Count >= m_itemMax)
            return;

        if (!m_canvas)
            return;

        //�f�[�^��ύX����
        m_itemList.Add(item);
        //�L�����o�X�Ɉړ�������
        item.gameObject.transform.SetParent(m_canvas.transform);
        //�@�q�G�����L�[�ň�ԉ��Ɉړ����A�O�ʂɕ\�������
        item.gameObject.transform.SetAsLastSibling();
        item.gameObject.transform.position = new Vector3(960, 540, 0);//����A�L�����o�X�̐^�񒆂ɂ����Ă�����ĈӖ����B

        //�C���f�b�N�X�����߂�
        int index = m_itemList.Count - 1;
        //����
        SortByFramePos(index);


        //�����ŃA�C�e���ɑ΂��ă`�[����o�^����@�G�ꂽ�������ł��������H
        //item.SetInventory(this);
    }


    //������Remove�����ǁAUse���������ɂ��Ă��ǂ��@�Ă����������������ł��邱�ƂɂȂ�


    /// <summary>
    /// �A�C�e���g�p�@�i�}�X�^�[���̃A�C�e���I�u�W�F�N�g����Ă΂��j������A�}�X�^�[�N���C�A���g����Ȃ��A�C�e���g�p�ҁH
    /// </summary>
    /// <param name="item"></param>
    public ITEM_STATE UseItemMaster(ItemObject item)
    {
        //�^�[���}�l�[�W���[����A���݂̏�Ԃ��擾
        if (TurnManager.Instance.GetCurrentTurnState() != TurnManager.TURN_STATE.PLAY_TURN)
        {
            Debug.Log("����^�[�����̂݃A�C�e�����g�p�ł��܂��B");
            return ITEM_STATE.FAILURE;
        }




        //���ݎg�p���Ȃ炾��
        if (m_state == ITEM_STATE.RUNNING)
            return ITEM_STATE.RUNNING;



        //�g�p�ł��邩
        bool isUsable = CheckIsUsableItem(item);

        if (isUsable==false)
            return ITEM_STATE.FAILURE;

        //�ԍ��擾
        m_itemIndex = m_itemList.IndexOf(item);
        if (m_itemIndex < 0)
            return ITEM_STATE.FAILURE;

        m_item = item;

        //���s�����݂�
        m_state = TryToExecuteItem(m_item);

        if(m_state == ITEM_STATE.SUCCESS)
        {
            //�폜
            RemoveAndDestroyItem(m_item);

            //�������PC���ł��A�C�e���𔭓�����
            if (m_team)
            {
                m_team.GetGameManager().SendItemUsingToOtherTeam(m_team, m_itemIndex);
                return ITEM_STATE.SUCCESS;
            }
            else
                Debug.LogWarning("�C���x���g���Ƀ`�[�����o�^����Ă��܂���");


            return ITEM_STATE.FAILURE;
        }
        else
        {
            return m_state;
        }
    }

    public void UseItemUnmaster(int _itemIndex)//��}�X�^�[���ŌĂ΂��
    {
        //�w��̔ԍ��̃A�C�e�����g�p����
        Debug.Log("�����ʂɂăA�C�e�����g�p����܂����I");

        ItemObject item = m_itemList[_itemIndex];


        //������������������������������������������������������������������������
        //����A�����Ŕ�������Ӗ����邩�H
        //������Item��Effect���ł��ł�Photon�̓����i�Ⴆ�΃G�t�F�N�g�����A�I�u�W�F�N�g�����A�{�[���ό^�j
        //����������Ă�����A��񔭓�����Ƌt�ɂ悭�Ȃ��ȁI
        //
        //������A�{���ɕK�v�Ȃ̂͂�����������C���x���g���̒��g�𓯊�������d�g�݂Ȃ̂�������Ȃ���
        //�ʂ̉�ʂɑ��݂���C���x���g�����ǂ����������悤���ȁ[
        //�����R�s�[�ł��Ȃ����ȁB���͂�߂Ă������B��肪�o�Ă���Ώ����܂��傤�B

        //ExecuteItem(item);
        RemoveAndDestroyItem(item);

        //������������������������������������������������������������������������


    }


    private ITEM_STATE TryToExecuteItem(ItemObject _item)
    {
        if (!_item)
        {
            //�̂��ɁA�A�C�e�������݂����Ƃ������I�Ɏg�p���Ă����������H�ÓI�֐����Ȃ�
            Debug.Log("�g�p���悤�Ƃ����A�C�e�������݂��܂���I�I");
            return ITEM_STATE.FAILURE;
        }

        //���ʔ����@�i���݂�Ƃ���ƁA���ۂɂł������A���Ƃŕ����Ȃ��ƂȂ��j
        return _item.Effect();

    }




    public void RemoveAndDestroyItem(ItemObject item) {  
        
        //�Q�ƍ폜
        m_item = null;

        m_itemList.Remove(item);
        Destroy(item.gameObject);

        //����
        SortByFrameEveryItem();
    }

    //���l�߂���
    private void SortCloser(int _index)
    {
        ItemObject item = m_itemList[_index];

        if (!item)
            return;

        //�r�W���A����ύX����i�ʒu���ȁj
        RectTransform rect = item.Get2DobjRectTransform();
        rect.transform.localPosition = Vector3.zero;

        //�ۗL���Ă��鐔�ɍ��킹��...�܂��A100�Ƃ��Ƃ����H
        Vector2 pos = new Vector2(100 * _index, 0);

        rect.transform.localPosition = (pos + m_offset);
    }

    //�S�Ă𐮗�
    private void SortCloserEveryItem()
    {
        for (int i = 0; i < m_itemList.Count; i++)
            SortCloser(i);
    }


    private void SortByFramePos(int _index)
    {
        ItemObject item = m_itemList[_index];

        if (!item)
            return;

        ////�r�W���A����ύX����i�ʒu���ȁj
        //RectTransform rect = item.Get2DobjRectTransform();
        //rect.transform.localPosition = Vector3.zero;

        //Debug.Log(rect);
        //Debug.Log(m_itemFramesList[_index]);

        //rect.transform.localPosition = m_itemFramesList[_index].transform.localPosition;



        //�g�̎q���Ɉړ�������
        item.gameObject.transform.SetParent(m_itemFramesList[_index].transform);
        item.gameObject.transform.localPosition = Vector3.zero;

    }

    private void SortByFrameEveryItem()
    {
        for (int i = 0; i < m_itemList.Count; i++)
            SortByFramePos(i);
    }


    private void DisplayInfomation()
    {
        Debug.Log(m_itemList.Count);

        for (int i=0;i<m_itemList.Count;i++)
        {
            Debug.Log(m_itemList[i]);
        }
    }


    /// <summary>
    /// �`�[���o�^�i�`�[���I�u�W�F�N�g����Ă΂��j
    /// </summary>
    /// <param name="_team"></param>
    public void SetTeam(TeamObject _team)
    {
        m_team = _team;
    }


    private bool CheckIsUsableItem(ItemObject _item)
    {
        //�����Ŕ��肵����


        //
        return true;
    }


    //�C���x���g���̓�����������@�������I��������ӂ��K�v���ȁB
    //��{�I�ɃA�C�e�����v���C���[���������Ă��邩��A�擾�Ɋւ��Ă�
    //�����Ă����Ă���{��������͂��B
    //�Ȃ�Ȃ�擾��l�͂œ������悤�Ƃ���ƁA���x�͎擾���ꂽ�A�C�e����
    //�������Ƃ�Ȃ��Ƃ����Ȃ��Ȃ���...�]�v��₱�����Ȃ�C������]�C

    //���́A�g�p���݂̂̓����ɂ��܂��Ă������I�I�I





}
