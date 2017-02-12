using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class LevelManager {

    private static LevelManager instance = null;

    private string[] Levels = null;
    private int currentLevel;

    public static LevelManager getInstance()
    {
        if (instance == null) instance = new LevelManager();
        return instance;
    }

    public void prepareForSingle()
    {
        clearParameters();
        readLevels(Config.SinglePlayerLevelsFileName);
    }

    public void prepareForMulti()
    {
        clearParameters();
        readLevels(Config.MultiPlayerLevelsFileName);
    }

    public bool hasNextLevel()
    {
        return currentLevel + 1 < Levels.Length;
    }

    public string getLevelAndInitIfNecessary()
    {
        return getLevelAtAndInitIfNecessary(currentLevel);
    }

    public string getLevelAtAndInitIfNecessary(int index)
    {
        if (Levels == null)
        {
            if (Config.isSingle)
            {
                prepareForSingle();
            }
            else
            {
                prepareForMulti();
            }
        }
        return Levels[index];
    }

    public string getLevel()
    {
        return Levels[currentLevel];
    }

    public string getAndIncrementLevel()
    {
        return Levels[currentLevel++];
    }

    public bool incrementAndCheckLevel()
    {
        return ++currentLevel < Levels.Length;
    }

    public void addLevelMulti(string level)
    {
        addLevel(level, Config.MultiPlayerLevelsFileName);
    }

    public void addLevelSingle(string level)
    {
        addLevel(level, Config.SinglePlayerLevelsFileName);
    }

    private void addLevel(string level, string file)
    {
        var configPass = Path.Combine(Directory.GetCurrentDirectory(),
                                           "Assets" + Path.DirectorySeparatorChar +
                                           "Resources"
                                           + Path.DirectorySeparatorChar + file);

        FileStream fcreate = File.Open(configPass, FileMode.Append);
        var stream = new StreamWriter(fcreate);
        stream.WriteLine(level);
        stream.Close();
        fcreate.Close();
    }

    private void readLevels(string filename)
    {
        Debug.Log(filename);
        var levelsText = Resources.Load(filename.Split('.')[0]) as TextAsset;
        Debug.Log(levelsText);
        Levels = levelsText.text.Split('\n');
    }

    private void clearParameters()
    {
        currentLevel = 0;
    }
}
