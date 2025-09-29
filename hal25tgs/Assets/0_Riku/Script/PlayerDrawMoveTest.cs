using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerDrawMoveTest : MonoBehaviourPun
{
    public float forceMultiplier = 10f;
    private Rigidbody rb;
    private LineRenderer line;

    private Vector3 dragStartPos = Vector3.zero;
    private Vector3 queuedForce = Vector3.zero;

    private double startTime;
    public float timer = 0f;
    private float interval = 10f;

    private bool forceReady = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;

        if (PhotonNetwork.IsMasterClient)
        {
            startTime = PhotonNetwork.Time;
            photonView.RPC("SetStartTime", RpcTarget.AllBuffered, startTime);
        }
    }

    void Update()
    {
        timer = (float)(PhotonNetwork.Time - startTime); // ← これでサーバー基準の timer に


        if (timer >= interval)
        {
            timer = 0f;
            startTime = PhotonNetwork.Time; // リセット

            if (forceReady && queuedForce != Vector3.zero)
            {
                // 矢印は移動開始時に消す
                line.enabled = false;

                rb.AddForce(queuedForce, ForceMode.Impulse);
                queuedForce = Vector3.zero;
                forceReady = false;
            }

            photonView.RPC("SetStartTime", RpcTarget.AllBuffered, startTime);
        }
    }

    void OnMouseDown()
    {
        dragStartPos = GetMouseWorldPos();
        rb.velocity = Vector3.zero;
        line.enabled = true;
    }

    void OnMouseDrag()
    {
        Vector3 currentMousePos = GetMouseWorldPos();
        Vector3 dragVector = currentMousePos - dragStartPos;
        dragVector.y = 0;

        line.SetPosition(0, transform.position);
        Vector3 predictedEnd = transform.position - dragVector.normalized * 2f;
        predictedEnd.y = transform.position.y;
        line.SetPosition(1, predictedEnd);
    }

    void OnMouseUp()
    {
        Vector3 dragVector = GetMouseWorldPos() - dragStartPos;
        dragVector.y = 0;

        Vector3 force = -dragVector * forceMultiplier * rb.mass;
        force.y = 0;

        if (force.magnitude > 0.01f)
        {
            queuedForce = force;
            forceReady = true;
            line.enabled = true; // 矢印は移動開始まで表示
        }
        else
        {
            queuedForce = Vector3.zero;
            forceReady = false;
            line.enabled = false;
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }

    void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    [PunRPC]
    void SetStartTime(double masterTime)
    {
        startTime = masterTime;
    }
}
