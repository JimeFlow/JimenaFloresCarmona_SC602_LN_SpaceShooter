using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceshipController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float speed = 10.0F;

    [SerializeField]
    Vector2 edges;

    [SerializeField]
    bool handleClamp;

    [Header("Rotation")]
    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    float rotationTime;

    [SerializeField]
    bool mouseRotation;

    [Header("Fire")]
    [SerializeField]
    Transform firePoint;

    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    float bulletLifeTime;

    [SerializeField]
    float fireTimeout;

    [Header("Animations")]
    [SerializeField]
    float dieTimeout;

    [SerializeField]
    float dieWaitTime;

    [SerializeField]
    string fireSoundSFX;

    Vector2 _move = Vector2.zero;
    Vector2 _mousePoint;

    Rigidbody2D _rigidbody;

    float _rotationDirection;
    float _fireTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInputMove();
        HandleInputRotation();
        HandleFire();
    }

    private void FixedUpdate()
    {
        HandleRotation();

        if (_move.sqrMagnitude == 0.0F)
        {
            return;
        }

        HandleMove();
        HandleClamp();
        HandleTeleport();
    }

    private void HandleFire()
    {
        _fireTimer -= Time.deltaTime;

        if (Input.GetButtonUp("Fire1"))
        {
            if (_fireTimer > 0.0F)
            {
                return;
            }

            GameObject bullet =
                Instantiate(bulletPrefab, firePoint.position, transform.rotation);

            Vector2 direction = (firePoint.position - transform.position).normalized;

            BulletController controller = bullet.GetComponent<BulletController>();
            controller.SetDirection(direction);

            Destroy(bullet, bulletLifeTime);
            _fireTimer = fireTimeout;

            SoundManager.Instance.PlaySFX(fireSoundSFX);
        }
    }

    private void HandleInputRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            _rotationDirection = 1.0F;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            _rotationDirection = -1.0F;
        }
        else
        {
            _rotationDirection = 0.0F;
        }

        _mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void HandleTeleport()
    {
        if (handleClamp)
        {
            return;
        }

        Vector2 currentPosition = _rigidbody.position;
        if (currentPosition.x > 0.0F && currentPosition.x >= edges.x)
        {
            _rigidbody.position = new Vector2(-edges.x + 0.01F, currentPosition.y);
        }
        else if (currentPosition.x < 0.0F && currentPosition.x <= -edges.x)
        {
            _rigidbody.position = new Vector2(edges.x - 0.01F, currentPosition.y);
        }

        if (currentPosition.y > 0.0F && currentPosition.y >= edges.y)
        {
            _rigidbody.position = new Vector2(currentPosition.x, -edges.y + 0.01F);
        }
        else if (currentPosition.y < 0.0F && currentPosition.y <= -edges.y)
        {
            _rigidbody.position = new Vector2(currentPosition.x, edges.y - 0.01F);
        }
    }

    private void HandleClamp()
    {
        if (!handleClamp)
        {
            return;
        }

        float x = Mathf.Clamp(_rigidbody.position.x, -edges.x, edges.x);
        float y = Mathf.Clamp(_rigidbody.position.y, -edges.y, edges.y);
        _rigidbody.position = new Vector2(x, y);
    }

    private void HandleMove()
    {
        Vector2 direction = _move.normalized;
        Vector2 currentPosition = _rigidbody.position;
        _rigidbody.MovePosition(currentPosition + direction * speed * Time.fixedDeltaTime);
    }

    private void HandleInputMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _move = new Vector2(x, y);
    }

    private void HandleRotation()
    {
        if (mouseRotation)
        {
            Vector2 currentPoint = _rigidbody.position;
            Vector2 direction = (_mousePoint - currentPoint).normalized;
            float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rigidbody.MoveRotation(angleZ);
            return;
        }

        float currentRotation = _rigidbody.rotation;
        if (_rotationDirection != 0.0F)
        {
            float targetRotation = currentRotation + _rotationDirection * rotationSpeed * Time.fixedDeltaTime;
            float rotation = Mathf.Lerp(currentRotation, targetRotation, rotationTime);
            _rigidbody.rotation = rotation;
        }
    }

    public void Die()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;

        SpaceshipController controller = GetComponent<SpaceshipController>();
        controller.enabled = false;

        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        Color color = renderer.color;

        while (color.a > 0.0F)
        {
            color.a -= 0.1F;
            renderer.color = color;
            yield return new WaitForSeconds(dieTimeout);
        }

        yield return new WaitForSeconds(dieWaitTime); //gameObject.SetActive(false); //Destroy(gameObject);

        // MOVE TO LevelManager AS Reload() => 10 PUNTOS
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // GO TO GAMEOVER WHEN HasLives IS FALSE =>10 PUNTOS
    }
}
