using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {
    public const string SinglePlayerLevelsFileName = "SinglePlayerLevels.txt";
    public const string MultiPlayerLevelsFileName = "MultiPlayerLevels.txt";

    public static bool isSingle = false;

    private static string _defaultState;

    public static string DefaultState
    {
        get { return _defaultState; }
        set
        {
            _defaultState = value;
            if (InitState == null) InitState = value;
        }
    }

    public static string ErrorMessage { get; private set; }

    private static string InitState = null;

    public static void SetErrorMessage(string message)
    {
        InitState = "Error";
        ErrorMessage = message;
    }

    public static string GetAndResetInitState()
    {
        string state = InitState;
        InitState = _defaultState;
        return state;
    }
}
