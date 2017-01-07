using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;

class Client2 : PoluClientPoluServer
{
    static BinaryFormatter formatter = new BinaryFormatter();
    TcpClient client = null;
    static Stream stream = null;
    Thread thread = null;

    
    public static string ip = "-1";

    Level level;
    private volatile bool isNeedSend = true;
    private bool inGame = false;
            
    void Start()
    {
        level = GameObject.Find("Plane").GetComponent<Level>();

        if (ip == "-1")
        {        
            level.RestartGame();
            return;
        }
        thread = new Thread(new ThreadStart(Client));
        thread.Start();
    }
    
    void Client()
    {
        try{
            client = new TcpClient(ip, 8865);
            stream = client.GetStream();

            //niiauaai i aioiainoe
            bool isOk = true;
            while(isOk)
            {
                Debug.Log("111");
                isOk = sRead();
                
                int counter = 10;
                while(!isNeedSend && counter != 0)
                {
                    --counter;
                    Thread.Sleep(50);
                }
                
                sWrite();
                formatter.Serialize(stream, new Mark(Mark.WAIT_SWAP));
                
                isNeedSend = false;
            }
        } catch (Exception e){
            Debug.Log(e);
        }
    }
    
    bool sRead()
    {
        bool isOk = true;
        bool swap = false;
        while(!swap)
        {
            Mark mark = (Mark)formatter.Deserialize(stream);
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
                    //V3 t = (V3)formatter.Deserialize(s);
                }; break;
                case Mark.WAIT_SWAP: {
                    swap = true;
                }; break;
                case Mark.WAIT_START: {
                    inGame = true;
                }; break;
                case Mark.WAIT_RESTART: {
                    inGame = true; //Oc, ii?ao ia aoaao ?aaioaou (ni update)
                }; break;
                default: {
                    Debug.Log("Incorrect mark type = " + mark.getType());
                }; break;
            }
        }
        return isOk;
    }
    
    volatile bool needSendWin = false;
    public void SendWin()
    {
        needSendWin = true;
    }
    
    volatile bool needSendGameover = false;
    public void SendGameover()
    {
        needSendGameover = true;
    }
    
	public static void ForForeach(SyncData sd)
	{
		formatter.Serialize(stream, new Mark(Mark.WAIT_SYNC_DATA));
		formatter.Serialize(stream, sd);
	}
	
    void sWrite()
    {
        SendAll(ForForeach);
        
        if(needSendWin)
        {
            needSendWin = false;
            formatter.Serialize(stream, new Mark(Mark.WAIT_WIN));
        }
            
        if(needSendGameover)
        {
            needSendGameover = false;
            formatter.Serialize(stream, new Mark(Mark.WAIT_GAMEOVER));
        }
    }
    	
    void Update(){
        isNeedSend = true;
        if (inGame) 
        {
            inGame = false;
            level.RestartGame();
        }
    }
    
    void Stop(){
        if (client != null)
            client.Close();
    }

	void OnNewSdata(SyncData sd)
	{
	
	}
}