using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KeeperAI : MonoBehaviour
{
    [Header("ñÓàÛ")]
    [SerializeField] private GameObject m_Arrow;

    private Transform m_Ball;

    [Header("à⁄ìÆë¨ìx")]
    [SerializeField] private float m_MaxSpeed = 5.0f;

    [Header("ècï˚å¸ÇÃà⁄ìÆîÕàÕ")]
    [SerializeField] private float m_VerticalRange = 5.0f;

    private Rigidbody m_Rb;
    private float m_StartPosX;
    private float m_StartPosZ;
    private float m_HalfRange;
    private float m_NextTargetZ;

    // Start is called before the first frame update
    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();

        m_Rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        m_Ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();

        m_StartPosX = transform.position.x;
        m_StartPosZ = transform.position.z;

        m_HalfRange = m_VerticalRange * 0.5f;

        if (m_Arrow)
        {
            m_Arrow.gameObject.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (m_Arrow && m_Arrow.activeSelf)
        {
            Vector3 targetPos = new Vector3(
                m_StartPosX,
                transform.position.y,
                m_NextTargetZ
            );

            Vector3 flat = targetPos - transform.position;
            flat.y = 0f;

            if (flat.sqrMagnitude > 0.001f)
            {
                Quaternion arrowRot = Quaternion.FromToRotation(Vector3.left, flat.normalized);
                m_Arrow.transform.rotation = arrowRot;
            }
        }

    }

    public void PrepareMove()
    {
        float desiredZ = m_Ball.position.z;

        m_NextTargetZ = Mathf.Clamp(desiredZ, m_StartPosZ - m_HalfRange, m_StartPosZ + m_HalfRange);

        if (m_Arrow)
        {
            m_Arrow.SetActive(true);
        }
    }

    public void Move()
    {
        if (m_Arrow)
            m_Arrow.gameObject.SetActive(false);

        Vector3 pos = transform.position;

        Vector3 target = new Vector3(m_StartPosX, pos.y, m_NextTargetZ);

        Vector3 delta = (target - pos);
        delta.y = 0;
        float dist = delta.magnitude;

        if (dist < 0.01f)
        {
            m_Rb.velocity = Vector3.zero;
            return;
        }

        Vector3 desiredVel = delta.normalized * m_MaxSpeed;

        if (pos.z <= m_StartPosZ - m_HalfRange && desiredVel.z < 0f)
            desiredVel.z = 0f;
        else if (pos.z >= m_StartPosZ + m_HalfRange && desiredVel.z > 0f)
            desiredVel.z = 0f;

        m_Rb.velocity = desiredVel;
    }
}
