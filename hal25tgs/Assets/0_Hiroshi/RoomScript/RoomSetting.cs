using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSetting : MonoBehaviourPunCallbacks
{
    // ルーム設定タイプ
    enum ROOMSETTING_TYPE
    { 
        Open,
        Visible,
    };

//    [SerializeField] ROOMSETTING_TYPE m_SettingType;    // 設定タイプ

    private Room m_Room;

    private void Start()
    {
        m_Room=PhotonNetwork.CurrentRoom;
    }
    public void RoomOpen()
    {
        m_Room.IsOpen = true;
    }

    public void RoomClose()
    {
        m_Room.IsOpen = false;
    }

    public void RoomVisible()
    {
        m_Room.IsVisible = true;
    }

    public void RoomInvisible()
    {
        m_Room.IsVisible = false;
    }

    
}
