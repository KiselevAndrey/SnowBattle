using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Rigidbody myRigibody;
    [SerializeField] private Transform spawnTransform;

    public void RespawnMe()
    {
        transform.position = spawnTransform.position;
        transform.rotation = spawnTransform.rotation;
        myRigibody.velocity = Vector3.zero;
        myRigibody.angularVelocity = Vector3.zero;
    }
}
