using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [Header("����")]
    [SerializeField] private float m_AddPower = 1.25f;

    [Header("�f�o�b�O�p")]
    [SerializeField] private Vector3 m_Position;
    [SerializeField] private bool m_TestWarp = false;
    [SerializeField] private bool m_FreezeY = false;

    private Rigidbody m_Rigidbody;  // Player��Rigidbody�擾�p
    private Vector3 m_Velocity;     // Player�ړ��擾�p
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        WarpPlayer();
        RigidbodyFreeze();

        if (m_FreezeY)
        {
            float time = Time.time;
            time += Time.deltaTime;
            if (time > 10.0)
            {
                m_FreezeY = false;
                RigidbodyFreeze();
            }
        }
    }

    void WarpPlayer()
    {
        if (!m_TestWarp) return;

        // �]��
        this.gameObject.transform.position = m_Position;

        // velocity���擾���Ĉړ������Ƀv���C���[������
        m_Velocity = m_Rigidbody.velocity;
        m_Rigidbody.AddForce(m_Velocity * m_AddPower, ForceMode.Impulse);

        m_FreezeY = true;
        RigidbodyFreeze();  // Y���Œ�
        m_TestWarp = false;
    }

    void RigidbodyFreeze()
    {
        if (m_FreezeY)
        {
            m_Rigidbody.constraints =
                RigidbodyConstraints.FreezePositionY |
                RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
