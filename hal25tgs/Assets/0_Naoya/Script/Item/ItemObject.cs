using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ITEM_STATE
{
    SUCCESS,    //�g�p����
    FAILURE,    //�g�p���s
    RUNNING,    //�g�p��

    JUDGING,    //���ʔ��蒆
}


//�������A���ʁA�O�������́A2�������̂��Ǘ�����
public class ItemObject : MonoBehaviour
{
 
    [SerializeField] ItemEffect_Base m_effect;  //����

    [SerializeField] Item3DObject m_3Dobj;  //3D�����i�q���j
    [SerializeField] Item2DObject m_2Dobj;  //UI�����i�q���j

    ItemInventory m_inventory = null;

    //�擾���ꂽ���ǂ���
    bool m_isGot = false;


    void Start()
    {
        m_2Dobj.gameObject.SetActive(false);

        if(m_effect)
            m_effect.SetItemObject(this);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        //3D�̌���
        if (m_3Dobj != null)
        {
            CollisionDetection();
        }

        //2D�̌���
        if(m_2Dobj.GetIsUsed())
        {
            //�C���x���g���ɁA�g�p�J�n�̍��}�𑗂�A�폜���ނ����ōs��
            if (m_inventory)
            {
                ITEM_STATE state = ITEM_STATE.JUDGING;
                state = m_inventory.UseItemMaster(this);

                //���s������A�g��Ȃ��������Ƃɂ���
                if (state == ITEM_STATE.FAILURE)
                    m_2Dobj.UseFailed();
                else if (state == ITEM_STATE.RUNNING)
                {
                    //�g�p��

                }
            }
            else
                Debug.LogWarning("�A�C�e���ɃC���x���g�����o�^����Ă��܂���I");
        }

    }


    //�o�^
    public void SetInventory(ItemInventory itemInventory)
    {
        m_inventory = itemInventory;
    }
    public ItemInventory GetInventory() { return m_inventory; }

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
    
    public ITEM_STATE EffectByEnemy()
    {
        if (m_effect)
            return m_effect.EffectByEnemy();


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

    private void CollisionDetection()
    {
        //3D���G�ꂽ��
        PlayerObject player = m_3Dobj.GetCollidedPlayer();
        if (player != null)
        {
            //�v���C���[����`�[���ɃA�N�Z�X���Ēǉ�
            player.GetTeamObject().AddItemToInventory(this);

            //�؂�ւ�
            Destroy(m_3Dobj.gameObject);
            m_2Dobj.gameObject.SetActive(true);

            //�擾���ꂽ
            m_isGot = true;
        }
        EnemyAI enemy = m_3Dobj.GetCollidedEnemy();
        if (enemy != null)
        {
            Debug.Log("�G�l�~�[�Ƃ̏Փ˂����m");

            //�v���C���[����`�[���ɃA�N�Z�X���Ēǉ�
            AITeamObject team = enemy.GetTeam();
            if (team != null)
                team.AddItemToInventory(this);
            else
                Debug.LogWarning("�G�l�~�[���`�[�����o�^����Ă��܂���");


            //�؂�ւ�
            Destroy(m_3Dobj.gameObject);
            m_2Dobj.gameObject.SetActive(true);

            //�擾���ꂽ
            m_isGot = true;
        }
    }


    public void ConsiderUsingByEnemy()
    {
        if(m_effect)
        {
            //�g�p����
            if (m_effect.ConsiderUsingByEnemy() == true)
                m_2Dobj.UseItem();
        }
    }
}
