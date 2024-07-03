using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float lifeTime;

    [SerializeField]
    int maxPathPoints;

    Rigidbody2D _rigidbody;
    Transform[] _pathPoints;
    Transform _nextPoint;

    float _aliveTime;
    int _maxPathPoints;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.position = _pathPoints[0].position;
        _maxPathPoints = Random.Range(maxPathPoints / 2, maxPathPoints);

        Next();
    }

    private void FixedUpdate()
    {
        if (_nextPoint == null)
        {
            Next();
        }

        Vector2 currentPosition = _rigidbody.position;
        Vector2 targetPosition = _nextPoint.position;
        Vector2 movePosition = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        _rigidbody.MovePosition(movePosition);
        if (Vector2.Distance(_rigidbody.position, targetPosition) < 0.2F)
        {
            if (_aliveTime <= 0.0F)
            {
                Next();
                _maxPathPoints--;
            }
            else
            {
                _aliveTime -= Time.deltaTime;
            }
        }
    }

    private void Next()
    {
        if (_maxPathPoints <= 0)
        {
            Destroy(gameObject);
            return;
        }

        int pointNumber = Random.Range(2, _pathPoints.Length + 1);

        foreach (Transform pathPoint in _pathPoints)
        {
            if (pathPoint.name == "Point " + pointNumber.ToString())
            {
                _nextPoint = pathPoint;
                break;
            }
        }

        _aliveTime = lifeTime;
    }

    public void SetPathPoints(Transform[] pathPoints)
    {
        _pathPoints = pathPoints;
        enabled = true;
    }
}
