using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;

class Client2 : MonoBehaviour
{
    BinaryFormatter formatter = new BinaryFormatter();
	TcpClient client = null;
	Stream s = null;
	StreamReader sr = null;
	StreamWriter sw = null;

    public static string ip = "-1";

    Level level;
	private volatile bool isNeedSend = true;
	private bool inGame = false;
			
	void Start()
    {
        //Этот скрипт прикреплён к префабу робота, но он не активен.

        //Лучше прямую ссылку задаать через editor, но так проще прикреплять, это временно
        level = GameObject.Find("Plane").GetComponent<Level>();

        if (ip == "-1") return;

		Thread t = new Thread(new ThreadStart(Client));
		t.Start();
	}
	
	void Client()
	{
		try{
			client = new TcpClient(ip, 8865);
            s = client.GetStream();

            //сообщаем о готовости
			bool isOk = true;
			while(isOk)
			{
				Debug.Log("111");
				isOk = sRead();
				
				int counter = 10;
				while(!isNeedSend && counter != 0)
				{
					--counter;
					Thread.Sleep(10);
				}
				
				sWrite();
				formatter.Serialize(s, new Mark(Mark.WAIT_SWAP));
				
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
			Mark mark = (Mark)formatter.Deserialize(s);
			switch (mark.getType())
			{
				case Mark.WAIT_HZ:
				case Mark.WAIT_EXIT:{
					isOk = false;
				}; break;
				case Mark.WAIT_VECTOR3: {
					V3 t = (V3)formatter.Deserialize(s);
				}; break;
				case Mark.WAIT_SWAP: {
					swap = true;
				}; break;
				case Mark.WAIT_START: {
					inGame = true;
				}; break;
				case Mark.WAIT_RESTART: {
					inGame = true; //Хз, может не будет работать (см update)
				}; break;
				default: {
					Debug.Log("Incorrect mark type = " + mark.getType());
				}; break;
			}
		}
		return isOk;
	}
	
	volatile bool needSendWin = false;
	void SendWin()
	{
		needSendWin = true;
	}
	
	volatile bool needSendGameover = false;
	void SendGameover()
	{
		needSendGameover = true;
	}
	
	void sWrite()
	{
		formatter.Serialize(s, new Mark(Mark.WAIT_VECTOR3));
		formatter.Serialize(s, pos);
		
		if(needSendWin)
		{
			needSendWin = false;
			formatter.Serialize(s, new Mark(Mark.WAIT_WIN));
		}
			
		if(needSendGameover)
		{
			needSendGameover = false;
			formatter.Serialize(s, new Mark(Mark.WAIT_GAMEOVER));
		}
	}
	
	volatile V3 pos = new V3(Vector3.zero);
	
    void Update(){
		isNeedSend = true;
		if (inGame) 
		{
			inGame = false;
			level.NotifySecondPlayerConnected();
		}
		pos = new V3(transform.position);
    }
	
	void Stop(){
		if (client != null)
			client.Close();
	}
}