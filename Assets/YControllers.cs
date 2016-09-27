using UnityEngine;
using System.Collections;

public class YControllers : MonoBehaviour {
    public float dAcc = 0.1F;
    
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    
    private static int size = 250;
    private float[] val = new float[size];
    private int lastVal = 0;
    
    void Start(){
        for(int i=0; i < size; ++i){
            val[i] = 0.0F;
        }
    }
    
    float sr(){
        float result = 0.0F;
        for(int i=0; i < size; ++i){
            result += val[i];
        }
        return result / size;
    }
    
    void updLastVal(){
        lastVal = lastVal + 1 == size ? 0 : lastVal + 1;
    }
    
    bool bb(float a, float b){
        return a - b > dAcc;
    }

    int abs(int a){
        return a > 0 ? a : -a;
    }
    
    public Vector3 top = Vector3.zero;
    public Vector3 mid = Vector3.zero;
    public Vector3 bot = Vector3.zero;
    
    public int dS = 5;
    private int step = 0;
    
    void Update() {
        if (bb(sr(), Input.acceleration.y)){
            --step;
        } else if (bb(Input.acceleration.y, sr())){
            ++step;
        } else {
            if(step < 0) ++step;
            else --step;
        }
        
        if (abs(step) < dS){
            Vector3 hz = transform.position;
            hz.y = mid.y;
            transform.position = hz;
        } else if (step < 0){
            Vector3 hz = transform.position;
            hz.y = top.y;
            transform.position = hz;
        } else {
            Vector3 hz = transform.position;
            hz.y = bot.y;
            transform.position = hz;
        }
        
        val[lastVal] = Input.acceleration.y;
        updLastVal();
    }
}
