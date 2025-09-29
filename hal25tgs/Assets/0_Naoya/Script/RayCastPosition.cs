using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastPosition : MonoBehaviour
{
    [Header("判定'する'レイヤー")]
    [SerializeField] private LayerMask m_layerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //カメラからマウスがある場所に向かってRayを発射
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
        {
            //  Rayが当たったところにカーソルを動かす
            transform.position = hit.point;

            //これ、自身にコライダーをつけていると、自分にRayが当たって、段々とカメラに近づいてしまう！
        }
    }
}
