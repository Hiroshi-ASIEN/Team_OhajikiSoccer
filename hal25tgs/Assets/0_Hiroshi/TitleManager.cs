using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviourPunCallbacks
{
//    [SerializeField] private SceneChanger m_SceneChanger;

    // Start is called before the first frame update
    void Start()
    {
        // ネットワークに接続
        if (!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();
        ScoreManager.Instance.ResetScores();// スコアリセット
    }

    // Update is called once per frame
    void Update()
    {
        /*
        bool start = false;

        // マウスクリックでスタート
        if (Input.GetMouseButton(0))
        { 
            start = true;
        }

        // スマホ画面タッチでスタート
        if (Input.touchCount > 0)
        { 
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                start = true;
            }
        }

        if (start)
        {
            // RPC()でネットワーク接続済みの同室クライアントで引数１関数が実行される
            // 引数２はターゲット：全て
            photonView.RPC("LoadScene",RpcTarget.All);
        }

        */
    }

    // マスターサーバーに接続された時実行
    public override void OnConnectedToMaster()
    {
        // ルームが存在しなければ新規作成、あればそこに参加
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    [PunRPC]    // 関数前にこれを付けることで、他ユーザーがリモートで呼び出せる関数にする
    public void LoadScene()
    {
        //        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("GameScene");    // ゲームシーンに遷移
    }

    // デバッグ用UI表示関数
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 16;
        GUI.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        GUILayout.Label("Start : " + PhotonNetwork.NetworkClientState.ToString());  // 接続状態
        GUILayout.Label("Start : " + PhotonNetwork.GetPing().ToString() + "ms");    // 接続にかかるミリ秒

        // ルームに入ったら表示
        if (PhotonNetwork.InRoom)
        {
            GUILayout.Label("RoomName : " + PhotonNetwork.CurrentRoom.Name);    // ルーム名
            GUILayout.Label("PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString());   // プレイヤー数
            GUILayout.Label("MasterClient : " + PhotonNetwork.IsMasterClient.ToString());   // マスタークライアント
        }
    }
}
