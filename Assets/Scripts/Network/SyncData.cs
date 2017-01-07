using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

[Serializable]
class SyncData
{
    public string rid {get;set;}
	public string info {get;set;}
    public Qt qt {get;set;}
    public V3 vec {get;set;}
	
	public SyncData(string _info, V3 _vec, Qt _qt)
	{
		qt = _qt;
		vec = _vec;
		rid = null;
		info = _info;
	}
	
	public SyncData(SyncData obj)
	{
		qt = (obj.qt != null) ? new Qt(obj.qt.getQt()) : null;
		vec = (obj.vec != null) ? new V3(obj.vec.getVec()) : null;
		rid = (obj.rid != null) ? String.Copy(obj.rid) : null;
		info = (obj.info != null) ? String.Copy(obj.info) : null;
	}
}