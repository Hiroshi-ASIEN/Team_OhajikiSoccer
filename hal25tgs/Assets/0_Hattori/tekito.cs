using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class tekito : MonoBehaviour
{
    Plane groundPlane;
    Vector3 downPosition3D;
    Vector3 upPosition3D;

    //public GameObject player;
    public float thrust = 3f;
    public float playerSpeed;

    float rayDistance;
    Ray ray;

    GameObject player0 = null;

    LineRenderer lineRenderer;
    Vector3 nowPosition3D;
    bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        groundPlane = new Plane(Vector3.up, 0f);

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���N���b�N����������
        {
            lineRenderer.positionCount = 2;
            downPosition3D = GetCursorPosition3D();
            GetObj();//�N���b�N�����v���C���[�擾
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0)) // ���N���b�N�𗣂�����
        {
            isDragging = false;
            lineRenderer.positionCount = 0;
            upPosition3D = GetCursorPosition3D();

            if (downPosition3D != ray.origin && upPosition3D != ray.origin)
            {
                if (player0)
                {
                    //�����������I�u�W�F�N�g�̈ړ��x�N�g����ۑ�
                    //PlayerMove script = player0.GetComponent<PlayerMove>();
                    //script.velocity = (downPosition3D - upPosition3D) * thrust;

                    player0.GetComponent<Rigidbody>().AddForce((downPosition3D - upPosition3D) * playerSpeed * thrust, ForceMode.Impulse); // �{�[�����͂���

                }
            }
        }
        if (isDragging)
        {
            DrawArrow();
        }

    }

    Vector3 GetCursorPosition3D()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // �}�E�X�J�[�\������A�J�������������ւ̃��C
        groundPlane.Raycast(ray, out rayDistance); // ���C���΂�

        return ray.GetPoint(rayDistance); // Plane�ƃ��C���Ԃ������_�̍��W��Ԃ�

    }

    //�N���b�N�����v���C���[���擾
    void GetObj()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // �}�E�X�J�[�\������A�J�������������ւ̃��C
        bool isHit = Physics.Raycast(ray, out RaycastHit hit);

        if (!isHit)
        {
            Debug.Log("�N���b�N���s");
            return;
        }

        if (hit.collider.gameObject.CompareTag("Player"))
        {
            player0 = hit.collider.gameObject;
            Debug.Log("�N���b�N����");
        }
        else
        {
            Debug.Log(hit.collider.gameObject.name);
            player0 = null;
        }
    }

    void DrawArrow()
    {

        nowPosition3D = GetCursorPosition3D();

        lineRenderer.SetPosition(0, downPosition3D);
        lineRenderer.SetPosition(1, nowPosition3D);

    }
}

