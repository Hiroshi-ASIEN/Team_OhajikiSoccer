using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GeneratePrefabPhoton))]

//======================================================================
// 05/19�܂Ł@�ނ炽����
// 05/20����@���킩�ݐ���
//
// �A�C�e���𓯊��Ő���
// ���������A�C�e�����Ǘ����邩�͖���
// �ł���Ε����Ă����������ǂȂ�
//======================================================================


public class ItemManager : MonoBehaviourPunCallbacks
{
    //�A�C�e��
    //�ć@ ���݂͂�����
    /*
      ���X�g�ɃA�C�e���̃v���n�u��ݒ�
      ���X�g�̒��̃A�C�e���������_���ɐ���
     */

    //�ćA
    /*
      �}���J�[�̂悤��1�̃v���n�u�ŁA
      �擾�����烉���_���ŃA�C�e���擾
    */
    [SerializeField] List<GameObject> m_ItemPrefabList;

    //�A�C�e�������ꏊ�ƕp�x
    /*
      �ꏊ�F�A�C�e���̐����ʒu�͕����p�ӂ��Ă����āA
      �@�@�@�ǂ����̏ꏊ���烉���_���ŏo��
    �@�p�x�F3�^�[����1����
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
        //�Ƃ肠�����f�o�b�O�ŏo��
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GenerateItem();

            //PhotonNetwork.Instantiate(m_ItemPrefabList[0].name, m_TargetPoint[0].transform.position, Quaternion.identity);
            Debug.Log("�A�C�e�����������݂܂���");
        }
    }


    /// <summary>
    /// ������Q�[���}�l�[�W���[���ŌĂԂ̂��ȁH����Ƃ������������Ŋ������ȁH
    /// �����͂ł��Ȃ����낤�ȁB��}�X�^�[�ł�����Ă񂶂Ⴂ���Ȃ�����
    /// </summary>
    public void GenerateItem()
    {
        if(m_ItemPrefabList.Count <=0)
        {
            Debug.Log("�A�C�e���̃v���n�u���o�^����Ă��炸�A�����ł��܂���");
            return;
        }


        //�܂��͔ԍ��������_���Ō��߂�...
        int index =
            UnityEngine.Random.Range(0, m_ItemPrefabList.Count);


        //�Q�[���I�u�W�F�N�g������
        GameObject prefab = m_ItemPrefabList[index];


        //������ꏊ�Ő�������B���g�̑̐ς������Ȃ�
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
