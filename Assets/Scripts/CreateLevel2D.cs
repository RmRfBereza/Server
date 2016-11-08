using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateLevel2D : MonoBehaviour {

    public const int SegmentSize2d = 12;
    public GameObject TileIntersection;
    public GameObject TileTurn;
    public GameObject TileTTurn;
    public GameObject TileStraight;
    public GameObject TileNull;
    public GameObject TileDeadend;
    private Dictionary<string, GameObject> SegmentPrefabsDictionary = new Dictionary<string, GameObject>();

    void Start()
    {
        string jsonString;
        SegmentPrefabsDictionary.Add("straight", TileStraight);
        SegmentPrefabsDictionary.Add("turn", TileTurn);
        SegmentPrefabsDictionary.Add("Tturn", TileTTurn);
        SegmentPrefabsDictionary.Add("intersection", TileIntersection);
        SegmentPrefabsDictionary.Add("null", TileNull);
        jsonString = getJSONString();
        Create2DLevelFromJsonString(jsonString, SegmentSize2d, SegmentPrefabsDictionary);
    }

    private string getJSONString()
    {
        return "[[{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":0,\"type\":\"straight\"},{\"angle\":180,\"type\":\"turn\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}],[null,null,null,{\"angle\":90,\"type\":\"straight\"}]";
    }

    public static void Create2DLevelFromJsonString(string jsonString, int segmentSize, Dictionary<string, GameObject> prefabDictionary)
    {
        Vector3 _position = Vector3.zero;
        Vector3 _rotation = Vector3.zero;

        var levelMapping = new JSONObject(jsonString);

        foreach (JSONObject segmentRaw in levelMapping.list)
        {
            _position.x = 0;
            foreach (JSONObject segmentParams in segmentRaw.list)
            {
                if (segmentParams.type == JSONObject.Type.NULL)
                {
                    var nullSegment = Instantiate(prefabDictionary["null"]);
                    _rotation.Set(0f, 0f, 0f);
                    nullSegment.transform.eulerAngles = _rotation;
                    nullSegment.transform.position = _position;
                    _position.x += segmentSize;
                    continue;
                }
                var currentSegment = Instantiate(prefabDictionary[segmentParams["type"].str]);

                //_rotation.Set(0f, segmentParams["angle"].n, 0f);
                _rotation.Set(0f, 0f, segmentParams["angle"].n);
                currentSegment.transform.eulerAngles = _rotation;
                currentSegment.transform.position = _position;

                _position.x += segmentSize;
            }
            _position.y -= segmentSize;
        }
    }
}
