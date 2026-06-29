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

        if(transform.position == playerPosition)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3.MoveTowards(transform.position , playerPosition , speed * Time.deltaTime);
        }
    }

}
