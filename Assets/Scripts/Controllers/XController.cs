using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class XController : MonoBehaviour 
{
    public static float dAcc = 0.1F;           // ¬еличина отклонени€ дл€ активации
    public static int dS = 5;                  // ћинимальное количество отклонений дл€ активации
    
    public delegate void LeftAction();
    public delegate void RightAction();
    public static LeftAction OnXLeft;
    public static RightAction OnXRight;
    
    private const int size = 250;
    private float[] val = new float[size];
    private int lastVal = 0;
    private int devation = 0;

    Text text;
    
    void Start()
    {
        text = GameObject.Find("SecondsText").GetComponent<Text>();
        for (int i=0; i < size; ++i)
        {
            val[i] = 0.0F;
        }
        Input.gyro.enabled = true;
    }

    double cos(float degrees)
    {
        float angle = degrees * Mathf.Deg2Rad;
        return Mathf.Cos(angle);
    }

    double sin(float degrees)
    {
        float angle = degrees * Mathf.Deg2Rad;
        return Mathf.Sin(angle);
    }

    float average()
    {
        float result = 0.0F;
        for(int i=0; i < size; ++i)
        {
            result += val[i];
        }
        return result / size;
    }
    
    void updLastVal()
    {
        lastVal = (lastVal + 1) % size;
    }
    
    bool isALargerB(float a, float b)
    {
        return a - b > dAcc;
    }
    
    
    void updateAverage(float x)
    {
        val[lastVal] = x;
        updLastVal();
    }
    
    void updateDevation(float x)
    {
        if (Mathf.Abs(average() - x) < dAcc)
        {
            if (devation < 0) ++devation;
            else --devation;
        } else if (average() > x)
        {
            --devation;
        } else
        {
            ++devation;
        }
        //if (/*isALargerB(average(), Input.acceleration.x)*/Input.acceleration.x < average())
        //{
        //    --devation;
        //}
        //else if (/*isALargerB(Input.acceleration.x, average())*/Input.acceleration.x > average())
        //{
        //    ++devation;
        //}
        //else 
        //{
        //    //if(devation < 0) ++devation;
        //    //else --devation;
        //}
    }
    
    void updatePosition()
    {
        if (Mathf.Abs(devation) > dS)
        {
            if (devation > 0)
            {
                if (OnXLeft != null)
                    OnXLeft();
            }
            else
            {
                if (OnXRight != null)
                    OnXRight();
            }
            devation = 0;
        }
    }
    
    void Update()
    {
        Vector3 a = Input.gyro.attitude.eulerAngles; 
        Vector3 acc = Input.gyro.userAcceleration;
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

        //траспонированна€

        accr.x = (float)((cos(a.x) * cos(a.z) - sin(a.x) * cos(a.y) * sin(a.z)) * acc.x
                        + (sin(a.x) * cos(a.z) + cos(a.x) * cos(a.y) * sin(a.z)) * acc.y
                        + (sin(a.y) * sin(a.z)) * acc.z);

        accr.y = (float)((-cos(a.x) * sin(a.z) - sin(a.x) * cos(a.y) * cos(a.z)) * acc.x
                        + (-sin(a.x) * sin(a.z) + cos(a.x) * cos(a.y) * cos(a.z)) * acc.y
                        + (sin(a.y) * cos(a.z)) * acc.z);

        accr.z = (float)((sin(a.x) * sin(a.y)) * acc.x
                        + (-cos(a.x) * sin(a.y)) * acc.y
                        + (cos(a.y)) * acc.z);

        updateDevation(Input.gyro.userAcceleration.x);
        updatePosition();      
        updateAverage(Input.gyro.userAcceleration.x);
        logContr(Input.gyro.userAcceleration.x);
    }

    void logContr(float x)
    {
        text.text = "devation " + devation + "\nacceleration x " + 
           x;
    }
}