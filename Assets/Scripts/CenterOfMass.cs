using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform centerOfMass;
    [SerializeField] bool drawGizmos;

    private void Awake()
    {
        rb.centerOfMass = Vector3.Scale(centerOfMass.localPosition, transform.localScale);
    }

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(rb.worldCenterOfMass, 0.1f);
    }
}
