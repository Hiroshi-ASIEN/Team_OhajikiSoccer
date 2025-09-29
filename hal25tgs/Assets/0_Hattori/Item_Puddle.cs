using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Puddle : MonoBehaviour
{
    public float speedMultiplier = 0.45f; // ���x�𗎂Ƃ�
    public float slowDuration = 1f; // ���x�𗎂Ƃ�����

    // ���蔲�����瑬�xdown
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
        Debug.Log("�ʉ�"); // �ʉ߂����烍�O���o��
    }

    // ���xDown����
    private System.Collections.IEnumerator SlowDown(Rigidbody rb)
    {
        Vector3 originalVelocity = rb.velocity;

        rb.velocity *= speedMultiplier;

        yield return new WaitForSeconds(slowDuration);
    }
}
