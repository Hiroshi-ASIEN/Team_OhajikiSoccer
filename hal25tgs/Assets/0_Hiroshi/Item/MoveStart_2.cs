using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStart_2 : MonoBehaviourPunCallbacks
{
    private bool m_Move = false;    // �v���C���[��������

    [SerializeField] private CreatePlayer m_CreatePlayer;
    [SerializeField] private TurnManager m_TurnManager;
    Timer m_TMTimer;

    GameObject[] m_Players;
    [SerializeField] private bool m_Burst = false;   // �o�[�X�g�^�C������

    // Start is called before the first frame update
    void Start()
    {
        m_TMTimer = m_TurnManager.GetTurnTimer();    // TM�Ŏg�p���̃^�C�}�[�擾
    }

    private void FixedUpdate()
    {
        if (!m_TurnManager.IsBurstTimeActive())
        {
            if (m_Burst) m_TurnManager.BurstTimeStart();
            m_Burst = false;
        }
    }

    // �^�[�������̓���
    private void TurnTime()
    {
        // �v���C���[���쎞�Ԓ��@�{�@�v���C���[���˂��ĂȂ�
        if (!m_TurnManager.IsPlayTimeTurn() && !m_Move)
        {
            for (int i = 0; i < m_CreatePlayer.m_Players.Length; i++)
            {
                // �v���C���[���������X�N���v�g����v���C���[���[�u�擾
                PlayerMove playerMove = m_CreatePlayer.m_Players[i].GetComponent<PlayerMove>();
                playerMove.Move();
            }
            m_Move = true;  // �v���C���[����
        }
        // ��󑀍쎞�Ԓ��@�{�@�v���C���[���ˍς�
        if (m_TurnManager.IsPlayTimeTurn() && m_Move)
        {
            m_Move = false; // �v���C���[���ˉ\�ɂ���
        }
    }

}
