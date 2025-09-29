using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matsu_TestMove : MonoBehaviour
{
    [SerializeField] float m_Speed = 3.0f;
    [SerializeField] Vector3 m_InitPos = new Vector3(0.0f,0.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        // Wキー（上移動）
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += m_Speed * transform.up * Time.deltaTime;
        }

        // Sキー（下移動）
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position -= m_Speed * transform.up * Time.deltaTime;
        }

        // Dキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += m_Speed * transform.forward * Time.deltaTime;
        }

        // Aキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= m_Speed * transform.forward * Time.deltaTime;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += m_Speed * transform.right * Time.deltaTime;
        }

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= m_Speed * transform.right * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().useGravity = !GetComponent<Rigidbody>().useGravity;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.gameObject.transform.rotation = Quaternion.identity;
            this.gameObject.transform.position = m_InitPos;
        }
    }
}
