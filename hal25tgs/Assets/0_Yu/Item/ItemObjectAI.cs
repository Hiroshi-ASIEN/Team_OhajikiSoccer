using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ITEM_STATE_AI
{
    SUCCESS,    //�g�p����
    FAILURE,    //�g�p���s
    RUNNING,    //�g�p��

    JUDGING,    //���ʔ��蒆
}


//�������A���ʁA�O�������́A2�������̂��Ǘ�����
public class ItemObjectAI : MonoBehaviour
{
 
    [SerializeField] ItemEffect_Base m_effect;  //����

    [SerializeField] Item3DObjectAI m_3Dobj;  //3D�����i�q���j
    [SerializeField] Item2DObjectAI m_2Dobj;  //UI�����i�q���j

    ItemInventory m_inventory = null;

    //�擾���ꂽ���ǂ���
    bool m_isGot = false;


    void Start()
    {
        m_2Dobj.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        //3D�̌���
        if (m_3Dobj != null)
        {

            //3D���G�ꂽ��
            PlayerObject player = m_3Dobj.GetCollidedPlayer();
            if (player != null)
            {
                //�؂�ւ�
                Destroy(m_3Dobj.gameObject);
                m_2Dobj.gameObject.SetActive(true);

                //�v���C���[����`�[���ɃA�N�Z�X���Ēǉ�
                //player.GetTeamObject().AddItemToInventory(this);

                //�擾���ꂽ
                m_isGot=true;

            }
        }

        //2D�̌���
        if(m_2Dobj.GetIsUsed())
        {
            //�C���x���g���ɁA�g�p�J�n�̍��}�𑗂�A�폜���ނ����ōs��
            if (m_inventory)
            {
                ITEM_STATE_AI state = ITEM_STATE_AI.JUDGING;
                //state = m_inventory.UseItemMaster(this);

                //���s������A�g��Ȃ��������Ƃɂ���
                if (state == ITEM_STATE_AI.FAILURE)
                    m_2Dobj.UseFailed();
            }
            else
                Debug.LogWarning("�A�C�e���ɃC���x���g�����o�^����Ă��܂���I");



            ////���ʔ����i���A�������Ō��ʏo���Ă邯�ǁA�C���x���g���ōs���Ă������B�Ⴆ�΁A���f������g�݂����Ƃ��Ƃ��j
            //if (m_effect)
            //    m_effect.Effect();


            ////�C���x���g������j������
            //if(m_inventory)
            //    m_inventory.RemoveItem(this);

            ////�������폜����
            //Destroy(this.gameObject);
        }

    }



    //�`�[���̓o�^
    public void SetInventory(ItemInventory itemInventory)
    {
        m_inventory = itemInventory;
    }

    public RectTransform Get2DobjRectTransform() { 
        return m_2Dobj.GetRectTransform(); 
    }

    public ITEM_STATE Effect()
    {
        if (m_effect)
            return m_effect.Effect();


        Debug.LogWarning("�A�C�e���ɃG�t�F�N�g���o�^����Ă��܂���I");
        return ITEM_STATE.FAILURE;
    }


    //�Ⴆ�΁A�ݒu�^�̃A�C�e���̂悤�ɁAEffect�ő��U���ʂ��o�Ȃ��ꍇ�A�C���x���g���͂������Q�Ƃ��Č��ʂ�����
    public ITEM_STATE GetItemState()
    {
        if (m_effect)
            return m_effect.GetState();


        Debug.LogWarning("�A�C�e���ɃG�t�F�N�g���o�^����Ă��܂���I");
        return ITEM_STATE.FAILURE;
    }


    public bool GetIsGot()
    {
        return m_isGot;
    }
}
