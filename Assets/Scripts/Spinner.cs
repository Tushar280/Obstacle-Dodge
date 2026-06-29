using UnityEngine;

public class Spinner : MonoBehaviour
{
    public Vector3 RotationAngles; 

    void Update()
    {
        transform.Rotate(RotationAngles);
    }
}
