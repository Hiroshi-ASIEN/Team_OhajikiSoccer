using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TurnManager;


public class snap : MonoBehaviour
{
    //[SerializeField] TurnManager m_TurnManager; //ターン制スクリプト

    GameObject m_Player = null; //クリックしたプレイヤー保存用

    //RayCast用
    Plane m_GroundPlane;
    float m_RayDistance;
    Ray   m_Ray;

    Vector3 m_DownPosition3D;   //マウスの左ボタン押した場所
    Vector3 m_UpPosition3D;     //マウスの左ボタンを離した場所

    public float m_Thrust = 3f;

    //LineRenderer用
    LineRenderer m_LineRenderer;       //プレイヤーが持っているラインレンダラー保存用
    public int   m_LinePoint = 2;      //Lineの端点の数
    public float m_LineWidth = 1.0f;   //Lineの幅
    Vector3      m_NowPosition3D;      //マウスの現在地保存用
    bool         m_IsDragging = false; //マウスをドラッグしているか

    bool m_IsBurstTime = false;

    // Start is called before the first frame update
    void Start()
    {
        m_GroundPlane = new Plane(Vector3.up, 0f);
        TurnManager.Instance.OnTurnChanged += SetBurstTimeSnap; // ターンマネージャーのターン切り替えイベントにバーストフラグ関数設定
    }

    // Update is called once per frame
    void Update()
    {
        // 左クリックを押した時
        if (Input.GetMouseButtonDown(0))
        {
            m_DownPosition3D = GetCursorPosition3D();
            GetObj();           //クリックしたオブジェクト(Player)取得
            SetLineRenderer();  //PlayerのLineRenderer取得
            m_IsDragging = true;
        }
        // 左クリックを離した時
        else if (Input.GetMouseButtonUp(0))
        {
            m_IsDragging = false;
            m_UpPosition3D = GetCursorPosition3D();

            if (m_DownPosition3D != m_Ray.origin && m_UpPosition3D != m_Ray.origin)
            {
                //クリックしてPlayerが取得出来ているのなら
                if(m_Player)
                {
                    //引っ張ったオブジェクトの移動ベクトルを保存
                    PlayerMove script = m_Player.GetComponent<PlayerMove>();
                    script.m_Velocity = (m_DownPosition3D - m_UpPosition3D) * m_Thrust;

                    //リアルタイム進行になった時の処理
                    if(m_IsBurstTime)
                    {
                        script.Move();
                    }
                }
            }
            m_Player = null;
        }
        if (m_IsDragging)
        {
            DrawArrow();
        }

    }

    //マウスの現在位置を取得する
    Vector3 GetCursorPosition3D()
    {
        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition); // マウスカーソルから、カメラが向く方へのレイ
        m_GroundPlane.Raycast(m_Ray, out m_RayDistance); // レイを飛ばす
        return m_Ray.GetPoint(m_RayDistance); // Planeとレイがぶつかった点の座標を返す
    }

    //クリックしたプレイヤーを取得
    void GetObj()
    {
        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition); // マウスカーソルから、カメラが向く方へのレイ

        //レイヤーマスクでPlayerのみ判定
        int layerMask = LayerMask.GetMask("Player", "Ghost");
        bool isHit = Physics.Raycast(m_Ray, out RaycastHit hit,Mathf.Infinity,layerMask);

        if (!isHit)
        {
            Debug.Log("クリック失敗");
            return;
        }

        if(hit.collider.gameObject.CompareTag("Player"))
        {
            m_Player = hit.collider.gameObject;
            Debug.Log("クリック成功");
        }
        else
        {
            Debug.Log(hit.collider.name);
        }
    }

    //矢印表示
    void DrawArrow()
    {
        m_NowPosition3D = GetCursorPosition3D();
        Vector3 dist = (m_NowPosition3D - m_DownPosition3D);
        m_LineRenderer.SetPosition(0, m_DownPosition3D + dist);
        m_LineRenderer.SetPosition(1, m_DownPosition3D - dist);
    }

    //PlayerのLineRendererを取得
    void SetLineRenderer()
    {
        m_LineRenderer = m_Player.GetComponent<LineRenderer>();
        m_LineRenderer.startWidth = m_LineWidth;
        m_LineRenderer.endWidth = m_LineWidth;
        m_LineRenderer.positionCount = m_LinePoint;
        m_LineRenderer.enabled = true;
    }

    // ターンマネージャーのイベントに設定
    // バーストタイムに突入したらsnap直後にプレイヤー移動フラグをtrueにする
    private void SetBurstTimeSnap(TURN_STATE _state)
    {
        if (_state == TURN_STATE.BURST_TURN)
        {
            m_IsBurstTime = true;
        }
        else
        {
            m_IsBurstTime = false;
        }
    }
}
