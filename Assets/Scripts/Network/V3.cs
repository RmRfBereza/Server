using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

[Serializable]
class V3
{
    public float x {get; set;}
    public float y {get; set;}
    public float z {get; set;}
    
    public V3(Vector3 vec)
    {
        setVec(vec);
    }

    public void setVec(Vector3 vec)
    {
        x = vec.x;
	y = vec.y;
	z = vec.z;
    }

    public Vector3 getVec()
    {
        Vector3 vec = Vector3.zero;
        vec.x = x;
        vec.y = y;
        vec.z = z;
        return vec;
    }
}
