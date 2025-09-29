using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class CircleEffectTrigger : MonoBehaviourPun
{
    private HashSet<GameObject> m_Players = new HashSet<GameObject>();

    private TurnManager m_TurnManager;

    private bool m_Destroy = false;

    private void Start()
    {
        m_TurnManager = TurnManager.Instance;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!other.GetComponent<Rigidbody>().IsSleeping()) return;  // �����Ă�������ʂ����f����Ȃ��悤��

        OriginalStats stats = other.GetComponent<OriginalStats>();
        if (stats == null) return;

        if (stats.GetItemEffectActive()) return;

        // �v���C���[�Ƀ^�[�������L�^
        stats.SetTurnCount(m_TurnManager.GetTurnCount());

        // ���݂̔{���i���݃T�C�Y �� �����T�C�Y�j���Z�o
        float currentScaleFactor = other.transform.localScale.x / stats.GetOriginalScale().x;

        // �����_���ŐV�����{���i0.5 or 2.0�j
        float newScaleMultiplier = Random.value < 0.5f ? 0.5f : 2.0f;
        
        GetComponent<LightningStrike>().photonView.RPC("PlayEffectOnce", RpcTarget.AllBuffered, other.transform.position);

        // �V�����X�P�[�����v�Z
        Vector3 newScale = stats.GetOriginalScale() * newScaleMultiplier;
        other.transform.localScale = newScale;

        stats.ItemEffectActive();


        // �v���C���[��ۑ�
        m_Players.Add(other.gameObject);

        // Scale�ɍ��킹���V�����{���i0.1 or 10.0�j
        float newMassMultiplier = newScaleMultiplier == 0.5f ? 0.1f : 10.0f;

        // ���ʂ�ύX
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float newMass = stats.GetOriginalMass() * newMassMultiplier;
            rb.mass = newMass;
        }
    }

    private void OnDestroy()
    {
        m_Destroy = true;

        foreach (var player in m_Players)
        {
            if (player == null) continue;

            OriginalStats stats = player.GetComponent<OriginalStats>();
            if (stats == null) continue;

            if (stats.GetTurnCount() == m_TurnManager.GetTurnCount()) continue;

            stats.SetTurnCount(-1.0f);

            player.transform.localScale = stats.GetOriginalScale();

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.mass = stats.GetOriginalMass();
            }
        }
    }

    public bool GetDestroyFlg()
    {
        return m_Destroy;
    }

    public void AllDefault()
    {
        foreach (var player in m_Players)
        {
            if (player == null) continue;

            OriginalStats stats = player.GetComponent<OriginalStats>();
            if (stats == null) continue;

            stats.SetTurnCount(-1.0f);

            player.transform.localScale = stats.GetOriginalScale();

            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.mass = stats.GetOriginalMass();
            }
        }
    }
}
