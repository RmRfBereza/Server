using UnityEngine;
using System.Collections;

public class AngleControllers : MonoBehaviour 
{
    public delegate void LeftRotateAction();
    public delegate void RightRotateAction();
    public delegate void UpRotateAction();
    public delegate void DownRotateAction();
    public static LeftRotateAction  OnLeftRotate;
    public static RightRotateAction OnRightRotate;
    public static UpRotateAction    OnUpRotate;
    public static DownRotateAction  OnDownRotate;
    
    private float angle = 0.0F;
    
    void Start()
    {
        Input.gyro.enabled = true;
    }
    
    void updateGyro()
    {
        angle += -Input.gyro.rotationRateUnbiased.y;
        Debug.Log("QQQQQQ CCCCCC " + angle);

        
        while(angle > 180)
            angle -= 360;
        while(angle < -180)
            angle += 360;
            
        if (angle > -45 && angle < 45){
            OnUpRotate();
        } else
        if (angle < -45 && angle > -45-90){
            OnLeftRotate();
        } else
        if (angle > 45 && angle < 45+90){
            OnRightRotate();
        } else {
            OnDownRotate();
        }
    }

    void Update()
    {
        updateGyro();
    }
}