using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentTrackUI : MonoBehaviour
{

    public Text Text;

    void Awake()
    {
    }

	// Use this for initialization
	void Start () {
        //TODO: find a better way
        gameObject.SetActive(false);
#if UNITY_EDITOR
        gameObject.SetActive(true);
#endif
    }

    // Update is called once per frame
    void Update ()
	{
	    //Text.text = "Current track: " + RobotMovement.CurrentTrack;
	}
}
