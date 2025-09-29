using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeper : MonoBehaviour
{
    [Header("�ړ��͈�")]
    [SerializeField] private float m_LeftLimit = -5.0f;
    [SerializeField] private float m_RightLimit = 5.0f;

    private Rigidbody m_Rigidbody;
    private void Start()
    {
//        m_LeftLimit = m_LPost.transform.position.z;
//       m_RightLimit = m_RPost.transform.position.z;
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float posZ = transform.position.z;

        if (posZ >= m_RightLimit && m_Rigidbody.velocity.z > 0)
        {
            // �E�[�ŉE�ɐi��ł���Ȃ甽��
            ReflectX();
            transform.position = new Vector3(transform.position.x, transform.position.y, m_RightLimit);
        }
        else if (posZ <= m_LeftLimit && m_Rigidbody.velocity.z < 0)
        {
            // ���[�ō��ɐi��ł���Ȃ甽��
            ReflectX();
            transform.position = new Vector3(transform.position.x, transform.position.y, m_LeftLimit);
        }
    }

    private void ReflectX()
    {
        Vector3 velocity = m_Rigidbody.velocity;
        velocity.z *= -1f;
        m_Rigidbody.velocity = velocity;
    }
}
