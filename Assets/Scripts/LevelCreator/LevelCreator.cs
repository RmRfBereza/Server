using UnityEngine;
using System.Collections;

public class LevelCreator : MonoBehaviour {

    public GameObject Cell;
    public GameObject SegmentStorage;

    public int Rows;
    public int Columns;

    private GameObject content;
    private GameObject[,] cells;

    private Segment3dStorageHandler _storage;

	// Use this for initialization
	void Start () {
        content = GameObject.Find("Content");
        CreateCells();
	    _storage = Instantiate(SegmentStorage).GetComponent<Segment3dStorageHandler>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CreateCells()
    {
        cells = new GameObject[Rows, Columns];
        for (int i = 0; i < Rows; ++i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                cells[i, j] = Instantiate(Cell);
                cells[i, j].transform.parent = content.transform;
            }
        }
    }

    public void CreateLevelFromCurrentState()
    {
        print(CreateJsonLevel());
    }

    public void AddLevelSingle()
    {
        LevelManager.getInstance().addLevelSingle(CreateJsonLevel());
    }

    public void AddLevelMulti()
    {
        LevelManager.getInstance().addLevelMulti(CreateJsonLevel());
    }

    public void Create3d()
    {
        var level = new GameObject("Level");
        CreateLevel.CreateLevelFromJsonString(CreateJsonLevel(), CreateLevel.SegmentSize3d, _storage.SegmentPrefabsDictionary, level.transform);
        print("The Level created in hierachy. Save it by making it a prefab.");
    }

    public void TransformSinglePlayerLevelsTo3D()
    {
        LevelManager.getInstance().prepareForSingle();

        int i = 1;
        while (LevelManager.getInstance().hasNextLevel())
        {
            var level = new GameObject("Level" + i++);
            level.tag = "Level";
            //level.transform.Translate(Vector3.up * 30 * i);
            CreateLevel.CreateLevelFromJsonString(LevelManager.getInstance().getAndIncrementLevel(),
                CreateLevel.SegmentSize3d, _storage.SegmentPrefabsDictionary, level.transform);
        }
    }

    private string CreateJsonLevel()
    {
        JSONObject obj = new JSONObject(JSONObject.Type.ARRAY);
        for (int i = 0; i < Rows; ++i)
        {
            obj.Add(new JSONObject(JSONObject.Type.ARRAY));
            for (int j = 0; j < Columns; ++j)
            {
                var handler = cells[i, j].GetComponent<LevelCellButtonHandler>();
                if (handler.Name == "null")
                {
                    obj[i].Add(new JSONObject(JSONObject.Type.NULL));
                }
                else
                {
                    obj[i].Add(new JSONObject());
                    obj[i][j].AddField("angle", handler.rotation);
                    obj[i][j].AddField("type", handler.Name);
                }
            }
        }
        
        var strings =  obj.ToString().Split('"');
        string result = strings[0];
        for (int i = 1; i < strings.Length; ++i)
        {
            if (i % 2 != 0)
            {
                result += "\\\\";
            }
            result += "\"" + strings[i];
        }
        return result;
    }
}
