using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���n�u�����̊֐������iPhoton�j
/// </summary>


public class GeneratePrefabPhoton : MonoBehaviourPunCallbacks
{
    public void GeneratePrefabPhotonByBoxVolume(GameObject _prefab,Vector3 _minPos,Vector3 _maxPos)
    {
        if(!_prefab)
        {
            Debug.LogWarning("�v���n�u�����݂��܂���I(GeneratePrefabPhoton)");
            return;
        }

        Vector3 pos;

        pos.x = UnityEngine.Random.Range(_minPos.x, _maxPos.x);
        pos.y = UnityEngine.Random.Range(_minPos.y, _maxPos.y);
        pos.z = UnityEngine.Random.Range(_minPos.z, _maxPos.z);

        SingleMultiUtility.Instantiate(_prefab.name, pos, Quaternion.identity);
    }
}
