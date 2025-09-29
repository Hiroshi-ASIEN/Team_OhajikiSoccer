using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Boost : MonoBehaviour
{
    public GameObject player;              // Player�I�u�W�F�N�g
    public float m_BoostAmount = 5.0f;        // �ǉ�����X�s�[�h
    public float m_BoostDuration = 60f;       // �u�[�X�g�̌p������

    private float m_OriginalSpeed;           // ���̃X�s�[�h
    private bool m_IsBoosting = false;       // �u�[�X�g�����ǂ���

    void Update()
    {
        // �X�y�[�X�L�[����������u�[�X�g
        if (Input.GetKeyDown(KeyCode.Space) && !m_IsBoosting)
        {
            StartCoroutine(Boost());
            Debug.Log("�X�s�[�h�A�b�v����");
        }
    }

    private System.Collections.IEnumerator Boost()
    {
        m_IsBoosting = true;

        // m_PlayerSpeed �ɃA�N�Z�X
        var playerScript = player.GetComponent<Move>();
        if (playerScript != null)
        {
            m_OriginalSpeed = playerScript.m_PlayerSpeed;
            playerScript.m_PlayerSpeed += m_BoostAmount;

            Debug.Log(playerScript.m_PlayerSpeed);

            yield return new WaitForSeconds(m_BoostDuration);
        }

        m_IsBoosting = false;
    }
}
