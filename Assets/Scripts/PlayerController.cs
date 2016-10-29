using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector3 newDirection;
    private double timeleft;

    private float speed = 0.1f;

    void Start()
    {
        transform.position = Vector2.zero;

        //в тестовом варианте по умолчанию игрок движется влево
        newDirection = new Vector3(speed, 0);
    }

    void FixedUpdate()
    {
        //следующий код отвечает за передвежиние игрока по полченным координатам, через апи
        int checkNumber = CheckTurnEvent();
        if (checkNumber != 0)
        {
            newDirection = GetNewDirection(checkNumber);
        }
        transform.Translate(newDirection);
    }

    int CheckTurnEvent()
    {
        //все(кроме последнего) условия проверяют находится ли игрок в углу, для того, чтобы вертеть его по кругу
        if (transform.position.x >= 10.8 && transform.position.y >= 10.8)
        {
            return 1;
        }
        else if (transform.position.x <= -10.8 && transform.position.y >= 10.8)
        {
            return 2;
        }
        else if (transform.position.x <= -10.8 && transform.position.y <= -10.8)
        {
            return 3;
        }
        else if (transform.position.x >= 10.8 && transform.position.y <= -10.8)
        {
            return 4;
        }
        else if (transform.position.x >= 10.8)
        {
            return 5;
        }
        else
        {
            return 0;
        }
    }

    Vector3 GetNewDirection(int directionNumber)
    {
        //!TODO: после реализации API изменить метод, на реальную проверку ивентов.
        switch (directionNumber)
        {
            case 1:
                return new Vector3(-speed, 0);
            case 2:
                return new Vector3(0, -speed);
            case 3:
                return new Vector3(speed, 0);
            case 4:
                return new Vector3(0, speed);
            case 5:
                return new Vector3(0, speed);
            default:
                return newDirection;
        }
    }
}
