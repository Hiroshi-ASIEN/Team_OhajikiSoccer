using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

//==================================================
// - �V���O���v���C���� �ʏ��Unity�@�\���g�p
// - �}���`�v���C���̓}�X�^�[�N���C�A���g�̂� Photon�@�\�g�p
//==================================================

public static class SingleMultiUtility
{
    //==================================================
    // �I�u�W�F�N�g����
    // �����FResources���̃v���n�u�� / �����ʒu / ����
    // �߂�l�F�������ꂽGameObject�B�������Ȃ������ꍇ��null
    //==================================================
    public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected)
        {
            // �V���O���v���C����Resources����ǂݍ���
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogWarning($"�v���n�u��������܂���: {prefabName}");
                return null;
            }

            return GameObject.Instantiate(prefab, position, rotation);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // �}���`�v���C�����}�X�^�[�Ȃ琶��
            return PhotonNetwork.Instantiate(prefabName, position, rotation);
        }

        // ��}�X�^�[�̏ꍇ�͐������Ȃ�
        return null;
    }

    public static GameObject InstantiateForClient(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected)
        {
            // �V���O���v���C����Resources����ǂݍ���
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogWarning($"�v���n�u��������܂���: {prefabName}");
                return null;
            }

            return GameObject.Instantiate(prefab, position, rotation);
        }

        // �}���`�v���C���Ȃ�e�N���C�A���g�������p�ɐ���
        return PhotonNetwork.Instantiate(prefabName, position, rotation);
    }
    //==================================================
    // �I�u�W�F�N�g�j��
    // �����F�j���������Q�[���I�u�W�F�N�g
    //==================================================
    public static void Destroy(GameObject obj)
    {
        if (obj == null) return;

        if (!PhotonNetwork.IsConnected)
        {
            // �V���O���v���C
            GameObject.Destroy(obj);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // �}���`�v���C�F�}�X�^�[���j��
            PhotonNetwork.Destroy(obj);
        }
    }

    // ���܂�g��Ȃ�������������
    public static void DestroyForClient(GameObject obj)
    {
        if (obj == null) return;

        if (!PhotonNetwork.IsConnected)
        {
            // �V���O���v���C
            GameObject.Destroy(obj);
        }
            // �}���`�v���C�F�e�N���C�A���g���j��
            PhotonNetwork.Destroy(obj);
    }
    //==================================================
    // �V�[���J��
    // �����F���̃V�[����
    //==================================================
    public static void LoadScene(string _nextSceneName)
    {
        if (!PhotonNetwork.IsConnected)
        {
            // �V���O���v���C
            SceneManager.LoadScene(_nextSceneName);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // �}���`�v���C�F�}�X�^�[���V�[���𓯊��J��
            PhotonNetwork.LoadLevel(_nextSceneName);
        }
    }

}
