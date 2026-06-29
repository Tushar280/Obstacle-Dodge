using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float spinX;
    [SerializeField] float spinY;
    [SerializeField] float spinZ;

    void Update()
    {
        transform.Rotate(spinX, spinY, spinZ);
    }
}
