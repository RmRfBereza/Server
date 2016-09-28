using UnityEngine;
using System.Collections;

public class XControllers : MonoBehaviour 
{
    public float dAcc = 0.1F;           // Величина отклонения для активации
    public int dS = 5;                  // Минимальное количество отклонений для активации
    
    public Vector3 left = Vector3.zero;  // Левая позиция
    public Vector3 mid = Vector3.zero;   // Стандартная позиция  
    public Vector3 right = Vector3.zero; // Правая позиция
    
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
            Vector3 hz = transform.position;
            hz.x = mid.x;
            transform.position = hz;
        }
        else if (devation > 0)
        {
            Vector3 hz = transform.position;
            hz.x = left.x;
            transform.position = hz;
        }
        else 
        {
            Vector3 hz = transform.position;
            hz.x = right.x;
            transform.position = hz;
        }
    }
    
    void Update()
    {
        updateDevation();
        updatePosition();      
        updateAverage();
    }
}
