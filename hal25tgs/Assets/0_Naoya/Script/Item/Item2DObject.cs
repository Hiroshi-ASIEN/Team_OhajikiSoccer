using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]

public class Item2DObject : MonoBehaviour
{
    //自身のRectTransform
    RectTransform m_rectTransform = null;

    bool m_isUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        m_rectTransform = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //とりあえずデバッグで使う
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    UseItem();
        //}
    }


    public RectTransform GetRectTransform() { 

        if (m_rectTransform == null)
            m_rectTransform = gameObject.GetComponent<RectTransform>();

        return m_rectTransform; 
    }

    //使用される
    public void UseItem() {
        if (m_isUsed)
            return;


        Debug.Log("アイテム使用を試みました");
        m_isUsed = true;
    }

    //使用済みか
    public bool GetIsUsed()
    { return m_isUsed; }


    //使用に失敗したら
    public void UseFailed()
    {
        m_isUsed=false;
    }


}
