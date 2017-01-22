using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public InputField ipInput;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadScene(
        int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void LoadSceneWithIp(
        int sceneNumber)
    {
        LevelManager.getInstance().prepareForMulti();
        Config.isSingle = false;

        if (ipInput.text != string.Empty)
        {
            Client2.ip = ipInput.text;
            SceneManager.LoadScene(sceneNumber);
        }
    }

    public void LoadSceneSinglePlayer(int sceneNumber)
    {
        LevelManager.getInstance().prepareForSingle();
        Config.isSingle = true;
        SceneManager.LoadScene(sceneNumber);
    }
}
