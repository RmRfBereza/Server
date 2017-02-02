using UnityEngine;


public class SkyboxManager : MonoBehaviour
{
    public Material[] skyboxMaterials;
    // Use this for initialization
    void Start()
    {
        RenderSettings.skybox = skyboxMaterials[Random.Range(0, skyboxMaterials.Length - 1)];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
