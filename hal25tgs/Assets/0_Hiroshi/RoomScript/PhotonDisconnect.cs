using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhotonDisconnect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
        { 
            PhotonNetwork.Disconnect();
        }
    }
}
