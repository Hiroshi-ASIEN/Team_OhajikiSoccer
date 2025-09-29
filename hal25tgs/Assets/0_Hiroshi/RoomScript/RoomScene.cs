using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class RoomScene : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        PhotonNetwork.NickName = "Player";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
//        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        // ÉvÉåÉCÉÑÅ[ê∂ê¨
//        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
//        PhotonNetwork.Instantiate("Player1", position, Quaternion.identity);

        if (PhotonNetwork.IsMasterClient)
        {
//            PhotonNetwork.CurrentRoom.SetStartTime(PhotonNetwork.ServerTimestamp);
        }
    }
}