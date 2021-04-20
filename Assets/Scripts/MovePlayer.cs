using UnityEngine;

enum InputType { AWSD, ARROWS}
public class MovePlayer : MonoBehaviour
{
    [SerializeField] InputType inputType;
    [SerializeField] Rigidbody rb;

    [Header("Скорости")]
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;

    void FixedUpdate()
    {
        if (rb.isKinematic) return;

        float inputVertical = 0;
        float inputHorizontal = 0;

        switch (inputType)
        {
            case InputType.AWSD:
                inputVertical = Input.GetAxis("Vertical_AWSD");
                inputHorizontal = Input.GetAxis("Horizontal_AWSD");
                break;
            case InputType.ARROWS:
                inputVertical = Input.GetAxis("Vertical_ARROWS");
                inputHorizontal = Input.GetAxis("Horizontal_ARROWS");
                break;
            default:
                break;
        }

        if (inputVertical != 0)
        {
            Vector3 velocity = transform.forward;
            velocity.y = 0;
            //rb.velocity = velocity * inputVertical * speed;
            rb.AddForce(velocity * inputVertical * speed);
        }

        if(inputHorizontal != 0)
        {
            Vector3 angularVelocity = Vector3.zero;
            angularVelocity.y = inputHorizontal * rotationSpeed;
            rb.angularVelocity = angularVelocity;
        }
        //transform.RotateAround(transform.position, Vector3.up, inputHorizontal * rotationSpeed);
    }
}
