using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Configuration;


class CcrMap
{
	private object thisLock = new object();
	private object ridgenLock = new object();
	private int lastrid = 0;
	
	Dictionary<string, SyncData> sdata = new Dictionary<string, SyncData>();
	
	public delegate void Frchsdt(SyncData sd);
	
	public void ForeachSdata(Frchsdt callback)
	{
		List<string> keys = new List<string> (sdata.Keys);
		foreach (string rid in keys)
		{
			SyncData sd = getSyncData(rid);
			if (sd != null) callback(sd);
		}
	}
	
	public string addSyncData(SyncData sd)
	{
		string newrid;
		lock (ridgenLock)
		{
			++lastrid;
			newrid = "kakoetog_" + lastrid;
		}
		setSyncData(newrid, sd);
		return newrid;
	}
	
	public void setSyncData(string rid, SyncData sd)
	{
		SyncData _sd = new SyncData(sd);
		_sd.rid = rid;
		lock (thisLock)
		{
			sdata.Remove(rid);
			sdata.Add(rid, _sd);
		}
	}
	
	public bool contain(string rid)
	{
		lock (thisLock)
		{
			return sdata.ContainsKey(rid);
		}
	}
	
	public SyncData getSyncData(string rid)
	{
		lock (thisLock)
		{
			if (sdata.ContainsKey(rid))
			{
				return new SyncData(sdata[rid]);
			}
			else
				return null;
		}
	}
}