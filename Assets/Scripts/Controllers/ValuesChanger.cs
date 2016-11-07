using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ValuesChanger : MonoBehaviour {

    public Slider dSslider;
    public Text dSsliderText;
    public Slider dAccSlider;
    public Text dAccSliderText;

    // Use this for initialization
    void Start () {
        dSslider.value = XController.dS;
        dAccSlider.value = XController.dAcc;
    }
	
	// Update is called once per frame
	void Update () {
        ChangeDevation();
        ChangeMinAcceleration();
	}

    public void ChangeDevation()
    {
        XController.dS = Mathf.RoundToInt(dSslider.value);
        dSsliderText.text = XController.dS.ToString();
    }

    public void ChangeMinAcceleration()
    {
        XController.dAcc = dAccSlider.value;
        dAccSliderText.text = XController.dAcc.ToString();
    }
}
