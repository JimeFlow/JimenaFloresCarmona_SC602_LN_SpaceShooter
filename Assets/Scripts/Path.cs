using System;
using UnityEngine;

[Serializable]
public class Path
{
    [SerializeField]
    GameObject gameObject;

    Transform[] _points;

    public void SetPoints(Transform[] points)
    {
        _points = points;
    }

    public Transform[] GetPoints()
    {
        return _points;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
