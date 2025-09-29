using UnityEngine;

//============================================================
//�}�E�X���W�擾
//============================================================

/// <summary>
/// �}�E�X���W�擾�N���X
/// </summary>
public class MousePositionGetter : MonoBehaviour
{
    //�}�E�X�ʒu
    Vector2 m_mousePos = Vector2.zero;

    void FixedUpdate()
    {
        m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 GetMousePos() { return m_mousePos; }
}
