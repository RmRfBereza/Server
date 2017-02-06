using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCommonsHandler : MonoBehaviour {

    private static int _currentLevel = 2;

	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadNextLevel()
    {
        print(SceneManager.sceneCountInBuildSettings);
        if (SceneManager.sceneCountInBuildSettings > ++_currentLevel)
        {
            SceneManager.LoadScene(_currentLevel);
        }
        else
        {
            SceneManager.LoadSceneAsync(0);
            //Destroy(gameObject);
        }
    }
}
