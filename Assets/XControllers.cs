using UnityEngine;
using System.Collections;

public class XControllers : MonoBehaviour {
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
    
    private static /*final*/ int STATE_FORWARD  = 0;
    private static /*final*/ int STATE_IN_LEFT  = 1;
    private static /*final*/ int STATE_IN_RIGHT = 2;
    
    private int state = STATE_FORWARD;
    private int pos = 1;
    
    void goToState(int st){
        if (st == STATE_FORWARD){
            if (state == STATE_IN_LEFT){
                if (pos != 0) --pos;
            }
            if (state == STATE_IN_RIGHT){
                if (pos != 2) ++pos;
            }
            state = STATE_FORWARD;
        } else {
            state = st;
        }
    }
    
    void Update() {
        if (bb(sr(), Input.acceleration.x)){
            --step;
        } else if (bb(Input.acceleration.x, sr())){
            ++step;
        } else {
            if(step < 0) ++step;
            else --step;
        }
        
        if (abs(step) < dS){
            goToState(STATE_FORWARD);
        } else if (step < 0){
            goToState(STATE_IN_RIGHT);
        } else {
            goToState(STATE_IN_LEFT);
        }
        
        
        if (pos == 0){
            Vector3 hz = transform.position;
            hz.x = bot.x;
            transform.position = hz;
        } else if (pos == 1){
            Vector3 hz = transform.position;
            hz.x = mid.x;
            transform.position = hz;
        } else {
            Vector3 hz = transform.position;
            hz.x = top.x;
            transform.position = hz;
        }
        
        val[lastVal] = Input.acceleration.x;
        updLastVal();
    }
}
