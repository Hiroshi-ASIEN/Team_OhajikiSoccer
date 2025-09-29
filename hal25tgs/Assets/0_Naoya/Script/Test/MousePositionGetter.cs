using UnityEngine;

//============================================================
//マウス座標取得
//============================================================

/// <summary>
/// マウス座標取得クラス
/// </summary>
public class MousePositionGetter : MonoBehaviour
{
    //マウス位置
    Vector2 m_mousePos = Vector2.zero;

    void FixedUpdate()
    {
        m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public Vector2 GetMousePos() { return m_mousePos; }
}
