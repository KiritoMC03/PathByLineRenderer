using UnityEditor;
using UnityEngine;


namespace Scripts.Path
{
    [CustomEditor(typeof(PathCreator))]
    public class PathEditor : Editor
    {
        private PathCreator _creator;
        private Path _path;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Create new"))
            {
                _creator.CreatePath();
                _path = _creator.path;
                SceneView.RepaintAll();
            }
            
            if (GUILayout.Button("Toggle closed"))
            {
                _path.ToggleClosed();
                SceneView.RepaintAll();
            }
        }

        private void OnSceneGUI()
        {
            Input();
            Draw();
        }

        private void Input()
        {
            Event guiEvent = Event.current;
            Vector2 mousePosition = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

            if (guiEvent.type == EventType.MouseDown && 
                guiEvent.button == 0 && 
                guiEvent.shift)
            {
                Undo.RecordObject(_creator, "Add segment");
                _path.AddSegment(mousePosition);
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            {
                var minDistanceToAnchor = 0.05f;
                var closestAnchorIndex = -1;

                for (int i = 0; i < _path.NumPoints; i+=3)
                {
                    var distance = Vector2.Distance(mousePosition, _path[i]);

                    if (distance < minDistanceToAnchor)
                    {
                        minDistanceToAnchor = distance;
                        closestAnchorIndex = i;
                    }

                    if (closestAnchorIndex != -1)
                    {
                        Undo.RecordObject(_creator, "Delete segment");
                        _path.DeleteSemgent(closestAnchorIndex);
                    }
                }
            }
        }
        
        private void Draw()
        {
            for (int i = 0; i < _path.NumSegments; i++)
            {
                var points = _path.GetPointsInSegment(i);
                
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
                Handles.DrawBezier(
                    points[0], 
                    points[3], 
                    points[1], 
                    points[2], 
                    Color.green, 
                    null,
                    2);
            }
            
            
            Handles.color = Color.red;
            
            for (int i = 0; i < _path.NumPoints; i++)
            {
                Vector2 newPosition = Handles.FreeMoveHandle(_path[i], Quaternion.identity, 0.1f, Vector2.zero, Handles.CylinderHandleCap);
                if (_path[i] != newPosition)
                {
                    Undo.RecordObject(_creator, "Move point");
                    _path.MovePoint(i, newPosition);
                }
            }
        }
        
        private void OnEnable()
        {
            _creator = (PathCreator) target;
            if (_creator.path == null)
            {
                _creator.CreatePath();
            }
            _path = _creator.path;
        }
    }
}