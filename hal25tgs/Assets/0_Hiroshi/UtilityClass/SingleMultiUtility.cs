using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

//==================================================
// - シングルプレイ時は 通常のUnity機能を使用
// - マルチプレイ時はマスタークライアントのみ Photon機能使用
//==================================================

public static class SingleMultiUtility
{
    //==================================================
    // オブジェクト生成
    // 引数：Resources内のプレハブ名 / 生成位置 / 向き
    // 戻り値：生成されたGameObject。生成しなかった場合はnull
    //==================================================
    public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected)
        {
            // シングルプレイ時はResourcesから読み込み
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogWarning($"プレハブが見つかりません: {prefabName}");
                return null;
            }

            return GameObject.Instantiate(prefab, position, rotation);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // マルチプレイ時かつマスターなら生成
            return PhotonNetwork.Instantiate(prefabName, position, rotation);
        }

        // 非マスターの場合は生成しない
        return null;
    }

    public static GameObject InstantiateForClient(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected)
        {
            // シングルプレイ時はResourcesから読み込み
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                Debug.LogWarning($"プレハブが見つかりません: {prefabName}");
                return null;
            }

            return GameObject.Instantiate(prefab, position, rotation);
        }

        // マルチプレイ時なら各クライアントが自分用に生成
        return PhotonNetwork.Instantiate(prefabName, position, rotation);
    }
    //==================================================
    // オブジェクト破棄
    // 引数：破棄したいゲームオブジェクト
    //==================================================
    public static void Destroy(GameObject obj)
    {
        if (obj == null) return;

        if (!PhotonNetwork.IsConnected)
        {
            // シングルプレイ
            GameObject.Destroy(obj);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // マルチプレイ：マスターが破棄
            PhotonNetwork.Destroy(obj);
        }
    }

    // あまり使わない方がいいかも
    public static void DestroyForClient(GameObject obj)
    {
        if (obj == null) return;

        if (!PhotonNetwork.IsConnected)
        {
            // シングルプレイ
            GameObject.Destroy(obj);
        }
            // マルチプレイ：各クライアントが破棄
            PhotonNetwork.Destroy(obj);
    }
    //==================================================
    // シーン遷移
    // 引数：次のシーン名
    //==================================================
    public static void LoadScene(string _nextSceneName)
    {
        if (!PhotonNetwork.IsConnected)
        {
            // シングルプレイ
            SceneManager.LoadScene(_nextSceneName);
        }
        else if (PhotonNetwork.IsMasterClient)
        {
            // マルチプレイ：マスターがシーンを同期遷移
            PhotonNetwork.LoadLevel(_nextSceneName);
        }
    }

}
