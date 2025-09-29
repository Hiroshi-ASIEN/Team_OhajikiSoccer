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
        // �l�b�g���[�N�ɐڑ�
        if (!PhotonNetwork.IsConnected) PhotonNetwork.ConnectUsingSettings();
        ScoreManager.Instance.ResetScores();// �X�R�A���Z�b�g
    }

    // Update is called once per frame
    void Update()
    {
        /*
        bool start = false;

        // �}�E�X�N���b�N�ŃX�^�[�g
        if (Input.GetMouseButton(0))
        { 
            start = true;
        }

        // �X�}�z��ʃ^�b�`�ŃX�^�[�g
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
            // RPC()�Ńl�b�g���[�N�ڑ��ς݂̓����N���C�A���g�ň����P�֐������s�����
            // �����Q�̓^�[�Q�b�g�F�S��
            photonView.RPC("LoadScene",RpcTarget.All);
        }

        */
    }

    // �}�X�^�[�T�[�o�[�ɐڑ����ꂽ�����s
    public override void OnConnectedToMaster()
    {
        // ���[�������݂��Ȃ���ΐV�K�쐬�A����΂����ɎQ��
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    [PunRPC]    // �֐��O�ɂ����t���邱�ƂŁA�����[�U�[�������[�g�ŌĂяo����֐��ɂ���
    public void LoadScene()
    {
        //        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("GameScene");    // �Q�[���V�[���ɑJ��
    }

    // �f�o�b�O�pUI�\���֐�
    private void OnGUI()
    {
        GUI.skin.label.fontSize = 16;
        GUI.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        GUILayout.Label("Start : " + PhotonNetwork.NetworkClientState.ToString());  // �ڑ����
        GUILayout.Label("Start : " + PhotonNetwork.GetPing().ToString() + "ms");    // �ڑ��ɂ�����~���b

        // ���[���ɓ�������\��
        if (PhotonNetwork.InRoom)
        {
            GUILayout.Label("RoomName : " + PhotonNetwork.CurrentRoom.Name);    // ���[����
            GUILayout.Label("PlayerCount : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString());   // �v���C���[��
            GUILayout.Label("MasterClient : " + PhotonNetwork.IsMasterClient.ToString());   // �}�X�^�[�N���C�A���g
        }
    }
}
