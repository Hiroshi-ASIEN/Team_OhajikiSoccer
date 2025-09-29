using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private Vector3 m_InitPos;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        m_InitPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            rb.isKinematic = true;
            transform.position = m_InitPos;
            rb.isKinematic = false;
        }
    }
}
