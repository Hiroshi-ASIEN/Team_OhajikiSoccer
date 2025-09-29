using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PhotonView))]

public class Ball_Expansion : MonoBehaviourPun
{
    // �Ǘ��t���O
    private bool m_EnableExpansion;

    [SerializeField] private float m_ExpandMul = 2.0f;  // ���剻�{��
    [SerializeField] private float m_ExpansionTime = 3.0f;  // ���剻����
    private float m_ElapsedTime;
    private Vector3 m_CurrentScale;
    private bool m_IsExpanded;
    private GameObject m_Ball;

    // Start is called before the first frame update
    void Start()
    {
        m_Ball = GameObject.FindGameObjectWithTag("Ball");

        // �����T�C�Y�ۑ�
        m_CurrentScale = m_Ball.transform.localScale;

        //�}�X�^�[�̏ꍇ�̂݊g����J�n����
        if (PhotonNetwork.IsMasterClient)
            ExpandStart();
        else if(PhotonNetwork.InRoom==false)
        {
            //��ʐM��ԂȂ�΁@�g����J�n����
            ExpandStart();
        }
        else
        {
            //��������
            Destroy(this.gameObject);
            Debug.Log("Ball_Expansion�͎������܂���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        // ���剻
        if (m_EnableExpansion && !m_IsExpanded)
        {
            //photonView.RPC("Expand", RpcTarget.All);
            Expand();
        }

        // m_ExpansionTime�o������T�C�Y��߂�
        if (m_ElapsedTime > m_ExpansionTime && m_IsExpanded)
        {
            //photonView.RPC("Contract", RpcTarget.All);
            Contract();
        }
    }

    private void Expand()
    {
        m_IsExpanded = true;
        m_ElapsedTime = 0.0f;

        // ���剻
        ApplyScale(m_CurrentScale * m_ExpandMul);
    }

    private void Contract()
    {
        m_EnableExpansion = false;
        m_IsExpanded = false;

        // �k����
        ApplyScale(m_CurrentScale);


        //��������
        Destroy(this.gameObject);
        Debug.Log("Ball_Expansion�͎������܂���");
    }

    public void ExpandStart()
    {
        m_EnableExpansion = true;
    }

    // �X�P�[���ύX��̋��ʏ���
    void ApplyScale(Vector3 newScale)
    {
        m_Ball.transform.localScale = newScale;

        //// �R���C�_�[�ėL�����i�X�P�[���ύX�̔��f�j
        //Collider col = m_Ball.GetComponent<Collider>();
        //col.enabled = false;
        //col.enabled = true;

        //// Rigidbody�Đݒ�
        //Rigidbody rb = m_Ball.GetComponent<Rigidbody>();

        //// �K�v�ɉ����Ď��ʂƊ����𒲐�
        //float scaleFactor = newScale.x; // �ϓ��X�P�[�����O�O��
        //rb.mass = m_BaseMass * Mathf.Pow(scaleFactor, 3); // �̐ςɔ��
        //rb.ResetInertiaTensor(); // ��]�����̃Y���h�~



    }
}

