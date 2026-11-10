using UnityEngine;

public class Crystal : MonoBehaviour
{
    private Rigidbody rb;
    private Collider box;
    private Collider range;

    [SerializeField] private CargoType type;
    public CargoType Type => type;

    [SerializeField] private int pointValue = 1;

    private float canPickUpAt;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Transform initialParent;

    public float Weight()
    {
        return type != null ? type.Weight() : 0f;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        range = GetComponent<SphereCollider>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialParent = transform.parent;
    }

    public bool CanBePickedUp()
    {
        return Time.time >= canPickUpAt;
    }

    public int Points()
    {
        return pointValue;
    }

    public void PickUp(Transform carryPoint)
    {
        rb.isKinematic = true;
        box.enabled = false;
        range.enabled = false;

        transform.SetParent(carryPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        transform.SetParent(null);
        box.enabled = true;
        range.enabled = true;
        rb.isKinematic = false;

        Vector3 push = transform.forward + Vector3.up;
        rb.AddForce(push * 3f, ForceMode.Impulse);

        canPickUpAt = Time.time + 0.5f;
    }

    public void ResetToSpawn()
    {
        transform.SetParent(initialParent);
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (box != null) box.enabled = true;
        if (range != null) range.enabled = true;

        canPickUpAt = Time.time;
    }
}