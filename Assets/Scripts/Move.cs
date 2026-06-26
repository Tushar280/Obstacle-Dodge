using UnityEngine;

public class Move : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float valueX = Input.GetAxis("horizontal");
        float valueY = 0;
        float valueZ = Input.GetAxis("vertical");
        transform.Translate(valueX,valueY,valueZ);
    }
}
