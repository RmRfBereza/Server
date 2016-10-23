using UnityEngine;
using System.Collections;

public class AngleControllers : MonoBehaviour 
{
    public delegate void LeftRotateAction();
    public delegate void RightRotateAction();
    //public delegate void UpRotateAction();
    //public delegate void DownRotateAction();
    public static LeftRotateAction  OnLeftRotate;
    public static RightRotateAction OnRightRotate;
    //public static UpRotateAction    OnUpRotate;
    //public static DownRotateAction  OnDownRotate;
    
    private float angle = 0.0F;
    public enum Directions
    {
        Up = 0, Down = 180, Left = 270, Right = 90
    }
	private Directions curDirect = Directions.Up;
	
	void SetDir(Directions dir)
	{
		do {
			if (curDirect == Directions.Up && dir == Directions.Right)
			{
				OnRightRotate();
				break;
			}
			if (curDirect == Directions.Up && dir == Directions.Left)
			{
				OnLeftRotate();
				break;
			}
			if (curDirect == Directions.Up && dir == Directions.Down)
			{
				OnLeftRotate();
				OnLeftRotate();
				break;
			}
			
			if (curDirect == Directions.Down && dir == Directions.Right)
			{
				OnLeftRotate();
				break;
			}
			if (curDirect == Directions.Down && dir == Directions.Left)
			{
				OnRightRotate();
				break;
			}
			if (curDirect == Directions.Down && dir == Directions.Up)
			{
				OnLeftRotate();
				OnLeftRotate();
				break;
			}
			
			if (curDirect == Directions.Left && dir == Directions.Up)
			{
				OnRightRotate();
				break;
			}
			if (curDirect == Directions.Left && dir == Directions.Down)
			{
				OnLeftRotate();
				break;
			}
			if (curDirect == Directions.Left && dir == Directions.Right)
			{
				OnLeftRotate();
				OnLeftRotate();
				break;
			}
			
			if (curDirect == Directions.Right && dir == Directions.Up)
			{
				OnLeftRotate();
				break;
			}
			if (curDirect == Directions.Right && dir == Directions.Down)
			{
				OnRightRotate();
				break;
			}
			if (curDirect == Directions.Right && dir == Directions.Left)
			{
				OnLeftRotate();
				OnLeftRotate();
				break;
			}
		} while(false);
		curDirect = dir;
	}
	
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
            SetDir(Directions.Up);
        } else
        if (angle < -45 && angle > -45-90){
            SetDir(Directions.Left);
        } else
        if (angle > 45 && angle < 45+90){
            SetDir(Directions.Right);
        } else {
            SetDir(Directions.Down);
        }
    }

    void Update()
    {
        updateGyro();
    }
}