using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private int hitCount = 0;
    private Rigidbody rb;
    private Move moveScript;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<Move>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Player has been hit!");
        //Debug.Log(collision.gameObject.name + " hit the player");
        hitCount++;
        Debug.Log("Total hits = " + hitCount + " on player");

        if(collision.gameObject.CompareTag("Obstacle"))
        {
            MeshRenderer meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
            Material material = meshRenderer.material;
            material.color = Color.red;

            rb.constraints = RigidbodyConstraints.None;
            //rb.useGravity = true;
            moveScript.enabled = false;
        }
    }
}
