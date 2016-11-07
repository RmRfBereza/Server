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
    public int port = 8865;
	public string my_ip { get; set; }
	public int mask { get; set; }
	
    BinaryFormatter formatter = new BinaryFormatter();
    static TcpListener listener;
    const int LIMIT = 1;
    volatile string myIp = "undefined";

	
    void Start(){
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
		this.my_ip = IPAddress.Parse(((IPEndPoint)listener.LocalEndpoint).Address.ToString()).ToString();

        for(int i = 0; i < LIMIT;i++){
            Thread t = new Thread(new ThreadStart(Service));
            t.Start();
        }
    }

	
	void OnGUI () {
		GUI.Label(new Rect(0, 50, 100, 100), my_ip);
    }
	
    void FixedUpdate() {
        var pos = nepos.getVec();
        var newPos = Vector3.zero;
        print(pos);
        newPos.x = pos.z*2/3;
        newPos.y = -pos.x*2/3;
        transform.position = newPos;
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
				{
					V3 t = (V3)formatter.Deserialize(s);
					V3 n = new V3(t.getVec());
					//n.y = n.z;
					//n.z = 0;
					nepos = n;
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
