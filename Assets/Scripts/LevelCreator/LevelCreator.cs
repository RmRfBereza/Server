using UnityEngine;
using System.Collections;

public class LevelCreator : MonoBehaviour {

    public GameObject Cell;

    public int Rows;
    public int Columns;

    private GameObject content;
    private GameObject[,] cells;

	// Use this for initialization
	void Start () {
        content = GameObject.Find("Content");
        CreateCells();
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

        print(obj.ToString().Replace("\"", "\\\""));
        
    }
}
