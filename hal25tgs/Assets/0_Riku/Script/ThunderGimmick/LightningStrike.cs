using UnityEngine;
using Photon.Pun;

public class LightningStrike : MonoBehaviourPun
{
    public GameObject m_EffectPrefab;

    [PunRPC]
    public void PlayEffectOnce(Vector3 position)
    {
        Vector3 pos = m_EffectPrefab.transform.position;
        pos += position;

        GameObject effect = Instantiate(m_EffectPrefab, pos, Quaternion.identity);

        // ParticleSystem �̌p�����Ԃ��擾���Ď����폜
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            // �\���F2�b��ɍ폜
            Destroy(effect, 2.0f);
        }
    }
}
