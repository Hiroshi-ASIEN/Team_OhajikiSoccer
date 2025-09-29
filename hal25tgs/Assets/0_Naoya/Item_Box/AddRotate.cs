using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRotate : MonoBehaviour
{
    [SerializeField]
    float delta = 0.075f;
    float a = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        a += delta;

        if (a > 360)
            a -= 360;

        this.gameObject.transform.rotation = Quaternion.Euler(0f, a, 0f);
    }

}
