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

	Vector3 lacc = Vector3.zero;
	Vector3 position = Vector3.zero;
	Vector3 velosity = Vector3.zero;
	public float dt = 10;
	private const int size = 250;
    private float[] valx = new float[size];
	private float[] valy = new float[size];
	private float[] valz = new float[size];
	private int last = 0;
	private int ccc = size;
	Text text;
	Vector3 b = Vector3.zero;
	
	void Start()
	{
		text = GameObject.Find("SecondsText").GetComponent<Text>();
		Input.gyro.enabled = true;
		b = Input.gyro.attitude.eulerAngles;
	}

	double cos(double degrees)
	{
		double angle = Math.PI * degrees / 180.0;
		return Math.Cos(angle);
	}
	
	double sin(double degrees)
	{
		double angle = Math.PI * degrees / 180.0;
		return Math.Sin(angle);
	}
	
	float average(float[] val)
    {
        float result = 0.0F;
        for(int i=0; i < size; ++i)
        {
            result += val[i];
        }
        return result / size;
    }
	
	int ggg = 20;
	
	int fff = 0;
	void FixedUpdate()
	{		
		if (!(ggg < 0) && --ggg == 0)
			b = Input.gyro.attitude.eulerAngles;
		if (ggg > 0) return;
		
		Vector3 a = Input.gyro.attitude.eulerAngles;
		Vector3 cv = a - b;
		a = cv;

		if (a.z < 20 && a.z > -20)
			fff = 0;
		if (a.z > 20 && a.z < 70)
			if (OnXLeft != null)
				OnXLeft();
		if (a.z < -20 && a.z > -70)
			if (OnXRight != null)
				OnXRight();
		
		text.text = "" + a;

	}
}