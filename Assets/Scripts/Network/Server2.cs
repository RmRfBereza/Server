using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;


class Server2 : PoluClientPoluServer
{
	public volatile V3 nepos = new V3(Vector3.zero);
    public int port = 8865;
	public string my_ip { get; set; }
	public int mask { get; set; }
	static Stream stream = null;
    Socket socket = null;
    Thread thread = null;


	
    static BinaryFormatter formatter = new BinaryFormatter();
    static TcpListener listener;    
    const int LIMIT = 1;
    public static volatile string myIp = "undefined";

    private CreateLevel2D level;
	public Player2d player;	
    void Start()
    {
        level = GameObject.Find("MapManager").GetComponent<CreateLevel2D>();
		player = GameObject.Find("Player").GetComponent<Player2d>();
		
		try {
			IPAddress my_ip = null;
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					my_ip = ip;
					mask = 8;
					var firstOctet = ip.GetAddressBytes()[0];
					for (int i = 0; i < 3; ++i)
					{
						if ((firstOctet & (1 << (7 - i))) == 0)
							break;
						mask += 8;
					}
					break;
				}
			}
			
			listener = new TcpListener(my_ip, port);
			listener.Start();
			myIp = this.my_ip = IPAddress.Parse(((IPEndPoint)listener.LocalEndpoint).Address.ToString()).ToString();
		} catch(Exception e){
			myIp = e.Message;
		}

		thread = new Thread(new ThreadStart(Service));
		thread.Start();
    }

	public volatile bool isNeedStart = false;
    public void StartGame()
    {
        isNeedStart = true;
    }
	
	public volatile bool isNeedRestart = false;
    public void RestartGame()
    {
        isNeedRestart = true;
    }

//    void OnGUI () {
//		//GUI.Label(new Rect(0, 50, 100, 100), my_ip);
//    }
	
//    void FixedUpdate() {
//        var pos = nepos.getVec();
//        var newPos = Vector3.zero;
//        //print(pos);
//        newPos.x = pos.z*2/3;
//        newPos.y = -pos.x*2/3;
//        transform.position = newPos;
//	}
	
	void Service(){
        while(true){
            socket = listener.AcceptSocket();
            
            Debug.Log("Connected: " + socket.RemoteEndPoint);
            level.NotifySecondPlayerConnected();
            try{
                stream = new NetworkStream(socket); 

				bool isOk = true;
				while(isOk)
				{
					sWrite();
					formatter.Serialize(stream, new Mark(Mark.WAIT_SWAP));
					isOk = sRead();
				}

                stream.Close();
            } catch(Exception e){
                Debug.Log(e.Message);
            }

            Debug.Log("Disconnected: " + socket.RemoteEndPoint);

            socket.Close();
        }
    }
	
	volatile bool isWin = false;
	volatile bool isGameover = false;
	
	void OnWin()
	{
	    level.NotifyGameWon();
	}
	
	void OnGameover()
	{
	    level.NotifyGameOver();
	}
	
	void Update(){
		if(isWin)
		{
			isWin = false;
			OnWin();
		}
		if(isGameover)
		{
			isGameover = false;
			OnGameover();
		}
	}
	
	bool sRead()
	{
		bool isOk = true;
		bool swap = false;
		while(!swap)
		{
			Mark mark = (Mark)formatter.Deserialize(stream);
			//Debug.Log("Type = " + mark.getType());
			switch (mark.getType())
			{
				case Mark.WAIT_HZ:
				case Mark.WAIT_EXIT:{
					isOk = false;
				}; break;
				case Mark.WAIT_SYNC_DATA: {
					SyncData sd = (SyncData)formatter.Deserialize(stream);
					addBecauseGet(sd);
				}; break;
				case Mark.WAIT_VECTOR3: {
					V3 t = (V3)formatter.Deserialize(stream);
					V3 n = new V3(t.getVec());
					//n.y = n.z;
					//n.z = 0;
					nepos = n;
				}; break;
				case Mark.WAIT_SWAP: {
					swap = true;
				}; break;
				case Mark.WAIT_WIN: {
					isWin = true;
				}; break;
				case Mark.WAIT_GAMEOVER: {
					isGameover = true;
				}; break;
				default:{
					Debug.Log("Incorrect mark type = " + mark.getType());
				}; break;
			}
		}
		return isOk;
	}
	
	public static void ForForeach(SyncData sd)
	{
		formatter.Serialize(stream, new Mark(Mark.WAIT_SYNC_DATA));
		formatter.Serialize(stream, sd);
	}
	
	void sWrite()
	{
		SendAll(ForForeach);
		if (isNeedStart)
		{
			formatter.Serialize(stream, new Mark(Mark.WAIT_START));
			isNeedStart = false;
		}
		if (isNeedRestart)
		{
			formatter.Serialize(stream, new Mark(Mark.WAIT_RESTART));
			isNeedRestart = false;
		}
	}

    void OnDestroy() {        
        if (socket != null)
        {
            socket.Close();
        }
        if (stream != null)
        {
            stream.Close();
        }
        if (thread != null)
        {
            thread.Interrupt();
        }
    }
	

	
	protected override void OnNewSdata(SyncData sd)
	{
		Debug.Log("//////////////////////////////////////////////////////////////////////////");
		if (sd.info == "player" && player != null)
			player.registration(sd);
	}
}