using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string m_FirstSceneName = "FirstSceneName";
    [SerializeField] private string m_IceSceneName = "IceSceneName";
    [SerializeField] private string m_ThunderSceneName = "ThunderSceneName";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene(m_FirstSceneName);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene(m_IceSceneName);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene(m_ThunderSceneName);
        }
    }
}
