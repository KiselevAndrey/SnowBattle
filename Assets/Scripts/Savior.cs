using System;
using UnityEngine;

public class Savior : MonoBehaviour
{
    static public Action<int> RespawnObj;

    private void OnTriggerEnter(Collider other)
    {
        string name = other.gameObject.name;
        if (int.TryParse(name[0].ToString(), out int number))
            RespawnObj(number);
        else
            RespawnObj(0);
    }
}
