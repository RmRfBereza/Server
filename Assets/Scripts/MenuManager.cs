using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    private const string MenuStatesFileName = "MenuStates";
    private const string MenuObjectsName = "MenuObjects";
    private const string InitStateName = "InitState";

    private GameObject[] _menuObjects;
    private JSONObject _menuStates;

    public void SetState(string state)
    {
        var activeInfo = _menuStates[state].list;
        if (activeInfo.Count != _menuObjects.Length)
        {
            throw new Exception($"Length of state {state} is not equal to number of menu objects");
        }

        for (int i = 0; i < _menuObjects.Length; ++i)
        {
            if (!activeInfo[i].IsBool)
            {
                throw new Exception($"Parameter at index {i} of state {state} is not boolean, it's {_menuStates[i].type}");
            }

            _menuObjects[i].SetActive(activeInfo[i].b);
        }
    }

    void Awake()
    {
        ReadMenuStates();
    }

	// Use this for initialization
	void Start() {
	    FillMenuObjects();
	    SetInitState();
	    SetState(Config.GetAndResetInitState());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ReadMenuStates()
    {
        _menuStates = new JSONObject(Resources.Load<TextAsset>(MenuStatesFileName).text);

    }

    private void FillMenuObjects()
    {
        _menuObjects = new GameObject[ _menuStates[MenuObjectsName].list.Count];

        int i = 0;
        foreach (var gameObjectName in _menuStates[MenuObjectsName].list)
        {
            GameObject menuObject = GameObject.Find(gameObjectName.str);

            if (menuObject == null)
            {
                throw new Exception($"Can't find menu object with name {gameObjectName}");
            }

            _menuObjects[i] = menuObject;
            ++i;
        }
    }

    private void SetInitState()
    {
        Config.DefaultState = _menuStates[InitStateName].str;
    }
}
