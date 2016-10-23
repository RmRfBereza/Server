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
	public static int WAIT_HZ		= 0;
	public static int WAIT_EXIT		= 1;
	public static int WAIT_VECTOR3 	= 2;
	
	private int type = WAIT_HZ;
	
	public int getType()
	{
		return type;
	}
	
	public void setType(int val)
	{
		type = val;
	}
}