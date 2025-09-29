using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]float moveForce = 1;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            rb.AddForce(moveForce * Vector3.forward, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-moveForce * Vector3.forward, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-moveForce * Vector3.right, ForceMode.Impulse);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(moveForce * Vector3.right, ForceMode.Impulse);
        }
    }
}
