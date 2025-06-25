using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    public float rotationSpeed = 30f; // độ/giây
    public float AngularVelocityRad { get; private set; }

    private float lastAngle;

    void Start()
    {
        lastAngle = transform.eulerAngles.z;
    }

    void Update()
    {
        // Quay bánh xe
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Tính tốc độ góc rad/s
        float currentAngle = transform.eulerAngles.z;
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
        AngularVelocityRad = delta * Mathf.Deg2Rad / Time.deltaTime;
        lastAngle = currentAngle;
    }
}
