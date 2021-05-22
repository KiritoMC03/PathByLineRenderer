using System;
using UnityEngine;

namespace Scripts.Path
{
    [RequireComponent(typeof(LineRenderer))]
    public class PathPointsKeeper : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private Vector3[] _tempPoints;

        private void Awake()
        {
            InitFields();
        }

        private void Start()
        {
            GetPoints();
        }

        private void InitFields()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            if (_lineRenderer is null)
            {
                throw new Exception("Line Renderer is null!");
            }
        }

        public Vector3[] GetPoints()
        {
            _tempPoints = new Vector3[GetPointsCount()];
            _lineRenderer.GetPositions(_tempPoints);
            return _tempPoints;
        }

        private int GetPointsCount()
        {
            return _lineRenderer.positionCount;
        }
    }
}