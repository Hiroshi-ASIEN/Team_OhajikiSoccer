using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryPosAI
{
    None = -1,
    Left,
    Right,
}


public class ItemInventoryAI : MonoBehaviourPunCallbacks
{
    //保有しているアイテム　リスト
    private List<ItemObject> m_itemList = new List<ItemObject>();

    //保有しているチーム
    private TeamObject m_team = null;

    //キャンバス
    [Header("キャンバスのプレハブたち")]
    [SerializeField] private GameObject m_canvasPrefabLeft;
    [SerializeField] private GameObject m_canvasPrefabRight;

    [Header("キャンバスのインスタンス")]
   /* [SerializeField] */private GameObject m_canvas;
    Vector2 m_offset = new Vector2(0, 0);

    //自分の画面で扱っているチームかどうか
    private bool m_isOwner = false;


    //今は雑に左上から順にやってこう
    [Header("アイテムの最大保有数")]
    /*[SerializeField]*/ private int m_itemMax = 3;
    //[Header("アイテムを新規で入手した際、上書きするかどうか")]
    //[SerializeField] private bool m_isOverWriting = false;

    //[Header("アイテム枠（Canvas内のインスタンス）")]
    //[SerializeField]
    private List<Image> m_itemFramesList;



    //現在使用中のアイテムのステート
    ITEM_STATE m_state = ITEM_STATE.JUDGING;
    //現在使用中のアイテムのインスタンス
    ItemObject m_item;
    //現在使用中のアイテムのインデックス
    int m_itemIndex;


    void Start()
    {



    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //ターンの状態によって色を変えるか　毎フレームの代入は嫌だなぁ
        //m_itemFramesList





        //アイテムの結果待ち
        if (m_state ==ITEM_STATE.RUNNING)
        {
            //結果を取得する
            if (m_item)
                m_state = m_item.GetItemState();


            if (m_state == ITEM_STATE.SUCCESS)
            {
                //削除
                RemoveAndDestroyItem(m_item);

                //もう一つのPC側でもアイテムを発動する
                if (m_team)
                {
                    m_team.GetGameManager().SendItemUsingToOtherTeam(m_team, m_itemIndex);
                }
                else
                    Debug.LogWarning("インベントリにチームが登録されていません");
            }
            else if(m_state==ITEM_STATE.FAILURE)
            {
                //失敗
            }
        }
    }


    public void SetInventoryPositionAndInit(InventoryPos inventoryPos)
    {
        if (inventoryPos == InventoryPos.Left)
        {
            Debug.Log("アイテムインベントリ（左）のキャンバス生成を試みました");

            if (m_canvasPrefabLeft)
                m_canvas = Instantiate(m_canvasPrefabLeft);
        }
        else if(inventoryPos == InventoryPos.Right)
        {
            Debug.Log("アイテムインベントリ（右）のキャンバス生成を試みました");

            if (m_canvasPrefabRight)
                m_canvas = Instantiate(m_canvasPrefabRight);
        }


        if (!m_canvas)
            return;

        m_itemFramesList = new List<Image>();

        //キャンバスからImageを探して格納（ひどいコードになってしまったな）
        for (int i = 0; i < m_canvas.transform.childCount; i++)
        {
            if (m_canvas.transform.GetChild(i).gameObject.TryGetComponent<Image>(out var image))
            {
                m_itemFramesList.Add(image);
            }
        }


        //m_itemFramesList.Add;

        //アイテム枠の数で最大保有数を決めよう
        m_itemMax = m_itemFramesList.Count;

        //キャンバスを子オブジェクトにする
        m_canvas.transform.SetParent(this.gameObject.transform, false);
    }


    /// <summary>
    /// アイテムを追加する（チームから呼び出される）
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemObject item)
    {
        //超過するかチェック
        if (m_itemList.Count >= m_itemMax)
            return;

        if (!m_canvas)
            return;

        //データを変更して
        m_itemList.Add(item);
        //キャンバスに移動させる
        item.gameObject.transform.SetParent(m_canvas.transform);
        //　ヒエラルキーで一番下に移動し、前面に表示される
        item.gameObject.transform.SetAsLastSibling();
        item.gameObject.transform.position = new Vector3(960, 540, 0);//これ、キャンバスの真ん中においているって意味か。

        //インデックスを決めて
        int index = m_itemList.Count - 1;
        //整列
        SortByFramePos(index);


        //ここでアイテムに対してチームを登録する　触れた時だけでもいいか？
        //item.SetInventory(this);
    }


    //今現在Removeだけど、Useをこっちにしても良い　ていうか多分こっちですることになる


    /// <summary>
    /// アイテム使用　（マスター側のアイテムオブジェクトから呼ばれる）←これ、マスタークライアントじゃなくアイテム使用者？
    /// </summary>
    /// <param name="item"></param>
    public ITEM_STATE UseItemMaster(ItemObject item)
    {
        //ターンマネージャーから、現在の状態を取得
        if (TurnManager.Instance.GetCurrentTurnState() != TurnManager.TURN_STATE.PLAY_TURN)
        {
            Debug.Log("操作ターン時のみアイテムを使用できます。");
            return ITEM_STATE.FAILURE;
        }




        //現在使用中ならだめ
        if (m_state == ITEM_STATE.RUNNING)
            return ITEM_STATE.RUNNING;



        //使用できるか
        bool isUsable = CheckIsUsableItem(item);

        if (isUsable==false)
            return ITEM_STATE.FAILURE;

        //番号取得
        m_itemIndex = m_itemList.IndexOf(item);
        if (m_itemIndex < 0)
            return ITEM_STATE.FAILURE;

        m_item = item;

        //実行を試みる
        m_state = TryToExecuteItem(m_item);

        if(m_state == ITEM_STATE.SUCCESS)
        {
            //削除
            RemoveAndDestroyItem(m_item);

            //もう一つのPC側でもアイテムを発動する
            if (m_team)
            {
                m_team.GetGameManager().SendItemUsingToOtherTeam(m_team, m_itemIndex);
                return ITEM_STATE.SUCCESS;
            }
            else
                Debug.LogWarning("インベントリにチームが登録されていません");


            return ITEM_STATE.FAILURE;
        }
        else
        {
            return m_state;
        }
    }

    public void UseItemUnmaster(int _itemIndex)//非マスター側で呼ばれる
    {
        //指定の番号のアイテムを使用する
        Debug.Log("相手画面にてアイテムを使用されました！");

        ItemObject item = m_itemList[_itemIndex];


        //※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
        //これ、ここで発動する意味あるか？
        //もしもItemのEffect内ですでにPhotonの同期（例えばエフェクト生成、オブジェクト生成、ボール変型）
        //が実装されていたら、二回発動すると逆によくないな！
        //
        //だから、本当に必要なのはもしかしたらインベントリの中身を同期させる仕組みなのかもしれないな
        //別の画面に存在するインベントリをどう同期させようかなー
        //情報をコピーできないかな。今はやめておこう。問題が出てから対処しましょう。

        //ExecuteItem(item);
        RemoveAndDestroyItem(item);

        //※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※


    }


    private ITEM_STATE TryToExecuteItem(ItemObject _item)
    {
        if (!_item)
        {
            //のちに、アイテムが存在せずとも強制的に使用してもいいかも？静的関数かなぁ
            Debug.Log("使用しようとしたアイテムが存在しません！！");
            return ITEM_STATE.FAILURE;
        }

        //効果発動　（試みるところと、実際にできた所、あとで分けないとなぁ）
        return _item.Effect();

    }




    public void RemoveAndDestroyItem(ItemObject item) {  
        
        //参照削除
        m_item = null;

        m_itemList.Remove(item);
        Destroy(item.gameObject);

        //整列
        SortByFrameEveryItem();
    }

    //左詰めする
    private void SortCloser(int _index)
    {
        ItemObject item = m_itemList[_index];

        if (!item)
            return;

        //ビジュアルを変更する（位置かな）
        RectTransform rect = item.Get2DobjRectTransform();
        rect.transform.localPosition = Vector3.zero;

        //保有している数に合わせて...まぁ、100としとくか？
        Vector2 pos = new Vector2(100 * _index, 0);

        rect.transform.localPosition = (pos + m_offset);
    }

    //全てを整列
    private void SortCloserEveryItem()
    {
        for (int i = 0; i < m_itemList.Count; i++)
            SortCloser(i);
    }


    private void SortByFramePos(int _index)
    {
        ItemObject item = m_itemList[_index];

        if (!item)
            return;

        ////ビジュアルを変更する（位置かな）
        //RectTransform rect = item.Get2DobjRectTransform();
        //rect.transform.localPosition = Vector3.zero;

        //Debug.Log(rect);
        //Debug.Log(m_itemFramesList[_index]);

        //rect.transform.localPosition = m_itemFramesList[_index].transform.localPosition;



        //枠の子供に移動させる
        item.gameObject.transform.SetParent(m_itemFramesList[_index].transform);
        item.gameObject.transform.localPosition = Vector3.zero;

    }

    private void SortByFrameEveryItem()
    {
        for (int i = 0; i < m_itemList.Count; i++)
            SortByFramePos(i);
    }


    private void DisplayInfomation()
    {
        Debug.Log(m_itemList.Count);

        for (int i=0;i<m_itemList.Count;i++)
        {
            Debug.Log(m_itemList[i]);
        }
    }


    /// <summary>
    /// チーム登録（チームオブジェクトから呼ばれる）
    /// </summary>
    /// <param name="_team"></param>
    public void SetTeam(TeamObject _team)
    {
        m_team = _team;
    }


    private bool CheckIsUsableItem(ItemObject _item)
    {
        //ここで判定したい


        //
        return true;
    }


    //インベントリの同期をさせる　しかし！これも注意が必要だな。
    //基本的にアイテムもプレイヤーも同期しているから、取得に関しては
    //放っておいても基本同期するはず。
    //なんなら取得を人力で同期しようとすると、今度は取得されたアイテムも
    //同期をとらないといけなくなって...余計ややこしくなる気がするゾイ

    //今は、使用時のみの同期にすませておこう！！！





}
