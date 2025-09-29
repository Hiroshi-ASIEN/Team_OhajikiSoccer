using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] GameObject m_HitVFX;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AI"))
        {
            SingleMultiUtility.InstantiateForClient(m_HitVFX.name, this.transform.position, Quaternion.identity);
        }
    }
}
