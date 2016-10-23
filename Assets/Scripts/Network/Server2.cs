using UnityEngine;
using System;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using System.Configuration;


class Server2 : MonoBehaviour
{
	public volatile V3 nepos = new V3(Vector3.zero);
    public int port = 2055;

    BinaryFormatter formatter = new BinaryFormatter();
    static TcpListener listener;
    const int LIMIT = 1;
    
    void Start(){
        listener = new TcpListener(port);
        listener.Start();

        for(int i = 0; i < LIMIT;i++){
            Thread t = new Thread(new ThreadStart(Service));
            t.Start();
        }
    }

    void FixedUpdate() {
		Debug.Log(nepos.getVec());
		transform.position = nepos.getVec();
	}
	
	void Service(){
        while(true){
            Socket soc = listener.AcceptSocket();
            
            Debug.Log("Connected: " + soc.RemoteEndPoint);
            
            try{
                Stream s = new NetworkStream(soc); 
                StreamReader sr = new StreamReader(s);
                StreamWriter sw = new StreamWriter(s);
                sw.AutoFlush = true;

				bool isOk = true;
				while(isOk)
				{/*
					Mark mark = (Mark)formatter.Deserialize(s);
					switch (mark.getType())
					{
					case 0:
						case 1:
					isOk = false;
					break;
					case 2:*/
						V3 t = (V3)formatter.Deserialize(s);
						V3 n = new V3(t.getVec());
						n.y = n.z;
						nepos = n;
						Debug.Log("UUUUUUUUU " + t.getVec());/*
						break;
					default:
					isOk = false;
					break;
					}*/
				}

                s.Close();
            } catch(Exception e){
                Debug.Log(e.Message);
            }

            Debug.Log("Disconnected: " + soc.RemoteEndPoint);

            soc.Close();
        }
    }
}
