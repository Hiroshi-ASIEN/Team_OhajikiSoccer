using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GeneratePrefabPhoton))]

//======================================================================
// 05/19まで　むらたつ制作
// 05/20から　かわかみ制作
//
// アイテムを同期で生成
// 生成したアイテムを管理するかは未定
// できれば放っておきたいけどなぁ
//======================================================================


public class ItemManager : MonoBehaviourPunCallbacks
{
    //アイテム
    //案① 現在はこっち
    /*
      リストにアイテムのプレハブを設定
      リストの中のアイテムをランダムに生成
     */

    //案②
    /*
      マリカーのように1つのプレハブで、
      取得したらランダムでアイテム取得
    */
    [SerializeField] List<GameObject> m_ItemPrefabList;

    //アイテム生成場所と頻度
    /*
      場所：アイテムの生成位置は複数用意しておいて、
      　　　どこかの場所からランダムで出現
    　頻度：3ターンに1個生成
    */
    [SerializeField] List<GameObject> m_TargetPoint;


    [SerializeField]
    [HideInInspector]
    GeneratePrefabPhoton m_generatePrefabPhoton;


    void Start()
    {
        m_generatePrefabPhoton = GetComponent<GeneratePrefabPhoton>();
    }

    void Update()
    {
        //とりあえずデバッグで出す
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GenerateItem();

            //PhotonNetwork.Instantiate(m_ItemPrefabList[0].name, m_TargetPoint[0].transform.position, Quaternion.identity);
            Debug.Log("アイテム生成を試みました");
        }
    }


    /// <summary>
    /// これをゲームマネージャー側で呼ぶのかな？それともこっちだけで完結かな？
    /// 完結はできないだろうな。非マスターでこれを呼んじゃいけないから
    /// </summary>
    public void GenerateItem()
    {
        if(m_ItemPrefabList.Count <=0)
        {
            Debug.Log("アイテムのプレハブが登録されておらず、生成できません");
            return;
        }


        //まずは番号をランダムで決めて...
        int index =
            UnityEngine.Random.Range(0, m_ItemPrefabList.Count);


        //ゲームオブジェクトを決定
        GameObject prefab = m_ItemPrefabList[index];


        //それを場所で生成する。自身の体積がいいなぁ
        float leftX;
        float rightX;
        leftX = this.transform.position.x - this.transform.lossyScale.x * 0.5f;
        rightX = this.transform.position.x + this.transform.lossyScale.x * 0.5f;

        float topY;
        float bottomY;
        topY = this.transform.position.y - this.transform.lossyScale.y * 0.5f;
        bottomY = this.transform.position.y + this.transform.lossyScale.y * 0.5f;

        float backZ;
        float frontZ;
        backZ = this.transform.position.z - this.transform.lossyScale.z * 0.5f;
        frontZ = this.transform.position.z + this.transform.lossyScale.z * 0.5f;

        Vector3 min;
        min = new Vector3(leftX, topY, backZ);
        Vector3 max;
        max = new Vector3(rightX, bottomY, frontZ);

        if (m_generatePrefabPhoton)
            m_generatePrefabPhoton.GeneratePrefabPhotonByBoxVolume(prefab, min, max);
    }






}
