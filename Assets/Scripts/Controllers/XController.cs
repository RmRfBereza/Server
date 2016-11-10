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
    
    
    void updateAverage()
    {
        val[lastVal] = Input.acceleration.x;
        updLastVal();
    }
    
    void updateDevation()
    {
        if (Mathf.Abs(average() - Input.acceleration.x) < dAcc)
        {
            if (devation < 0) ++devation;
            else --devation;
        } else if (average() > Input.acceleration.x)
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
        updateDevation();
        updatePosition();      
        updateAverage();
        //logContr();
    }

    void logContr()
    {
        text.text = "devation " + devation + "\nacceleration x " + 
            Input.acceleration.x;
    }
}