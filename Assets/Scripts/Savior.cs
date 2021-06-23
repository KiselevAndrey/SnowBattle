using System;
using UnityEngine;

public class Savior : MonoBehaviour
{
    //static public Action<int> RespawnObj;

    //private void OnTriggerEnter(Collider other)
    //{
    //    string name = other.gameObject.name;
    //    print(name);
    //    if (int.TryParse(name[0].ToString(), out int number))
    //    {
    //        print(number);
    //        RespawnObj(number);
    //    }
    //    else
    //    {
    //        print(0);
    //        RespawnObj(0);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Respawn respawn))
        {
            respawn.RespawnMe();
        }
    }
}
