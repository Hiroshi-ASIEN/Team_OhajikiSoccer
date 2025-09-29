using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_MoveTest : MonoBehaviourPun
{
    [SerializeField] float m_Speed = 3.0f;
    [SerializeField] float m_Damping = 0.9f;    // å∏êäó¶Åi0.0Å`1.0Åj
    private GameObject m_ItemManager;
    private Vector3 vel;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        m_ItemManager = GameObject.FindGameObjectWithTag("Item");
        vel = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        vel = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) vel += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) vel -= Vector3.forward;
        if (Input.GetKey(KeyCode.D)) vel += Vector3.right;
        if (Input.GetKey(KeyCode.A)) vel -= Vector3.right;

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_ItemManager.GetComponent<Ball_Expansion>().photonView.RPC("ExpandStart", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            rb.AddForce(Vector3.forward * 30.0f, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        Vector3 currentVel = rb.velocity;
        Vector3 newVel = currentVel;

        if (vel != Vector3.zero)
        {
            Vector3 inputVel = vel.normalized * m_Speed;
            newVel.x = inputVel.x;
            newVel.z = inputVel.z;
        }
        else
        {
            newVel.x *= m_Damping;
            newVel.z *= m_Damping;
        }

        rb.velocity = new Vector3(newVel.x, currentVel.y, newVel.z);
    }
}
