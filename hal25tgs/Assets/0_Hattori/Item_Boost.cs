using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Boost : MonoBehaviour
{
    public GameObject player;              // Playerオブジェクト
    public float m_BoostAmount = 5.0f;        // 追加するスピード
    public float m_BoostDuration = 60f;       // ブーストの継続時間

    private float m_OriginalSpeed;           // 元のスピード
    private bool m_IsBoosting = false;       // ブースト中かどうか

    void Update()
    {
        // スペースキーを押したらブースト
        if (Input.GetKeyDown(KeyCode.Space) && !m_IsBoosting)
        {
            StartCoroutine(Boost());
            Debug.Log("スピードアップ成功");
        }
    }

    private System.Collections.IEnumerator Boost()
    {
        m_IsBoosting = true;

        // m_PlayerSpeed にアクセス
        var playerScript = player.GetComponent<Move>();
        if (playerScript != null)
        {
            m_OriginalSpeed = playerScript.m_PlayerSpeed;
            playerScript.m_PlayerSpeed += m_BoostAmount;

            Debug.Log(playerScript.m_PlayerSpeed);

            yield return new WaitForSeconds(m_BoostDuration);
        }

        m_IsBoosting = false;
    }
}
