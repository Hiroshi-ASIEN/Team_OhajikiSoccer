using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// �u�[�X�g�A�C�e��
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_Boost : ItemEffect_Base
{
    [SerializeField] GameObject m_boost;


    [Header("�G�l�~�[���g�p����܂ł̕b��")]
    [SerializeField] float m_secondToUseByEnemy = 3.0f;
    float m_secondCount = 0.0f;




    void Start()
    {

    }

    public override ITEM_STATE Effect()
    {
        if (m_boost)
        {
            //���������ɂ������ʂ��Ȃ��Ȃ��Photon���o�R�����ɍ���Ă悢�̂ł�
            GameObject instance = Instantiate(m_boost, this.transform.position, Quaternion.identity);
            //GameObject instance = PhotonNetwork.Instantiate(m_boost.name, this.transform.position, Quaternion.identity);

            if (instance.TryGetComponent<Item_Boost_Newest>(out var boost))
            {
                if (m_itemObject)
                {
                    PlayerObject[] playerArray =
                        m_itemObject.GetInventory().GetTeamObject().GetPlayersArray();

                    //��������Move�X�N���v�g���擾�@�܂��āAPlayer��Move���Ă˂��I
                    List<PlayerMove> playerMoves = new List<PlayerMove>();

                    for (int i = 0; i < playerArray.Length; i++)
                    {
                        playerMoves.Add(playerArray[i].gameObject.GetComponent<PlayerMove>());
                    }


                        boost.BoostStartPlayer(playerMoves);
                }
            }


            Debug.Log("�u�[�X�g�����݂܂����B");
        }
        else
            Debug.LogWarning("�u�[�X�g�v���n�u������܂���");



        return ITEM_STATE.SUCCESS;
    }


    public override ITEM_STATE EffectByEnemy()
    {
        //��ƈႢ�APhoton�ł͂Ȃ������Ȑ���
        if (m_boost)
        {
            GameObject instance = Instantiate(m_boost, this.transform.position, Quaternion.identity);

            if(instance.TryGetComponent<Item_Boost_Newest>(out var boost))
            {
                if(m_itemObject)
                {
                    List<EnemyAI> enemyAIs =
                        m_itemObject.GetInventory().GetAITeamObject().GetEnemyAIs();
                    
                    boost.BoostStartEnemy(enemyAIs);
                }
            }


            Debug.Log("�u�[�X�g�����݂܂����B");
        }
        else
            Debug.LogWarning("�u�[�X�g�v���n�u������܂���");


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
