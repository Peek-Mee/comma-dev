using UnityEngine;
using Comma.Global.SaveLoad;
using Comma.Utility.Collections;
using Comma.Gameplay.DetectableObject;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Comma.Gameplay.Environment
{
    [System.Serializable]
    public struct MovingTerrainInfo
    {
        [SerializeField] private GameObject _movingTerrain;
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private float _moveTime;
        [SerializeField] private float _delayTime;
        [SerializeField] private LeanTweenType _easing;
        [SerializeField] private bool _shakeCamera;

        public GameObject MovingTerrain => _movingTerrain;
        public Vector3 InitialPosition => _initialPosition;
        public Vector3 TargetPosition => _targetPosition;
        public float MoveTime => _moveTime;
        public float DelayTime => _delayTime;
        public LeanTweenType Easing => _easing;
        public bool ShakeCamera => _shakeCamera;
    }
    public class MoveTerrainTrigger : MonoBehaviour
    {
        [SerializeField] private string _triggerId;
        [SerializeField, HideInInspector] private MovingTerrainInfo[] _terrains;
        private bool _moved = false;

        private enum TriggerState { UP, DOWN };

        private PlayerSaveData _saveData;


        private void Start()
        {
            _saveData = SaveSystem.GetPlayerData();

            if (!_saveData.IsObjectInteracted(_triggerId)) return;
            // Get the state of the trigger
            var temp = _saveData.GetObjectPosition(_triggerId);
            // Since the saved data is in Vector3, we need to convert it first
            var state = temp.y > 0 ? TriggerState.UP : TriggerState.DOWN;

            // If the terrain is UP from the last saved data
            // Then move it up without 
            if (state == TriggerState.UP)
            {
                foreach (var terrain in _terrains)
                {
                    MoveTerrain(terrain.MovingTerrain, terrain.TargetPosition, 0f, 0f, false);
                }
                _moved = true;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_moved) { return; }
            foreach (var terrain in _terrains)
            {
                MoveTerrain(terrain.MovingTerrain, terrain.TargetPosition, terrain.MoveTime, 0f, false);
            }
            _moved = true;

        }
        /// <summary>
        /// Initialize terain position according to the save data (if any)
        /// </summary>
        //private void InitTerrainPosition(GameObject terrainToMove, Vector3 )
        //{

        //}
        /// <summary>
        /// Move the terrain to a certain position with the given time and delay.
        /// </summary>
        /// <param name="terrainToMove"></param>
        /// <param name="targetPosition"></param>
        /// <param name="moveTime"></param>
        /// <param name="delayTime"></param>
        /// <param name="shakeCamera"></param>
        private void MoveTerrain(GameObject terrainToMove, Vector3 targetPosition, 
            float moveTime, float delayTime = 0f, bool shakeCamera = true, LeanTweenType easing = LeanTweenType.notUsed)
        {
            LeanTween.value(0f, 1f, delayTime).setOnComplete(() =>
            {
                LeanTween.move(terrainToMove, targetPosition, moveTime)
                .setEase(easing)
                .setOnStart(() =>
                {
                    // Start Camera Shake
                })
                .setOnUpdate((Vector3 val) =>
                {
                    terrainToMove.transform.position = val;
                })
                .setOnComplete(() =>
                {
                    // Make sure the terrain position is correct
                    terrainToMove.transform.position = targetPosition;
                    // Stop Camera Shake
                });

            });
        }
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects, CustomEditor(typeof(MoveTerrainTrigger))]
    public class MoveTerrainTriggerEditor : Editor
    {
        private SerializedProperty _terrains;
        private GUIStyle _style;

        private void OnEnable()
        {
            _terrains = serializedObject.FindProperty("_terrains");
            _terrains.isExpanded = true;
            _style = new()
            {
                richText = true
            };
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            var fold = true;
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("<color=white><b>Moving Terrain</b></color>", _style);
            
            DrawPropertyArray(_terrains, ref fold);
            serializedObject.ApplyModifiedProperties();
        }
        private void DrawPropertyArray(SerializedProperty property, ref bool fold)
        {
            fold = EditorGUILayout.Foldout(fold, new GUIContent(
                property.displayName,
                "These are the waypoints that will be used for the moving object's path."), true);
            if (!fold) return;
            GUILayout.BeginHorizontal();
            var arraySizeProp = property.FindPropertyRelative("Array.size");
            var style = _style;
            style.stretchWidth = false;
            EditorGUILayout.PropertyField(arraySizeProp);
            if (GUILayout.Button("Add Terrain Info"))
            {
                _terrains.arraySize++;
            }
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            for (var i = 0; i < arraySizeProp.intValue; i++)
            {
                var arr = property.GetArrayElementAtIndex(i);
                EditorGUILayout.LabelField($"<color=white><b>Terrain {i + 1}</b></color>", _style);
                EditorGUI.indentLevel++;
                var style1 = _style;
                // _movingTerrain
                arr.FindPropertyRelative("_movingTerrain").objectReferenceValue = EditorGUILayout.ObjectField("Terrain", 
                    arr.FindPropertyRelative("_movingTerrain").objectReferenceValue, typeof(GameObject), true);
                // _initialPosition V3
                arr.FindPropertyRelative("_initialPosition").vector3Value = EditorGUILayout.Vector3Field("Initial Position",
                    arr.FindPropertyRelative("_initialPosition").vector3Value);
                var moveTerrain = arr.FindPropertyRelative("_movingTerrain").objectReferenceValue;
                EditorGUILayout.Separator();
                if (GUILayout.Button("Set Current Position as Initial"))
                {
                    if (moveTerrain != null)
                    {
                        var go = (GameObject)moveTerrain;
                        arr.FindPropertyRelative("_initialPosition").vector3Value = go.transform.position;
                    }
                }
                EditorGUILayout.Separator();
                // _targetPosition V3
                arr.FindPropertyRelative("_targetPosition").vector3Value = EditorGUILayout.Vector3Field("Target Position",
                    arr.FindPropertyRelative("_targetPosition").vector3Value);
                EditorGUILayout.Separator();
                if (GUILayout.Button("Set Current Position as Target"))
                {
                    if (moveTerrain != null)
                    {
                        var go = (GameObject)moveTerrain;
                        arr.FindPropertyRelative("_targetPosition").vector3Value = go.transform.position;
                    }
                }
                if (GUILayout.Button("Move Object to Initial Position"))
                {
                    if (moveTerrain != null)
                    {
                        var go = (GameObject)moveTerrain;
                        go.transform.position = arr.FindPropertyRelative("_initialPosition").vector3Value;
                    }
                }
                if (GUILayout.Button("Move Object to Target Position"))
                {
                    if (moveTerrain != null)
                    {
                        var go = (GameObject)moveTerrain;
                        go.transform.position = arr.FindPropertyRelative("_targetPosition").vector3Value;
                    }
                }
                EditorGUILayout.Separator();
                // _moveTime float
                arr.FindPropertyRelative("_moveTime").floatValue = EditorGUILayout.FloatField("Move Time",
                    arr.FindPropertyRelative("_moveTime").floatValue);
                // _delayTime float
                arr.FindPropertyRelative("_delayTime").floatValue = EditorGUILayout.FloatField("Delay Time",
                    arr.FindPropertyRelative("_delayTime").floatValue);
                // _easing LeanTweenType

                //Debug.Log($"Before: {easingType}");
                var easingType = (LeanTweenType)EditorGUILayout.EnumPopup("Easing Method", (LeanTweenType)arr.FindPropertyRelative("_easing").enumValueIndex);
                arr.FindPropertyRelative("_easing").enumValueIndex = (int)easingType;
                //easing = (int)easingType;
                //Debug.Log($"After: {easing}");
                //Debug.Log($"After: {easingType}");
                //Debug.Log(easing);
                // _shake bool
                EditorGUI.indentLevel--;
                EditorGUILayout.Separator();
                if (GUILayout.Button($"Delete Terrain {i + 1} Info"))
                {
                    property.DeleteArrayElementAtIndex(i);
                    //property.arraySize--;
                }
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.indentLevel--;
        }
    }
#endif
}
