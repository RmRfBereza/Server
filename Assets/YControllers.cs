using UnityEngine;
using System.Collections;

public class YControllers : MonoBehaviour 
{
    public float dAcc = 0.1F;           // Величина отклонения для активации
    public int dS = 5;                  // Минимальное количество отклонений для активации
    
    public Vector3 top = Vector3.zero;  // Верхняя позиция
    public Vector3 mid = Vector3.zero;  // Стандартная позиция  
    public Vector3 bot = Vector3.zero;  // Нижняя позиция
    
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
        val[lastVal] = Input.acceleration.y;
        updLastVal();
    }
    
    void updateDevation()
    {
        if (isALargerB(average(), Input.acceleration.y))
        {
            --devation;
        }
        else if (isALargerB(Input.acceleration.y, average()))
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
            hz.y = mid.y;
            transform.position = hz;
        }
        else if (devation < 0)
        {
            Vector3 hz = transform.position;
            hz.y = top.y;
            transform.position = hz;
        }
        else 
        {
            Vector3 hz = transform.position;
            hz.y = bot.y;
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
