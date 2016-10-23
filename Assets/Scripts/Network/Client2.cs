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
			
	void Start()
	{
		try{
			client = new TcpClient("127.0.0.1", 2055);
            s = client.GetStream();
            sr = new StreamReader(s);
            sw = new StreamWriter(s);
            sw.AutoFlush = true;
		} catch (Exception e){
            Debug.Log("GGGGGGGGGGGGGGGGGGGGGGGG " + e.Message);
        }
	}
	
    void Update(){
		V3 v = new V3(transform.position);
		formatter.Serialize(s, v);
    }

/*
    void SendSpam(){
        TcpClient client = null;
        try{
            //Debug.Log("QQQQ1111");
            client = new TcpClient("127.0.0.1", 2055);
            //Debug.Log("QQQQ222");
            Stream s = client.GetStream();
            StreamReader sr = new StreamReader(s);
            StreamWriter sw = new StreamWriter(s);
            sw.AutoFlush = true;
            //Debug.Log("QQQQ3333");
            
            Vector3 vect = Vector3.zero;
            //sw.WriteLine("AAA");
            
            V3 v = new V3(vect);
            formatter.Serialize(s, v);
            V3 t = (V3)formatter.Deserialize(s);
            Vector3 temp = t.getVec();
            Debug.Log("Server say " + temp);
            
            s.Close();
        } catch (Exception e){
            Debug.Log("GGGGGGGGGGGGGGGGGGGGGGGG " + e.Message);
        } finally{
            // code in finally block is guranteed 
            // to execute irrespective of 
            // whether any exception occurs or does 
            // not occur in the try block
            if (client != null)
                client.Close();
        } 
    }*/
	
	void Stop(){
		if (client != null)
			client.Close();
	}
}