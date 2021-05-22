using System;
using UnityEngine;

namespace Scripts.Path
{
    [RequireComponent(typeof(LineRenderer))]
    public class Path : MonoBehaviour
    {
        [SerializeField, Range(0, 100)]
        private int segments = 50;
        [SerializeField, Range(0, 5)]
        private float xRadius = 5;
        [SerializeField, Range(0, 5)]
        private float yRadius = 5;
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            InitFields();

            _lineRenderer.positionCount = segments + 1;
            _lineRenderer.useWorldSpace = false;
            CreatePoints();
        }

        private void InitFields()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            if (_lineRenderer is null)
            {
                throw new Exception("Line Renderer is null!");
            }
        }

        private void CreatePoints()
        {
            var angle = 20f;

            for (var i = 0; i < (segments + 1); i++)
            {
                var x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
                var  y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;

                _lineRenderer.SetPosition(i, new Vector3(x, 0f, y));

                angle += (360f / segments);
            }
        }
    }
}