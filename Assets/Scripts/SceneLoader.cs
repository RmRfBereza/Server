using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class SceneLoader : MonoBehaviour {

    public InputField ipInput;
    public GameObject Preloader;
    public Thread th;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreatePreloader()
    {
        Instantiate(Preloader);
    }

    public void LoadScene(
        int sceneNumber)
    {
        Instantiate(Preloader);
        SceneManager.LoadSceneAsync(sceneNumber);
    }

    public void LoadSceneWithIp(
        int sceneNumber)
    {
        LevelManager.getInstance().prepareForMulti();
        Config.isSingle = false;

        if (ipInput.text != string.Empty)
        {
            Instantiate(Preloader);
            Client2.ip = ipInput.text;
            SceneManager.LoadSceneAsync(sceneNumber);
        }
    }

    public void LoadSceneSinglePlayer(int sceneNumber)
    {
        Instantiate(Preloader);
        LevelManager.getInstance().prepareForSingle();
        Config.isSingle = true;
        SceneManager.LoadSceneAsync(sceneNumber);
    }
}
