using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class XControllerEpic : MonoBehaviour 
{
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
	Vector3 a = Vector3.zero;
	
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
	
	void FixedUpdate()
	{
		//Vector3 a = Input.gyro.attitude.eulerAngles;
		
		a.x += Input.gyro.rotationRateUnbiased.x;
		a.y += Input.gyro.rotationRateUnbiased.y;
		a.z += Input.gyro.rotationRateUnbiased.z; //!!!!!Скорость в радианах в секунду(см документацию https://docs.unity3d.com/ScriptReference/Gyroscope-rotationRateUnbiased.html)
                                                    //, а ты прибавляешь с частотой FixedUpdate и потом ещё переводишь в радианы заново
        Vector3 acc = Input.acceleration; //Input.gyro.userAcceleration не учитывает гравитацию, нам нужен он
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
					   
		 //траспонированная
		 
		accr.x = (float) ((cos(a.x) * cos(a.z) - sin(a.x) * cos(a.y) * sin(a.z)) * acc.x
						+ (sin(a.x) * cos(a.z) + cos(a.x) * cos(a.y) * sin(a.z)) * acc.y
						+ (sin(a.y) * sin(a.z)) * acc.z);
		
		accr.y = (float) ((-cos(a.x) * sin(a.z) - sin(a.x) * cos(a.y) * cos(a.z)) * acc.x
						+ (-sin(a.x) * sin(a.z) + cos(a.x) * cos(a.y) * cos(a.z)) * acc.y
						+ ( sin(a.y) * cos(a.z)) * acc.z);
		
		accr.z = (float) (( sin(a.x) * sin(a.y)) * acc.x
						+ (-cos(a.x) * sin(a.y)) * acc.y
						+ ( cos(a.y) ) * acc.z);
		

		last = (last + 1) % size;
		valx[last] = accr.x;
		valy[last] = accr.y;
		valz[last] = accr.z;
		
		ccc = ccc != 0 ? ccc -1 : ccc;
		if(ccc > 0) return;
		
		accr.x -= average(valx);
		accr.y -= average(valy);
		accr.z -= average(valz);
		
		velosity.x += (accr.x ) * dt;
		velosity.y += (accr.y ) * dt;
		velosity.z += (accr.z ) * dt;
		
		position.x += velosity.x * dt;
		position.y += velosity.y * dt;
		position.z += velosity.z * dt;
		
		text.text = "" + a;
		
		Debug.Log("AAAA " + a);
		Debug.Log("BBBB " + acc);
		Debug.Log("CCCC " + accr);
		
		lacc = accr;
	}
}