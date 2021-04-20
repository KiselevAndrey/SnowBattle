using System;
using System.Collections;
using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField] float timeWithoutTouch;
    [SerializeField] Rigidbody rb;

    [Header("Звуки")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitPlayer;
    [SerializeField] AudioClip hitWall;

    int touchCount;

    static public Action TheyDontTouchMe;

    private void OnCollisionEnter(Collision collision)
    {
        touchCount++;
        StartCoroutine(Touching(touchCount));

        switch (collision.transform.tag)
        {
            case "Player":
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.volume = Mathf.Min(1f, (float)(rb.velocity.magnitude / 10));
                audioSource.PlayOneShot(hitPlayer);
                break;

            case "Ice":
                break;

            case "Wall":
                audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                audioSource.volume = Mathf.Min(1f, (float)(rb.velocity.magnitude / 10));
                audioSource.PlayOneShot(hitWall);
                break;
        }
    }

    IEnumerator Touching(int touching)
    {
        yield return new WaitForSeconds(timeWithoutTouch);
        if (touchCount == touching && rb.velocity.magnitude < 0.1f)
            TheyDontTouchMe();
    }
}
