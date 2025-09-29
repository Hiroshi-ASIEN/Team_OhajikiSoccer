using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]

public class Item2DObject : MonoBehaviour
{
    //���g��RectTransform
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
        //�Ƃ肠�����f�o�b�O�Ŏg��
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

    //�g�p�����
    public void UseItem() {
        if (m_isUsed)
            return;


        Debug.Log("�A�C�e���g�p�����݂܂���");
        m_isUsed = true;
    }

    //�g�p�ς݂�
    public bool GetIsUsed()
    { return m_isUsed; }


    //�g�p�Ɏ��s������
    public void UseFailed()
    {
        m_isUsed=false;
    }


}
