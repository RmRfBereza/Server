using UnityEngine;
using System.Collections;

public class AngleControllers : MonoBehaviour 
{
    public int dAngle = 90;
    
    public delegate void LeftRotateAction();
    public delegate void RightRotateAction();
    public static LeftRotateAction OnLeftRotate;
    public static RightRotateAction OnRightRotate;
    
    private float angle = 0.0F;
    
    void Start()
    {
        Input.gyro.enabled = true;
    }
    
    int pos = 0;
    int lastPos = 0;
    
    void updateGyro()
    {
        Quaternion stRot = transform.rotation;
        Quaternion rotation = Quaternion.LookRotation(Input.acceleration.normalized);
        angle += rotation.x;
        Debug.Log("AAAA " + angle);
        
        int aa = (int)(angle*10);
        while(aa > 180)
            aa -= 180;
        while(aa < -180)
            aa += 180;
        if (aa > -45 && aa < 45){
            //lastPos = pos;
            pos = 0;
        } else
        if (aa < -45 && aa > -45-90){
            //lastPos = pos;
            pos = 3;
        } else
        if (aa > 45 && aa < 45+90){
            //lastPos = pos;
            pos = 1;
        } else {
            //lastPos = pos;
            pos = 2;
        }
            
        if(pos > lastPos || (lastPos == 3 && pos == 0)){
            OnRightRotate();
        } else if(pos < lastPos || (lastPos == 0 && pos == 3))
            OnLeftRotate();
            
        lastPos = pos;
    }

    void Update()
    {
        updateGyro();
    }
}