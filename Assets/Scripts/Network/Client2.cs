using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

class Client2 : MonoBehaviour
{
    BinaryFormatter formatter = new BinaryFormatter();
    TcpClient client = null;
    Stream s = null;

    public static string ip = "-1";
    public const string ipSkip = "s";
    public static bool enableBase64 = true;

    Level level;
    private volatile bool isNeedSend = true;
    private volatile bool isNeedExit = false;
    private bool inGame = false;
    Thread t;
            
    void Start()
    {
        if (Config.isSingle)
        {
            this.enabled = false;
            return;
        }

        //���� ������ ��������� � ������� ������, �� �� �� �������.

        //����� ������ ������ ������� ����� editor, �� ��� ����� �����������, ��� ��������
        level = GameObject.Find("Plane").GetComponent<Level>();

        if (ip == "s")
        {
            level.RestartGame();
            return;
        }
        if (ip == "-1") return;

        t = new Thread(new ThreadStart(Client));
        t.Start();
    }
    
    void OnDestroy(){
        t.Interrupt();
    }
    
    void Client()
    {
        try{
            if (enableBase64)
            {
                var base64EncodedBytes = System.Convert.FromBase64String(ip + "==");
                var ipaddres = new IPAddress(base64EncodedBytes);
                ip = ipaddres.ToString();
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                Debug.Log(ipaddres.ToString());
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            }
            
            client = new TcpClient(ip, 8865);
            s = client.GetStream();

            //�������� � ���������
            bool isOk = true;
            while(isOk)
            {
                //Debug.Log("111");
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
        } finally {
            if (client != null)
                client.Close();
            isNeedExit = true;
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
                    //V3 t = (V3)formatter.Deserialize(s);
                }; break;
                case Mark.WAIT_SWAP: {
                    swap = true;
                }; break;
                case Mark.WAIT_START: {
                    inGame = true;
                }; break;
                case Mark.WAIT_RESTART: {
                    inGame = true; //��, ����� �� ����� �������� (�� update)
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
            level.RestartGame();
        }
        pos = new V3(transform.position);
        if (isNeedExit)
            SceneManager.LoadScene(0);
    }
    
    void Stop(){
        if (client != null)
            client.Close();
    }
}