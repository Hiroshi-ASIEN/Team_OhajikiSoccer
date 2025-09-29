using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]

public class WarpUnit : MonoBehaviour
{
    [Header("ワープ親")]
    [SerializeField] private WarpGimmick m_warpGimmick;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWarpGimmick(WarpGimmick _warp)
    {
        m_warpGimmick = _warp;
        Debug.Log("ワープの親がセットされました");

    }

    //触れたプレイヤーを送る　いやぁ。Enterにしたかったけど、最初はStayなんだよなぁ　エンターでいっか
    private void OnTriggerEnter(Collider other)
    {
        //プレイヤーレイヤーだけを判定したいなぁ
        //今はとりあえずコンポーネントで判定する

        if (other.gameObject.TryGetComponent<PlayerObject>(out var player))
        {
            Debug.Log("ワープを試みました");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecievePlayer(player, this);
            }
        }
        else if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
        {
            Debug.Log("ワープを試みました");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecieveEnemy(enemy, this);
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (m_warpGimmick)
        {
            if(m_warpGimmick.GetIsDetectingStayCollision() == false)
            {
                return;
            }
        }

        //プレイヤーレイヤーだけを判定したいなぁ
        //今はとりあえずコンポーネントで判定する
        if (other.gameObject.TryGetComponent<PlayerObject>(out var player))
        {
            Debug.Log("ワープを試みました");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecievePlayer(player, this);
            }
            else
            {
                Debug.Log("親がいません");
            }
        }
        else if (other.gameObject.TryGetComponent<EnemyAI>(out var enemy))
        {
            Debug.Log("ワープを試みました");

            if (m_warpGimmick)
            {
                m_warpGimmick.RecieveEnemy(enemy, this);
            }
        }
    }


}
