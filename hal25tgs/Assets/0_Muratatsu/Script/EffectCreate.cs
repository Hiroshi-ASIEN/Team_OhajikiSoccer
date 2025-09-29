using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;



public class EffectCreate : MonoBehaviour
{
    [SerializeField] GameObject m_Effect;
    private GameObject m_CurrentEffect;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            //m_CurrentEffect = Instantiate(m_Effect,new Vector3(0.0f,0.0f,0.0f),Quaternion.identity);
        }
    }
}
