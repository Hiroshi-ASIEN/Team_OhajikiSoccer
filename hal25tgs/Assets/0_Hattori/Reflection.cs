using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflection : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody Player = collision.rigidbody;
            if (Player != null)
            {
                ContactPoint contactPoint = collision.GetContact(0);

                Player.velocity = Vector3.Reflect(collision.relativeVelocity, contactPoint.normal);
            }
        }
    }
}
