using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentinelController : MonoBehaviour
{
    [SerializeField]
    float points;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpaceshipController controller = other.gameObject.GetComponent<SpaceshipController>();
            controller.Die();

            Destroy(gameObject);
        }
    }
}
