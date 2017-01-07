using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

[Serializable]
class Mark
{
    public const int WAIT_HZ        = 0;
    public const int WAIT_EXIT      = 1;
    public const int WAIT_SWAP      = 2;
    public const int WAIT_VECTOR3   = 3;
    public const int WAIT_START     = 4;
    public const int WAIT_RESTART   = 5;
    public const int WAIT_WIN       = 6;
    public const int WAIT_GAMEOVER  = 7;
    public const int WAIT_SYNC_DATA = 8;
    
    private int type = WAIT_HZ;
    
    public Mark(int _type)
    {
        type = _type;
    }
    
    public int getType()
    {
        return type;
    }
    
    public void setType(int val)
    {
        type = val;
    }
}