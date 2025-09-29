using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Puddle : MonoBehaviour
{
    public float speedMultiplier = 0.45f; // 速度を落とす
    public float slowDuration = 1f; // 速度を落とす時間

    // すり抜けたら速度down
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")|| other.CompareTag("AI"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                StartCoroutine(SlowDown(rb));
            }
        }
        Debug.Log("通過"); // 通過したらログを出す
    }

    // 速度Down処理
    private System.Collections.IEnumerator SlowDown(Rigidbody rb)
    {
        Vector3 originalVelocity = rb.velocity;

        rb.velocity *= speedMultiplier;

        yield return new WaitForSeconds(slowDuration);
    }
}
