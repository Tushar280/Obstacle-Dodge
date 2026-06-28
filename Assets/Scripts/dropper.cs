using UnityEngine;

public class dropper : MonoBehaviour
{
    public int timer = 3;
    public Rigidbody rigidBody;

    void Update()
    {
        if(Time.time > timer)
        {
            rigidBody.useGravity = true;
        }
    }
}
