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
	
	void Start()
	{
		text = GameObject.Find("SecondsText").GetComponent<Text>();
		Input.gyro.enabled = true;
	}

	void FixedUpdate()
	{			
		Vector3 a = Input.gyro.attitude.eulerAngles;
		
		float ay = a.y > 180 ? a.y - 180 : a.y;
		float magic = (ay - 90.0f) /1.5f;
		float angle = a.z > 180 ? a.z - 180 : a.z;
		angle = angle + magic;
		
		
		if (angle > 0 && angle < 90 - dAngle)
		{
			if (OnXLeft != null)
				OnXLeft();
			text.text = "\n\n\nRight\n" + angle;
		} else
		if (angle > 90 + dAngle && angle < 180)
		{
			if (OnXRight != null)
				OnXRight();
			text.text = "\n\n\nLeft\n" + angle;
		} else
			text.text = "\n\n\n" + angle;

	}
}