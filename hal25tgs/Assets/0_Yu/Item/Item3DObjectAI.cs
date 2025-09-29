using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// �A�C�e����3D���N���X
/// </summary>
//----------------------------------------------------------------

//---------------------
//�v��
//
//�����蔻��ƃ��f���Ǘ����s��
//---------------------

//�K�{�R���|�[�l���g
[RequireComponent(typeof(SphereCollider))]      //���R���C�_�[

public class Item3DObjectAI : MonoBehaviour
{
    //============================================================
    // �ϐ�
    //============================================================

    //------------------
    // �v���C�x�[�g
    //------------------
    private SphereCollider m_sphereCollider = null;     //���R���C�_�[
    private bool m_isCollided = false;                  //�G�ꂽ���ǂ���

    //�G�ꂽ�v���C���[
    private PlayerObject m_playerObject = null;

    //============================================================
    // �֐�
    //============================================================
    void Reset()
    {
        // �K�v�ȃR���|�[�l���g�̎Q�Ƃ��擾����
        if (!m_sphereCollider)
            m_sphereCollider = this.GetComponent<SphereCollider>();

        m_sphereCollider.isTrigger = true;
    }
    void Start()
    {
        // �K�v�ȃR���|�[�l���g�̎Q�Ƃ��擾����
        if (!m_sphereCollider)
            m_sphereCollider = this.GetComponent<SphereCollider>();

        m_sphereCollider.isTrigger = true;
    }


    //�����蔻��
    private void OnTriggerEnter(Collider other)
    {
        ////�G�ꂽ�����Ă��v���C���[�Ȃ�΁A�G�ꂽ�A�ɂ���
        //if(other.gameObject.CompareTag("Player"))
        //{
        //    m_isCollided = true;

        //    //�G�ꂽ�����ۑ��������ȁB���t���[���ɐG�ꂽ�ꍇ�A�A�C�e����2�ɂȂ�Ȃ��悤�ɁB������

        //}

        //�i�[�I
        if (other.gameObject.TryGetComponent<PlayerObject>(out var player))
        {
            if (m_playerObject == null)
                m_playerObject = player;

            m_isCollided = true;

            Debug.Log("collide player");
        }
    }

    //�G�ꂽ���ǂ����𑗐M����iItem������Ăԁj
    public bool GetIsCollided()
    {
        return m_isCollided;
    }


    //�G�ꂽ���v���C���[�𑗐M����iItem������Ăԁj
    public PlayerObject GetCollidedPlayer()
    {
        return m_playerObject;
    }





}
