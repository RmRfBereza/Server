using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateLevel : MonoBehaviour {

    public const int SegmentSize3d = 18;

    public static List<GameObject> CreateLevelFromJsonString(string jsonString, int segmentSize, Dictionary<string, GameObject> prefabDictionary)
    {
        List<GameObject> levelSegments = new List<GameObject>();

        Vector3 _position = Vector3.zero;
        Vector3 _rotation = Vector3.zero;

        print(jsonString);

        var levelMapping = new JSONObject(jsonString);

        foreach (JSONObject segmentRaw in levelMapping.list)
        {
            _position.z = 0;
            foreach (JSONObject segmentParams in segmentRaw.list)
            {
                if (segmentParams.type == JSONObject.Type.NULL)
                {
                    _position.z += segmentSize;
                    continue;
                }
                print(segmentParams);
                print(segmentParams["type"].str);
                var currentSegment = Instantiate(prefabDictionary[segmentParams["type"].str]);

                levelSegments.Add(currentSegment);

                _rotation.Set(0f, -segmentParams["angle"].n, 0f);
                currentSegment.transform.eulerAngles = _rotation;
                currentSegment.transform.position = _position;

                if (segmentParams["type"].str == Level.StartSegmentName) {
                    Level.StartPosition = _position;
                    _rotation.y += 90 + 180;
                    Level.StartRotation = _rotation;
                }

                _position.z += segmentSize;
            }
            _position.x += segmentSize;
        }

        return levelSegments;
    }
}
