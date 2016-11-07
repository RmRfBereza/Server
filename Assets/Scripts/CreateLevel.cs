using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateLevel : MonoBehaviour {

    public const int SegmentSize3d = 18;

    public static void CreateLevelFromJsonString(string jsonString, int segmentSize, Dictionary<string, GameObject> prefabDictionary)
    {
        Vector3 _position = Vector3.zero;
        Vector3 _rotation = Vector3.zero;

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
                var currentSegment = Instantiate(prefabDictionary[segmentParams["type"].str]);

                _rotation.Set(0f, segmentParams["angle"].n, 0f);
                currentSegment.transform.eulerAngles = _rotation;
                currentSegment.transform.position = _position;

                _position.z += segmentSize;
            }
            _position.x += segmentSize;
        }
    }
}
