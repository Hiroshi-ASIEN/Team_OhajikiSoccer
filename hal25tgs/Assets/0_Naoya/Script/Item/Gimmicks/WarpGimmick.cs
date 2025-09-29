using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class WarpGimmick : MonoBehaviour
{
    //���̃^�[���ʉ߂����v���C���[�����@���^�[�����Z�b�g...
    //���Ă��������Ƃ��낾���ǁA���[�v�̐^��Ŏ~�܂�����ǂ�����H
    //���̃^�[���̊J�n���Ƀ��[�v�̏�ɂ��Ă�ok
    [Header("����")]
    [SerializeField] private float m_AddPower = 1.25f;

    [Header("���[�v�P�́i�q�I�u�W�F�N�g�j")]
    [SerializeField] private WarpUnit m_warpUnitA;
    [SerializeField] private WarpUnit m_warpUnitB;

    //�G�ꂽ�v���C���[�̃��X�g�@�v���C���[���}�ɏ����Ȃ����Ƃ�]��
    List<PlayerObject> m_playerList = new List<PlayerObject>();
    List<EnemyAI> m_enemyList = new List<EnemyAI>();


    bool m_isDetectingStayCollision = false;
    int m_detectCount = 0;


    //������t���Ȃ��ƁA�������W�ɏo���Ⴄ
    //public event Action ;   // �Q�[���I�Ղɍ����|���������ɔ�������C�x���g

    //�����́A�q���v���C���[�ƐG�ꂽ���Ƃ��A�q���瑗�M����Ȃ��Ƃ�����


    private void Awake()
    {
        TurnManager.Instance.OnTurnChanged += AddTurnWarp;
    }

    private void OnDestroy()
    {
        TurnManager.Instance.OnTurnChanged -= AddTurnWarp;
    }


    //�^�[�����o�߂�������ċ�����iTurnManager����g�p�j
    public void AddTurnWarp(TurnManager.TURN_STATE _state)
    {
        if (_state == TurnManager.TURN_STATE.ACTIVE_TURN)
        {
            //�v���C���[�̃��X�g�@���Z�b�g
            ClearPlayerList();
            ClearEnemyList();

            m_isDetectingStayCollision = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("���[�v�̐e�̃Z�b�g�����݂܂���");

        //�o�^
        if (m_warpUnitA)
            m_warpUnitA.SetWarpGimmick(this);
        else
            Debug.LogWarning("�q�����܂���");
        if (m_warpUnitB)
            m_warpUnitB.SetWarpGimmick(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_isDetectingStayCollision)
        {
            if (m_detectCount > 3)
            {
                m_isDetectingStayCollision = false;
                m_detectCount = 0;
            }
            m_detectCount++;
        }
    }



    //�G�ꂽ�v���C���[���󂯎��A���[�v������
    public void RecievePlayer(PlayerObject _player, WarpUnit _warpUnit)
    {
        if (m_warpUnitA == null)
        {
            Debug.LogWarning("�q�I�u�W�F�N�g���Ȃ��A���[�v���o���܂���");
            return;
        }
        if (m_warpUnitB == null)
        {
            Debug.LogWarning("�q�I�u�W�F�N�g���Ȃ��A���[�v���o���܂���");
            return;
        }


        if (m_playerList.Contains(_player))
        {
            //���łɂ���
            Debug.Log("���łɓ��^�[�����Ń��[�v�����܂���");
            return;
        }

        //�v���C���[�����g�̃V�[���Ő����������ǂ���
        if (_player.gameObject.TryGetComponent<PhotonView>(out var photonView))
        {
            if(photonView.IsMine==false)
            {
                Debug.Log("�O���Ő������ꂽ�v���C���[�̓��[�v�����܂���");
                return;
            }
        }
        else
        {
            Debug.LogWarning("�v���C���[��PhotonView�����Ă��܂���");
        }

        //�ǉ�����
        m_playerList.Add(_player);

        //��������ֈړ�
        if (m_warpUnitA == _warpUnit)
        {
            Vector3 pos = m_warpUnitB.gameObject.transform.position;
            pos.y = _player.gameObject.transform.position.y;
            _player.gameObject.transform.position = pos;
        }
        else
        {
            Vector3 pos = m_warpUnitA.gameObject.transform.position;
            pos.y = _player.gameObject.transform.position.y;
            _player.gameObject.transform.position = pos;
        }

        //����
        if (_player.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 vel = rb.velocity;
            vel += vel.normalized * m_AddPower;

            rb.velocity = vel;
        }

        Debug.Log("���[�v�𐋍s���܂���");
    }

    void ClearPlayerList()
    {
        Debug.LogWarning("�v���C���[�̃��X�g�����Z�b�g���܂���");
        m_playerList.Clear();
    }

    //�G�ꂽ�G�l�~�[���󂯎��A���[�v������
    public void RecieveEnemy(EnemyAI _enemy, WarpUnit _warpUnit)
    {
        if (m_warpUnitA == null)
        {
            Debug.LogWarning("�q�I�u�W�F�N�g���Ȃ��A���[�v���o���܂���");
            return;
        }
        if (m_warpUnitB == null)
        {
            Debug.LogWarning("�q�I�u�W�F�N�g���Ȃ��A���[�v���o���܂���");
            return;
        }


        if (m_enemyList.Contains(_enemy))
        {
            //���łɂ���
            Debug.Log("���łɓ��^�[�����Ń��[�v�����܂���");
            return;
        }

        //�ǉ�����
        m_enemyList.Add(_enemy);

        //��������ֈړ�
        if (m_warpUnitA == _warpUnit)
        {
            _enemy.gameObject.transform.position = m_warpUnitB.gameObject.transform.position;
        }
        else
        {
            _enemy.gameObject.transform.position = m_warpUnitA.gameObject.transform.position;
        }

        //����
        if (_enemy.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 vel = rb.velocity;
            vel += vel.normalized * m_AddPower;

            rb.velocity = vel;
        }
    }

    void ClearEnemyList()
    {
        m_enemyList.Clear();
    }

    public bool GetIsDetectingStayCollision()
    {
        return m_isDetectingStayCollision;
    }

}
