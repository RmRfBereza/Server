using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{
    public Text ErrorText;

	// Use this for initialization
	void OnEnable()
	{
	    ErrorText.text = "Error: " + Config.ErrorMessage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
