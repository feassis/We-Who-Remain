using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadGameScene();
        }  
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
