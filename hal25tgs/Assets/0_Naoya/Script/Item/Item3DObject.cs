using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// アイテムの3D情報クラス
/// </summary>
//----------------------------------------------------------------

//---------------------
//要件
//
//あたり判定とモデル管理を行う
//---------------------

//必須コンポーネント
[RequireComponent(typeof(SphereCollider))]      //球コライダー

public class Item3DObject : MonoBehaviour
{
    //============================================================
    // 変数
    //============================================================

    //------------------
    // プライベート
    //------------------
    private SphereCollider m_sphereCollider = null;     //球コライダー
    private bool m_isCollided = false;                  //触れたかどうか

    //触れたプレイヤー
    private PlayerObject m_playerObject = null;
    private EnemyAI m_enemyObject = null;

    //============================================================
    // 関数
    //============================================================
    void Reset()
    {
        // 必要なコンポーネントの参照を取得する
        if (!m_sphereCollider)
            m_sphereCollider = this.GetComponent<SphereCollider>();

        m_sphereCollider.isTrigger = true;
    }
    void Start()
    {
        // 必要なコンポーネントの参照を取得する
        if (!m_sphereCollider)
            m_sphereCollider = this.GetComponent<SphereCollider>();

        m_sphereCollider.isTrigger = true;
    }


    //あたり判定
    private void OnTriggerEnter(Collider other)
    {
        ////触れたあいてがプレイヤーならば、触れた、にする
        //if(other.gameObject.CompareTag("Player"))
        //{
        //    m_isCollided = true;

        //    //触れた相手を保存したいな。同フレームに触れた場合、アイテムが2個にならないように。未実装

        //}

        if (m_isCollided)
            return;

        //格納！
        if (other.gameObject.TryGetComponent<PlayerObject>(out var player))
        {
            if (m_playerObject == null)
                m_playerObject = player;

            m_isCollided = true;

            Debug.Log("collide player");
        }

        //格納！
        else if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
        {
            if (m_enemyObject == null)
                m_enemyObject = enemy;

            m_isCollided = true;

            Debug.Log("collide enemyAI");
        }
    }

    //触れたかどうかを送信する（Item側から呼ぶ）
    public bool GetIsCollided()
    {
        return m_isCollided;
    }


    //触れたかプレイヤーを送信する（Item側から呼ぶ）
    public PlayerObject GetCollidedPlayer()
    {
        if (m_playerObject)
            return m_playerObject;
        else 
            return null;
    }
    public EnemyAI GetCollidedEnemy()
    {
        return m_enemyObject;
    }




}
