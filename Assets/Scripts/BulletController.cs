using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float speed;

    Rigidbody2D _rigidbody;

    Vector2 _direction;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rigidbody.velocity = _direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            CentinelController controller = other.gameObject.GetComponent<CentinelController>();
            
            // IMPLEMENTS GetPoints ON CentinelController => 10 PUNTOS
            // float poits = controller.GetPoints();

            // IncreasePoints(points);

            Destroy(other.gameObject);
        }
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
