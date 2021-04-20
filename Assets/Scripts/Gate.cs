using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    static public Action<int> GoalMe;

    private void OnCollisionEnter(Collision collision)
    {
        // если это шайба
        if (collision.gameObject.CompareTag("Puck"))
        {
            string name = gameObject.name;  // узнаю свое имя
            int number = int.Parse(name[0].ToString()); // нахожу первую цифру
            GoalMe(number); // отправляюсобытие
        }
    }
}
