using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private int hitCount = 0;


    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Player has been hit!");
        //Debug.Log(collision.gameObject.name + " hit the player");
        hitCount++;
        Debug.Log("Total hits = " + hitCount + " on player");

        if(collision.gameObject.CompareTag == "Obstacle")
        {
            
        }
    }
}
