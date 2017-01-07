using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;


class PoluClientPoluServer : MonoBehaviour
{
	private CcrMap from = new CcrMap();
	private CcrMap to = new CcrMap();
	
	public string addToSend(SyncData sd)
	{
		return from.addSyncData(sd);
	}
	
	public void setToSend(string rid, SyncData sd)
	{
		from.setSyncData(rid, sd);
	}
	
	public SyncData getSyncData(string rid)
	{
		return to.getSyncData(rid);
	}
	
	protected void addBecauseGet(SyncData sd)
	{
		//Debug.Log("Rid = " + sd.rid);
		if (!to.contain(sd.rid))
			OnNewSdata(sd);
		to.setSyncData(sd.rid, sd);
	}
	
	//to Overwrite
	protected virtual void OnNewSdata(SyncData sd)
	{
		Debug.Log("||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
	}
	
	protected void SendAll(CcrMap.Frchsdt callback)
	{
		from.ForeachSdata(callback);
	}
}