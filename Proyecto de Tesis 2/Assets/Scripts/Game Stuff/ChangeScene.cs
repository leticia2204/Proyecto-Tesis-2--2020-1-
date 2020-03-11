using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    void Update()
    {
        //SceneManager.LoadScene("TopicSelect");
        if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene(2);
    }
}

