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

    public static string ip;

    Level level;
			
	void Start()
    {
        //Этот скрипт прикреплён к префабу робота, но он не активен.

        //Лучше прямую ссылку задаать через editor, но так проще прикреплять, это временно
        Level level = GameObject.Find("Plane").GetComponent<Level>();

        if (ip == "-1") return;

		try{
			client = new TcpClient(ip, 8865);
            s = client.GetStream();
            sr = new StreamReader(s);
            sw = new StreamWriter(s);
            sw.AutoFlush = true;

            //сообщаем о готовости
            level.NotifySecondPlayerConnected();
		} catch (Exception e){
            Debug.Log("GGGGGGGGGGGGGGGGGGGGGGGG " + e.Message);
        }
	}
	
    void Update(){
        if (s != null)
        {
            V3 v = new V3(transform.position);
            formatter.Serialize(s, v);
        }
    }
	
	void Stop(){
		if (client != null)
			client.Close();
	}
}