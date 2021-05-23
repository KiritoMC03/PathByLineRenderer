using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Path
{
    [Serializable]
    public class Path
    {
        [HideInInspector]
        [SerializeField] private List<Vector2> points;

        public Vector2 this[int i] => points[i];
        public int NumPoints => points.Count;
        public int NumSegments => (points.Count - 4) / 3 + 1;

        public Path(Vector2 centre)
        {
            points = new List<Vector2>()
            {
                centre + Vector2.left,
                centre + (Vector2.left + Vector2.up) * 0.5f,
                centre + (Vector2.right + Vector2.down) * 0.5f,
                centre + Vector2.right
            };
        }
        
        public void AddSegment(Vector2 anchorPosition)
        {
            points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
            points.Add((points[points.Count - 1] + anchorPosition) * 0.5f);
            points.Add(anchorPosition);
        }

        public Vector2[] GetPointsInSegment(int i)
        {
            return new Vector2[]
            {
                points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 + 3]
            };
        }

        public void MovePoint(int i, Vector2 position)
        {
            Vector2 deltaMove = position - points[i];
            points[i] = position;

            if (i % 3 == 0)
            {
                if (i + 1 < points.Count)
                {
                    points[i + 1] += deltaMove;
                }

                if (i - 1 >= 0)
                {
                    points[i - 1] += deltaMove;
                }
            }
            else
            {
                bool nextPointIsAnchor = (i + 1) % 3 == 0;
                int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
                int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

                if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count)
                {
                    float distance = (points[anchorIndex] - points[correspondingControlIndex]).magnitude;
                    Vector2 dir = (points[anchorIndex] - position).normalized;
                    points[correspondingControlIndex] = points[anchorIndex] + dir * distance;
                }
            }
        }
    }
}