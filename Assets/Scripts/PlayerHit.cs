using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player has been hit!");

        Debug.Log(collision.gameObject.name + " hit the player");


        MeshRenderer meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
        Material material = meshRenderer.material;
        material.color = Color.red;
    }
}
