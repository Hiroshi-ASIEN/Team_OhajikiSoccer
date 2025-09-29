using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class snap : MonoBehaviour
{
    Plane groundPlane;
    Vector3 downPosition3D;
    Vector3 upPosition3D;

    //public GameObject player;
    public float thrust = 3f;

    float rayDistance;
    Ray ray;

    GameObject player0 = null;

    // Start is called before the first frame update
    void Start()
    {
        groundPlane = new Plane(Vector3.up, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリックを押した時
        {
            downPosition3D = GetCursorPosition3D();
            GetObj();//クリックしたプレイヤー取得
        }
        else if (Input.GetMouseButtonUp(0)) // 左クリックを離した時
        {
            upPosition3D = GetCursorPosition3D();

            if (downPosition3D != ray.origin && upPosition3D != ray.origin)
            {
                if(player0)
                {
                    //引っ張ったオブジェクトの移動ベクトルを保存
                    PlayerMove script = player0.GetComponent<PlayerMove>();
                    script.velocity = (downPosition3D - upPosition3D) * thrust;

                    //player0.GetComponent<Rigidbody>().AddForce((downPosition3D - upPosition3D) * thrust, ForceMode.Impulse); // ボールをはじく
                }
            }
        }
    }

    Vector3 GetCursorPosition3D()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // マウスカーソルから、カメラが向く方へのレイ
        groundPlane.Raycast(ray, out rayDistance); // レイを飛ばす

        return ray.GetPoint(rayDistance); // Planeとレイがぶつかった点の座標を返す

    }

    //クリックしたプレイヤーを取得
    void GetObj()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // マウスカーソルから、カメラが向く方へのレイ
        bool isHit = Physics.Raycast(ray,out RaycastHit hit);

        if (!isHit)
        {
            Debug.Log("クリック失敗");
            return;
        }

        if(hit.collider.gameObject.CompareTag("Player"))
        {
            player0 = hit.collider.gameObject;
            Debug.Log("クリック成功");
        }
        else
        {
            Debug.Log("相手");
            //player0 = null;
        }
    }
}
