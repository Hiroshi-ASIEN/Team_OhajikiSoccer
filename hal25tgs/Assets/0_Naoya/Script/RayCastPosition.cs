using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastPosition : MonoBehaviour
{
    [Header("����'����'���C���[")]
    [SerializeField] private LayerMask m_layerMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�J��������}�E�X������ꏊ�Ɍ�������Ray�𔭎�
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
        {
            //  Ray�����������Ƃ���ɃJ�[�\���𓮂���
            transform.position = hit.point;

            //����A���g�ɃR���C�_�[�����Ă���ƁA������Ray���������āA�i�X�ƃJ�����ɋ߂Â��Ă��܂��I
        }
    }
}
