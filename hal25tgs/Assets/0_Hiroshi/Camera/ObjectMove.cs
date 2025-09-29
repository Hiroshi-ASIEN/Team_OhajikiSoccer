using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    [Serializable]
    public class ObjTransform
    {
        [SerializeField] public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        [SerializeField] public Quaternion rotation = Quaternion.identity;
        [SerializeField] public Vector3 localScale = new Vector3(0.0f, 0.0f, 0.0f);
    };

    [SerializeField] protected GameObject m_Object;
    [SerializeField] ObjTransform[] m_Transforms;

    public void SetPosition(int _index)
    {
        m_Object.transform.position = m_Transforms[_index].position;
    }

    public void SetRotation(int _index)
    {
        m_Object.transform.rotation = m_Transforms[_index].rotation;
    }

    public void SetScale(int _index)
    {
        m_Object.transform.localScale = m_Transforms[_index].localScale;
    }

    public void SetAll(int _index)
    {
        SetPosition(_index);
        SetRotation(_index);
        SetScale(_index);
    }
}
