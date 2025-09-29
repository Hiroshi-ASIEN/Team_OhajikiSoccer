using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeImmediately : MonoBehaviour
{
    [SerializeField] SceneChanger sceneChanger;
    [SerializeField] FadeInOut fade;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            sceneChanger.IsActive();
            fade.FadeStart();
        }
    }
}
