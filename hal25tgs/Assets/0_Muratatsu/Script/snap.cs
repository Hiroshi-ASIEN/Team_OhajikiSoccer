using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TurnManager;


public class snap : MonoBehaviour
{
    //[SerializeField] TurnManager m_TurnManager; //�^�[�����X�N���v�g

    GameObject m_Player = null; //�N���b�N�����v���C���[�ۑ��p

    //RayCast�p
    Plane m_GroundPlane;
    float m_RayDistance;
    Ray   m_Ray;

    Vector3 m_DownPosition3D;   //�}�E�X�̍��{�^���������ꏊ
    Vector3 m_UpPosition3D;     //�}�E�X�̍��{�^���𗣂����ꏊ

    public float m_Thrust = 3f;

    //LineRenderer�p
    LineRenderer m_LineRenderer;       //�v���C���[�������Ă��郉�C�������_���[�ۑ��p
    public int   m_LinePoint = 2;      //Line�̒[�_�̐�
    public float m_LineWidth = 1.0f;   //Line�̕�
    Vector3      m_NowPosition3D;      //�}�E�X�̌��ݒn�ۑ��p
    bool         m_IsDragging = false; //�}�E�X���h���b�O���Ă��邩

    bool m_IsBurstTime = false;

    // Start is called before the first frame update
    void Start()
    {
        m_GroundPlane = new Plane(Vector3.up, 0f);
        TurnManager.Instance.OnTurnChanged += SetBurstTimeSnap; // �^�[���}�l�[�W���[�̃^�[���؂�ւ��C�x���g�Ƀo�[�X�g�t���O�֐��ݒ�
    }

    // Update is called once per frame
    void Update()
    {
        // ���N���b�N����������
        if (Input.GetMouseButtonDown(0))
        {
            m_DownPosition3D = GetCursorPosition3D();
            GetObj();           //�N���b�N�����I�u�W�F�N�g(Player)�擾
            SetLineRenderer();  //Player��LineRenderer�擾
            m_IsDragging = true;
        }
        // ���N���b�N�𗣂�����
        else if (Input.GetMouseButtonUp(0))
        {
            m_IsDragging = false;
            m_UpPosition3D = GetCursorPosition3D();

            if (m_DownPosition3D != m_Ray.origin && m_UpPosition3D != m_Ray.origin)
            {
                //�N���b�N����Player���擾�o���Ă���̂Ȃ�
                if(m_Player)
                {
                    //�����������I�u�W�F�N�g�̈ړ��x�N�g����ۑ�
                    PlayerMove script = m_Player.GetComponent<PlayerMove>();
                    script.m_Velocity = (m_DownPosition3D - m_UpPosition3D) * m_Thrust;

                    //���A���^�C���i�s�ɂȂ������̏���
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

    //�}�E�X�̌��݈ʒu���擾����
    Vector3 GetCursorPosition3D()
    {
        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition); // �}�E�X�J�[�\������A�J�������������ւ̃��C
        m_GroundPlane.Raycast(m_Ray, out m_RayDistance); // ���C���΂�
        return m_Ray.GetPoint(m_RayDistance); // Plane�ƃ��C���Ԃ������_�̍��W��Ԃ�
    }

    //�N���b�N�����v���C���[���擾
    void GetObj()
    {
        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition); // �}�E�X�J�[�\������A�J�������������ւ̃��C

        //���C���[�}�X�N��Player�̂ݔ���
        int layerMask = LayerMask.GetMask("Player", "Ghost");
        bool isHit = Physics.Raycast(m_Ray, out RaycastHit hit,Mathf.Infinity,layerMask);

        if (!isHit)
        {
            Debug.Log("�N���b�N���s");
            return;
        }

        if(hit.collider.gameObject.CompareTag("Player"))
        {
            m_Player = hit.collider.gameObject;
            Debug.Log("�N���b�N����");
        }
        else
        {
            Debug.Log(hit.collider.name);
        }
    }

    //���\��
    void DrawArrow()
    {
        m_NowPosition3D = GetCursorPosition3D();
        Vector3 dist = (m_NowPosition3D - m_DownPosition3D);
        m_LineRenderer.SetPosition(0, m_DownPosition3D + dist);
        m_LineRenderer.SetPosition(1, m_DownPosition3D - dist);
    }

    //Player��LineRenderer���擾
    void SetLineRenderer()
    {
        m_LineRenderer = m_Player.GetComponent<LineRenderer>();
        m_LineRenderer.startWidth = m_LineWidth;
        m_LineRenderer.endWidth = m_LineWidth;
        m_LineRenderer.positionCount = m_LinePoint;
        m_LineRenderer.enabled = true;
    }

    // �^�[���}�l�[�W���[�̃C�x���g�ɐݒ�
    // �o�[�X�g�^�C���ɓ˓�������snap����Ƀv���C���[�ړ��t���O��true�ɂ���
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
