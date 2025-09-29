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
        // W�L�[�i��ړ��j
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += m_Speed * transform.up * Time.deltaTime;
        }

        // S�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.Z))
        {
            transform.position -= m_Speed * transform.up * Time.deltaTime;
        }

        // D�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += m_Speed * transform.forward * Time.deltaTime;
        }

        // A�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= m_Speed * transform.forward * Time.deltaTime;
        }

        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += m_Speed * transform.right * Time.deltaTime;
        }

        // A�L�[�i���ړ��j
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
