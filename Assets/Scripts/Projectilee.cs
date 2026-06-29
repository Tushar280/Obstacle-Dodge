using UnityEngine;

public class Projectilee : MonoBehaviour
{
    public Transform player;
    private Vector3 playerPosition;
    public float speed;

    private void Start()
    {
        playerPosition = player.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position , playerPosition , speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("Player has been hit by projectile");
        }
    }
}
