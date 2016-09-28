using UnityEngine;
using System.Collections;

public class XControllers : MonoBehaviour 
{
    public float dAcc = 0.1F;           // Величина отклонения для активации
    public int dS = 5;                  // Минимальное количество отклонений для активации
    
    public delegate void LeftAction();
    public delegate void RightAction();
    public static LeftAction OnXLeft;
    public static RightAction OnXRight;
    
    private const int size = 250;
    private float[] val = new float[size];
    private int lastVal = 0;
    private int devation = 0;
    
    void Start()
    {
        for(int i=0; i < size; ++i)
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
        if (isALargerB(average(), Input.acceleration.x))
        {
            --devation;
        }
        else if (isALargerB(Input.acceleration.x, average()))
        {
            ++devation;
        }
        else 
        {
            if(devation < 0) ++devation;
            else --devation;
        }
    }
    
    void updatePosition()
    {
        if (Mathf.Abs(devation) < dS)
        {

        }
        else if (devation > 0)
        {
            if(OnXLeft != null)
                OnXLeft();
        }
        else 
        {
            if(OnXRight != null)
                OnXRight();
        }
    }
    
    void Update()
    {
        updateDevation();
        updatePosition();      
        updateAverage();
    }
}
