using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�Q�l�T�C�g
//https://bluebirdofoz.hatenablog.com/entry/2023/09/12/230456

[RequireComponent(typeof(MousePositionGetter))]
[RequireComponent(typeof(CircleCollider2D))]

public class CursorObjeTest : MonoBehaviour
{
    //�}�E�X�ʒu���擾�Ă��̏ꏊ��UI�̂�\��
    
    //UI�ʒu�̕ύX
    //https://qiita.com/Maru60014236/items/0e3eb6c60307fa083117

    MousePositionGetter m_mousePosGetter=null;
    /*[SerializeField] */RectTransform m_rectTransform = null;


    /// <summary>
    /// �}�E�X�|�C���^�[�𓊉e����Canvas�R���|�[�l���g�̎Q��
    /// </summary>
    [SerializeField] private Canvas m_canvas;

    /// <summary>
    /// �}�E�X�|�C���^�[�𓊉e����Canvas��RectTransform�R���|�[�l���g�̎Q��
    /// </summary>
    [SerializeField] private RectTransform m_canvasTransform;


    bool m_isPushing = false;
    bool m_hasSendInfo = false;

    //=================================================================
    //�ȉ��͒͂ޏ���
    //=================================================================
    private MouseCatchableObje m_CatchableObje;//�͂�ł������
    RectTransform m_objTransform;




    void Start()
    {
        m_mousePosGetter = GetComponent<MousePositionGetter>();
        m_rectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        // Canvas��RectTransform���ɂ���}�E�X�̍��W�����[�J�����W�ɕϊ�����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_canvasTransform,
            Input.mousePosition,
            m_canvas.worldCamera,
            out var mousePosition);

        // �|�C���^�[���}�E�X�̍��W�Ɉړ�����
        m_rectTransform.anchoredPosition = new Vector2(mousePosition.x, mousePosition.y);






        //�Ǐ]����
        if(m_CatchableObje)
        {
            Debug.Log("catching");

            m_objTransform.localPosition = m_rectTransform.localPosition;


            //������������Ȃ��ˁH�܁A������

            if (Input.GetMouseButtonUp(0))
            {
                ReleaseObje();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_isPushing = true;
            m_hasSendInfo = false;
        }
        else if(m_hasSendInfo)
        {
            m_isPushing = false;
        }



    }

    private void FixedUpdate()
    {
        if (m_isPushing)
        {
            m_hasSendInfo = true;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {

        //�͂ޏ����@���͂����
        if(m_isPushing)
        {

            if (collision.gameObject.TryGetComponent<MouseCatchableObje>(out var obj))
            {

                Debug.Log("collided");
                CatchObje(obj);
            }


            if (collision.gameObject.TryGetComponent<Item2DObject>(out var item2D))
            {
                item2D.UseItem();
            }
        }


        //if (jumping)
        //else
        //    GetComponent<Renderer>().material.color = Color.green;

    }

    void CatchObje(MouseCatchableObje obj)
    {
        if (m_CatchableObje)
            return;

        if(obj.gameObject.TryGetComponent<RectTransform>(out var tf))
        {
            m_objTransform = tf;
        }

        m_CatchableObje = obj;
    }
    void ReleaseObje()
    {
        m_CatchableObje = null;
    }


}
