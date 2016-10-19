using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurrentTrackUI : MonoBehaviour
{

    public Text Text;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
	{
	    Text.text = "Current track: " + RobotMovement.CurrentTrack;
	}
}
