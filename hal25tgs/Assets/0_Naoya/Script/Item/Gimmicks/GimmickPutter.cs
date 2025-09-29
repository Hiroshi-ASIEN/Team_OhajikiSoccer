using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(RayCastPosition))]

public class GimmickPutter : MonoBehaviourPunCallbacks
{
    //�ݒu�������v���n�u
    GameObject m_prefab = null;

    //���ݐݒu����
    ITEM_STATE m_state = ITEM_STATE.JUDGING;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�ݒu���Ȃ��...
        if (m_state == ITEM_STATE.RUNNING)
        {
            //�t���̃R���|�[�l���g�A�i�ʃI�u�W�F�N�g�ɂł�����H�j�ňړ�



            //�{�^���������ꂽ��ݒu����
            if (Input.GetMouseButtonDown(0))
            {
                Put();
            }

            //�E�{�^���������ꂽ��L�����Z���i���j
            if (Input.GetMouseButtonDown(1))
            {
                CancelPutting();
            }
        }
    }

    private void FixedUpdate()
    {

    }


    //�ݒu�J�n
    public void StartPutting()
    {
        m_state = ITEM_STATE.RUNNING;
    }


    private void Put()
    {
        //��]�͂��܍l�����Ȃ��I�ݒu�ォ��
        //SingleMultiUtility.Instantiate(m_prefab.name, this.transform.position, Quaternion.identity);
        //�����ꂾ�߂��I

        if (!PhotonNetwork.IsConnected)
        {
            // �V���O���v���C����Resources����ǂݍ���
            GameObject prefab = Resources.Load<GameObject>(m_prefab.name);
            if (prefab == null)
            {
                Debug.LogWarning($"�v���n�u��������܂���: {m_prefab.name}");
                m_state = ITEM_STATE.FAILURE;
                return;
            }

            GameObject.Instantiate(m_prefab, this.transform.position, Quaternion.identity);
        }
        else
        {
            // �}���`�v���C���Ȃ琶��
            PhotonNetwork.Instantiate(m_prefab.name, this.transform.position, Quaternion.identity);
        }


        m_state = ITEM_STATE.SUCCESS;

        Debug.Log("�v���n�u��ݒu���܂���");

    }


    //���f����(������{�^��������ĂԂ̂��ȁH)
    public void CancelPutting()
    {
        m_state = ITEM_STATE.FAILURE;
    }
    

    //�X�e�[�g���擾����@������C���x���g���H���g�p����
    public ITEM_STATE GetState()
    {
        return m_state;
    }

    public void SetPrefab(GameObject _prefab)
    {
        m_prefab = _prefab;
    }
}
