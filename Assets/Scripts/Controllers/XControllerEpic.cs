using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class XControllerEpic : MonoBehaviour 
{
    public delegate void LeftAction();
    public delegate void RightAction();
    public static LeftAction OnXLeft;
    public static RightAction OnXRight;

	private Text text;
	public float dAngle = 15.0f;
	GameObject camera;
	
	void Start()
	{
		camera = GameObject.Find("VrCamera");
		text = GameObject.Find("SecondsText").GetComponent<Text>();
		Input.gyro.enabled = true;
	}

	void FixedUpdate()
	{	
		//text.text = "" + camera.transform.rotation.eulerAngles;
		Vector3 a = camera.transform.rotation.eulerAngles;
		float angle = a.z;

		if (angle > 0 + dAngle && angle < 90)
		{
			if (OnXLeft != null)
				OnXLeft();
			//text.text = "\n\n\nLeft\n" + angle;
		} else
		if (angle < 360 - dAngle && angle > 270)
		{
			if (OnXRight != null)
				OnXRight();
			//text.text = "\n\n\nRight\n" + angle;
		}// else
			//text.text = "\n\n\n" + angle;

	}
}