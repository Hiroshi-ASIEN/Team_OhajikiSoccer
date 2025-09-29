using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ҁ@�͏㏮��
/// 
/// �͂��Ƃ肭�񂪍���Ă��ꂽItem_Boost�̐݌v�v�z�����̂܂܂�
/// �v���C���[�S�̂ɓK�p�A���G�l�~�[���g�p�ł���悤�ɂ����B
/// </summary>

public class Item_Boost_Newest : MonoBehaviour
{
    [SerializeField] private float m_BoostPower = 1.2f;  // �u�[�X�g��
    [SerializeField] private float m_BoostTime = 10.0f;  // �u�[�X�g����
    float m_secondCount = 0.0f;

    bool m_isBoosting = false;

    //Move.cs��m_PlayerSpeed�́A�����Ȃ̂ŁA������ύX����

    //�ۗL���Ă���@���X�g
    private List<PlayerMove> m_playerMoveList;
    private List<EnemyAI> m_enemyMoveList;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (m_isBoosting == false)
            return;

        m_secondCount += Time.deltaTime;
        if (m_secondCount >= m_BoostTime)
        {
            BoostEnd();

            //��������
            Destroy(this.gameObject);
            Debug.Log("Boost�͎������܂���");
        }
    }


    public void BoostStartPlayer(List<PlayerMove> moves)
    {
        //�`�[�������o�[�S���ɑ΂��āAm_PlayerSpeed��ύX����
        m_playerMoveList = moves;

        foreach (PlayerMove move in m_playerMoveList)
        {
            move.SetSpeedRate(m_BoostPower);
        }

        m_isBoosting=true;
    }

    public void BoostStartEnemy(List<EnemyAI> enemyAIs)
    {
        //�G�l�~�[�̏ꍇ�̓`�[�������o�[�S���ɑ΂��āAm_SpeedRate��ύX����
        m_enemyMoveList = enemyAIs;

        foreach (EnemyAI enemyAI in m_enemyMoveList)
        {
            enemyAI.SetSpeedRate(m_BoostPower);
        }

        m_isBoosting = true;
    }

    void BoostEnd()
    {
        Debug.Log("�u�[�X�g���I���B");


        foreach (PlayerMove move in m_playerMoveList)
        {
            move.SetSpeedRate(1.0f);
        }
        foreach (EnemyAI enemyAI in m_enemyMoveList)
        {
            enemyAI.SetSpeedRate(1.0f);
        }

    }


}
