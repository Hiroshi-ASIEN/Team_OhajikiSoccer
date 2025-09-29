using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]

public class WarpUnit : MonoBehaviour
{
    [Header("���[�v�e")]
    [SerializeField] private WarpGimmick m_warpGimmick;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWarpGimmick(WarpGimmick _warp)
    {
        m_warpGimmick = _warp;
        Debug.Log("���[�v�̐e���Z�b�g����܂���");

    }

    //�G�ꂽ�v���C���[�𑗂�@���₟�BEnter�ɂ������������ǁA�ŏ���Stay�Ȃ񂾂�Ȃ��@�G���^�[�ł�����
    private void OnTriggerEnter(Collider other)
    {
        //�v���C���[���C���[�����𔻒肵�����Ȃ�
        //���͂Ƃ肠�����R���|�[�l���g�Ŕ��肷��

        if (other.gameObject.TryGetComponent<PlayerObject>(out var player))
        {
            Debug.Log("���[�v�����݂܂���");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecievePlayer(player, this);
            }
        }
        else if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
        {
            Debug.Log("���[�v�����݂܂���");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecieveEnemy(enemy, this);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (m_warpGimmick)
        {
            if(m_warpGimmick.GetIsDetectingStayCollision() == false)
            {
                return;
            }
        }

        //�v���C���[���C���[�����𔻒肵�����Ȃ�
        //���͂Ƃ肠�����R���|�[�l���g�Ŕ��肷��
        if (other.gameObject.TryGetComponent<PlayerObject>(out var player))
        {
            Debug.Log("���[�v�����݂܂���");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecievePlayer(player, this);
            }
            else
            {
                Debug.Log("�e�����܂���");
            }
        }
        else if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
        {
            Debug.Log("���[�v�����݂܂���");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecieveEnemy(enemy, this);
            }
        }
    }


}
