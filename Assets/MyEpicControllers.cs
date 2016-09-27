using UnityEngine;
using System.Collections;

public class MyEpicControllers : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    public float speed = 10.0F;
    public Vector3 tipoJump = Vector3.zero;
    public Vector3 normalPos = Vector3.zero;
    public float yAcc = 1.0F;
    void Update() {
        Debug.Log("Y-ACC = " + Input.acceleration.y);
        if (Input.acceleration.y > yAcc){
            transform.position = tipoJump;
        } else {
            transform.position = normalPos;
        }
    }
}
