using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class XControllerEpic : MonoBehaviour 
{
	Vector3 position = Vector3.zero;
	Vector3 velosity = Vector3.zero;
	public float dt = 1.0f/60.0f;
	private const int size = 250;
    private float[] valx = new float[size];
	private float[] valy = new float[size];
	private float[] valz = new float[size];
	private int last = 0;
	private int ccc = size;
	Text text;
	
	void Start()
	{
		text = GameObject.Find("SecondsText").GetComponent<Text>();
		Input.gyro.enabled = true;
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
	
	void Update()
	{
		Vector3 a = Input.gyro.attitude.eulerAngles;
		Vector3 acc = Input.acceleration;
		Vector3 accr = acc;
		
		/*
		accr.x = (float) ((cos(a.x) * cos(a.z) - sin(a.x) * cos(a.y) * sin(a.z)) * acc.x
		               + (-cos(a.x) * sin(a.z) - sin(a.x) * cos(a.y) * cos(a.z)) * acc.y
			           + ( sin(a.x) * sin(a.y)) * acc.z);
					   
		accr.y = (float) ((sin(a.x) * cos(a.z) + cos(a.x) * cos(a.y) * sin(a.z)) * acc.x
		               + (-sin(a.x) * sin(a.z) + cos(a.x) * cos(a.y) * cos(a.z)) * acc.y
			           + (-cos(a.x) * sin(a.y)) * acc.z);
					   
		accr.z = (float) ((sin(a.y) * sin(a.z)) * acc.x
					   + ( sin(a.y) * cos(a.z)) * acc.y
					   + ( cos(a.y) ) * acc.z);
		*/

		 //����������������
		 /*
		accr.x = (float) ((cos(a.x) * cos(a.z) - sin(a.x) * cos(a.y) * sin(a.z)) * acc.x
						+ (sin(a.x) * cos(a.z) + cos(a.x) * cos(a.y) * sin(a.z)) * acc.y
						+ (sin(a.y) * sin(a.z)) * acc.z);
		
		accr.y = (float) ((-cos(a.x) * sin(a.z) - sin(a.x) * cos(a.y) * cos(a.z)) * acc.x
						+ (-sin(a.x) * sin(a.z) + cos(a.x) * cos(a.y) * cos(a.z)) * acc.y
						+ ( sin(a.y) * cos(a.z)) * acc.z);
		
		accr.z = (float) (( sin(a.x) * sin(a.y)) * acc.x
						+ (-cos(a.x) * sin(a.y)) * acc.y
						+ ( cos(a.y) ) * acc.z);
		*/
					
		last = (last + 1) % size;
		valx[last] = accr.x;
		valy[last] = accr.y;
		valz[last] = accr.z;
		
		//���������� ������ � ����� ������
		ccc = ccc != 0 ? ccc -1 : ccc;
		if(ccc > 0) return;

		
		velosity.x = (accr.x - average(valx)) * dt;
		velosity.y = (accr.y - average(valy)) * dt;
		velosity.z = (accr.z - average(valz)) * dt;
		
		position.x += velosity.x * dt;
		position.y += velosity.y * dt;
		position.z += velosity.z * dt;
		
		text.text = "" + velosity;
		
		Debug.Log("AAAA " + a);
		Debug.Log("BBBB " + acc);
		Debug.Log("CCCC " + accr);
	}
}