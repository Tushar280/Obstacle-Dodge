using UnityEngine;

public class ProjTrigger : MonoBehaviour
{
    public GameObject[] projectiles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            foreach(GameObject projectile in projectiles)
            {
                projectile.SetActive(true);
            }
        }
    }
}
