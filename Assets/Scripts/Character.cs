using System.Collections;
using Scripts.Path;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private PathPointsKeeper path;
    private Transform _transform;

    private void Awake()
    {
        InitFields();
    }

    private void Start()
    {
        StartCoroutine(MovementRoutine());
    }

    private void InitFields()
    {
        _transform = transform;
    }

    private IEnumerator MovementRoutine()
    {
        var pathPoints = path.GetPoints();
        var distanceDelta = moveSpeed * Time.deltaTime;
        
        foreach (var point in pathPoints)
        {
            while (_transform.position != point)
            {
                _transform.position = Vector3.MoveTowards(_transform.position, point, distanceDelta);
                yield return null;
            }
        }
    }
}
