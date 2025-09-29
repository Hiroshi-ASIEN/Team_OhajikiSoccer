using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [Header("加速")]
    [SerializeField] private float m_AddPower = 1.25f;

    [Header("デバッグ用")]
    [SerializeField] private Vector3 m_Position;
    [SerializeField] private bool m_TestWarp = false;
    [SerializeField] private bool m_FreezeY = false;

    private Rigidbody m_Rigidbody;  // PlayerのRigidbody取得用
    private Vector3 m_Velocity;     // Player移動取得用
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

        // 転送
        this.gameObject.transform.position = m_Position;

        // velocityを取得して移動方向にプレイヤーを加速
        m_Velocity = m_Rigidbody.velocity;
        m_Rigidbody.AddForce(m_Velocity * m_AddPower, ForceMode.Impulse);

        m_FreezeY = true;
        RigidbodyFreeze();  // Y軸固定
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
