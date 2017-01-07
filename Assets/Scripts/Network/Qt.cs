using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

[Serializable]
class Qt
{
    public float x {get; set;}
    public float y {get; set;}
    public float z {get; set;}
    public float w {get; set;}
    
    public Qt(Quaternion q)
    {
        setQt(q);
    }

    public void setQt(Quaternion q)
    {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }

    public Quaternion getQt()
    {
        Quaternion q = new Quaternion(x,y,z,w);
        return q;
    }
}