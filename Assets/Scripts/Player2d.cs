using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;


class Player2d : MonoBehaviour
{
	public Server2 server = null;
	string rid = null;
	
	void Start()
	{
		server = GameObject.Find("Server2").GetComponent<Server2>();
	}
	
	public void registration(SyncData sd)
	{
		rid = sd.rid != null ? sd.rid : null;
		Debug.Log("Set rid ========================================================= " + rid);
	}
	
	public void updatePos(SyncData sd)
	{
		if (sd == null)
			return;
		if (sd.vec != null)
		{
			var pos = sd.vec.getVec();
			var newPos = Vector3.zero;
		   //print(pos);
			newPos.x = pos.z*2/3;
			newPos.y = -pos.x*2/3;
			transform.position = newPos;
		}
		
		if (sd.qt != null)
			transform.rotation = sd.qt.getQt();
	}
	
	void Update()
	{
		if (rid != null)
			updatePos(server.getSyncData(rid));
	}
}