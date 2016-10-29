using UnityEngine;
using System.Collections;

public class LogoAnimationManager : MonoBehaviour {

    private static bool _firstTime = true;

    void Awake()
    {
        if (_firstTime)
        {
            GameObject.Find("LogoImageStatic").SetActive(false);
        } else
        {
            GameObject.Find("LogoImageDynamic").SetActive(false);
        }

        Animator anim = GetComponent<Animator>();
        //anim.SetBool("FirstTime", _firstTime);
        _firstTime = false;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
