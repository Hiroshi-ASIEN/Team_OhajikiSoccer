using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// �����܂�ݒu�A�C�e���@�i�����܂肻�̂��̂ł͂Ȃ��̂Œ��Ӂj
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_PutPuddle : ItemEffect_Base
{
    //����A�C���X�^���X����Ȃ��Ă������ȁB�V�K�����ł���
    [Header("�M�~�b�N�ݒu�҂̃C���X�^���X�i�q�I�u�W�F�H�j")]
    [SerializeField] private GimmickPutter m_gimmickPutter;

    //�v���n�u
    [Header("�����܂�̃v���n�u")]
    [SerializeField] private GameObject m_puddle;


    [Header("�G�l�~�[���g�p����܂ł̕b��")]
    [SerializeField] float m_secondToUseByEnemy = 3.0f;
    float m_secondCount = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        //�ŏ��̓A�N�e�B�u�I�t�ɂ���
        if (m_gimmickPutter)
            m_gimmickPutter.gameObject.SetActive(false);
        else
            Debug.LogWarning("ItemEffect_PutPuddle��GimmickPutter���o�^����Ă��܂���I");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(m_gimmickPutter)
        {
            //�X�e�[�g�X�V�@������ItemObject�ɑ����āAInventory�ɑ�����i�����Ȃ��j
            m_state = m_gimmickPutter.GetState();

            if(m_state==ITEM_STATE.FAILURE)
            {
                //�A�N�e�B�u���~
                m_gimmickPutter.gameObject.SetActive(false);
            }

        }
        
    }

    public override ITEM_STATE Effect()
    {
        if(!m_puddle)
        {
            Debug.LogWarning("ItemEffect_PutPuddle�ɐ����܂�̃v���n�u���o�^����Ă��܂���I");
            return ITEM_STATE.FAILURE;
        }

        //�M�~�b�N�v�b�^�[�Ƀv���n�u��o�^
        if (m_gimmickPutter)
        {
            //�A�N�e�B�u���J�n
            m_gimmickPutter.gameObject.SetActive(true);
            m_gimmickPutter.SetPrefab(m_puddle);
            m_gimmickPutter.StartPutting();
        }
        else
        {
            Debug.LogWarning("ItemEffect_PutPuddle��GimmickPutter���o�^����Ă��܂���I");
            return ITEM_STATE.FAILURE;
        }

        Debug.Log("�����܂�ݒu�J�n�����݂܂����B");
        m_state = ITEM_STATE.RUNNING;
        return ITEM_STATE.RUNNING;
    }


    public override ITEM_STATE EffectByEnemy()
    {
        if (!m_puddle)
        {
            Debug.LogWarning("ItemEffect_PutPuddle�ɐ����܂�̃v���n�u���o�^����Ă��܂���I");
            return ITEM_STATE.FAILURE;
        }


        //�{�[����T���āB�����ɁH�ݒu����@���͓K����
        Vector3 pos = Vector3.zero;
        pos = GameObject.FindGameObjectWithTag("Ball").transform.position;
        pos += Vector3.left;

        GameObject newGameObject =
            Instantiate(m_puddle, pos, Quaternion.identity) as GameObject;

        return ITEM_STATE.SUCCESS;
    }


    public override bool ConsiderUsingByEnemy()
    {
        m_secondCount += Time.deltaTime;
        if (m_secondCount >= m_secondToUseByEnemy)
        {
            return true;
        }

        return false;
    }


}
