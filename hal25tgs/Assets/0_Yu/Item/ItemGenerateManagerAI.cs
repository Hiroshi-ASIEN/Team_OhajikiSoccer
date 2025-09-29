using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//======================================================================
// アイテム生成器
// 
// ヒエラルキーに常駐させ、ターンマネージャーに登録させておきたい。
// ターンマネージャーから毎ターン AddTurn()関数を呼ぶことで動作する。   ターンの状態に注意！
// もしも依存関係を逆転させたいのであれば、
// ターンマネージャー側に、累計ターン数と、ターンが経過したタイミングを
// 取得できる関数を用意して欲しい。
//
//
// ↑ターンが変化したときに実行されるイベントが、ターンマネージャーにあるらしいので、そこで！
//======================================================================

public class ItemGenerateManagerAI : MonoBehaviourPunCallbacks
{
    //累計ターン数
    private int m_totalTurnNum = 1;

    [Header("何ターンに一度生成するか")]
    [SerializeField] private int m_generateTurn = 3;

    [Header("生成位置オブジェクト")]
    [SerializeField] private GameObject m_spawnPosObjectA;
    [SerializeField] private GameObject m_spawnPosObjectB;

    [Header("生成アイテム一覧（Resorceファイルにあること）")]
    [SerializeField] List<GameObject> m_ItemPrefabList = new List<GameObject>();//アイテムボックスのプレハブたち（これ、見た目を統一すればいいのか！）

    
    //生成したアイテムの参照（とられたかどうかを判定する）
    private ItemObject m_itemObjectUp = null;
    private ItemObject m_itemObjectDown = null;

    //もしも3ターン経ってもとられなかったら、反対側？かな？そのままでもいい。
    //じゃあ、取られたかどうかを知っていないといけないな。　今はいいや


    private void Awake()
    {
        TurnManager.Instance.OnTurnChanged += AddTurn;
    }

    private void OnDestroy()
    {
        TurnManager.Instance.OnTurnChanged -= AddTurn;
    }

    private void FixedUpdate()
    {
        //取得されたかチェックする

        if(m_itemObjectUp)
        {
            if(m_itemObjectUp.GetIsGot())
            {
                m_itemObjectUp = null;
            }
        }
        if (m_itemObjectDown)
        {
            if (m_itemObjectDown.GetIsGot())
            {
                m_itemObjectDown = null;
            }
        }
    }


    //ターンが経過したよって教える（TurnManagerから使用）
    public void AddTurn(TurnManager.TURN_STATE _state)
    {
        if (_state != TurnManager.TURN_STATE.PLAY_TURN)
        {
            Debug.Log(_state);
            Debug.Log("ターン状態がPLAY_TURNではないのでアイテム生成を行いません");
            return;
        }

        //そもそも非マスターでは呼ばない
        if (PhotonNetwork.IsMasterClient == false)
        {
            Debug.LogWarning("マスタークライアントではないのでアイテム生成を行いません");
            return;
        }

        m_totalTurnNum++;

        //指定の倍数でないならばなにもしない
        if((m_totalTurnNum % m_generateTurn) != 0)
        {
            Debug.Log(m_totalTurnNum);
            Debug.Log("ターン数が指定の倍数でないのでアイテムを生成しません");
            return;
        }


        //以下生成
        GenerateItem();
    }

    private void GenerateItem()
    {
        if (m_spawnPosObjectA == null)
        {
            Debug.LogWarning("子オブジェクトがなく、アイテム生成が出来ません");
            return;
        }
        if (m_spawnPosObjectB == null)
        {
            Debug.LogWarning("子オブジェクトがなく、アイテム生成が出来ません");
            return;
        }
        if (m_ItemPrefabList.Count <= 0)
        {
            Debug.LogWarning("アイテムのプレハブが登録されておらず、生成できません");
            return;
        }
        if(m_itemObjectUp && m_itemObjectDown)
        {
            Debug.Log("アイテムがプレイヤーに取得されずに残っているので、生成しません");
            return;
        }


        //プレハブからランダムで生成
        //まずは番号をランダムで決めて...
        int index =
            UnityEngine.Random.Range(0, m_ItemPrefabList.Count + 1);

        //ゲームオブジェクトを決定
        GameObject prefab = m_ItemPrefabList[index];

        //位置を取得して
        Vector3 pos;
        int random = Random.Range(0, 2);

        //まずはあきをみないと
        if(m_itemObjectUp)
            random = 1;
        else if(m_itemObjectDown)
            random = 0;



        if (random == 0)
        {
            pos = m_spawnPosObjectA.transform.position;
            GameObject item = SingleMultiUtility.Instantiate(prefab.name, pos, Quaternion.identity);

            if(TryGetComponent<ItemObject>(out var itemObj))
            {
                m_itemObjectUp = itemObj;
            }
        }
        else
        {
            pos = m_spawnPosObjectB.transform.position;
            GameObject item = SingleMultiUtility.Instantiate(prefab.name, pos, Quaternion.identity);

            if (TryGetComponent<ItemObject>(out var itemObj))
            {
                m_itemObjectUp = itemObj;
            }
        }

        Debug.Log("アイテム生成を試みました！！！！！！！！！！！！！！！！！！！！！！");
    }
}
