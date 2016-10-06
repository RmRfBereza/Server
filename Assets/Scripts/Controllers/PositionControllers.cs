using UnityEngine;
using System.Collections;

public class PositionControllers : MonoBehaviour 
{
    public Vector3 mid = Vector3.zero;  // Стандартная позиция  
    
    /*
     * YActions
     */
    public Vector3 top = Vector3.zero;  // Верхняя позиция
    public Vector3 bot = Vector3.zero;  // Нижняя позиция
    public int timeYAction = 60;
    
    void OnYSpace()
    {
        OnStartYAction();
        Vector3 hz = transform.position;
        hz.y = top.y;
        transform.position = hz;
    }

    void OnYCtrl()
    {
        OnStartYAction();
        Vector3 hz = transform.position;
        hz.y = bot.y;
        transform.position = hz;
    }
    
    void OnStartYAction()
    {
        unsubscribeYAction();
        counterYAction = 0;
    }
    
    void OnEndYAction()
    {
        subscribeYAction();
        counterYAction = -1;
        Vector3 hz = transform.position;
        hz.y = mid.y;
        transform.position = hz;
    }
    
    void subscribeYAction()
    {
        YControllers.OnYSpace += OnYSpace;
        YControllers.OnYCtrl  += OnYCtrl;
    }
    
    void unsubscribeYAction()
    {
        YControllers.OnYSpace -= OnYSpace;
        YControllers.OnYCtrl  -= OnYCtrl;
    }
    
    // TODO переделать на паттерн метод обновления
    //      или на нормальный таймер
    //      или как-то еще вызывать колбек - OnEndYAction();
    private int counterYAction = -1;
    void updYAction()
    {
        counterYAction = counterYAction != -1 ? counterYAction + 1 : -1;
        if (counterYAction == timeYAction)
        {
            OnEndYAction();
        }
    }
    
    /*
     * XActions
     */
    public Vector3 left = Vector3.zero;  // Левая позиция
    public Vector3 right = Vector3.zero;  // Правая позиция
    public int timeXAction = 60;
    
    private const int POS_LEFT   = 0;
    private const int POS_CENTER = 1;
    private const int POS_RIGHT  = 2;
    private int pos = POS_CENTER;
    
    void GoToXLeft()
    {
        Vector3 hz = transform.position;
        hz.x = left.x;
        transform.position = hz;
    }
   
    void GoToXCenter()
    {
        Vector3 hz = transform.position;
        hz.x = mid.x;
        transform.position = hz;
    }

    void GoToXRight()
    {
        Vector3 hz = transform.position;
        hz.x = right.x;
        transform.position = hz;
    }
    
    void updateXPos()
    {
        if (pos == POS_LEFT)
            GoToXLeft();
        else if (pos == POS_RIGHT)
            GoToXRight();
        else 
            GoToXCenter();
    }
    
    void OnXLeft()
    {
        OnStartXAction();
        pos = pos != POS_LEFT ? pos - 1 : POS_LEFT;
        updateXPos();
    }

    void OnXRight()
    {
        OnStartXAction();
        pos = pos != POS_RIGHT ? pos + 1 : POS_RIGHT;
        updateXPos();
    }
    
    void OnStartXAction()
    {
        unsubscribeXAction();
        counterXAction = 0;
    }
    
    void OnEndXAction()
    {
        subscribeXAction();
        counterXAction = -1;
    }
    
    void subscribeXAction()
    {
        XControllers.OnXLeft  += OnXLeft;
        XControllers.OnXRight += OnXRight;
    }
    
    void unsubscribeXAction()
    {
        XControllers.OnXLeft  -= OnXLeft;
        XControllers.OnXRight -= OnXRight;
    }
    
    // TODO переделать на паттерн метод обновления
    //      или на нормальный таймер
    //      или как-то еще вызывать колбек - OnEndXAction();
    private int counterXAction = -1;
    void updXAction()
    {
        counterXAction = counterXAction != -1 ? counterXAction + 1 : -1;
        if (counterXAction == timeXAction)
        {
            OnEndXAction();
        }
    }

    
    /////////////////////////////////////
    void Start()
    {
        subscribeYAction();
        subscribeXAction();
    }
    
    void Update()
    {
        updYAction();
        updXAction();
    }
}