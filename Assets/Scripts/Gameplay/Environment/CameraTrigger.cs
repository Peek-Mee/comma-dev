using Comma.Global.PubSub;
using UnityEngine;
using static Comma.Gameplay.Environment.CameraTrigger;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Comma.Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class CameraTrigger : MonoBehaviour
    {
        public enum CameraTriggerType
        {
            Zoom, Offset, Attachment
        }
        [SerializeField] private CameraTriggerType _triggerType;

        [Header("Trigger Direction")]
        [SerializeField] private bool _rightToLeft;
        [SerializeField] private LeanTweenType _easingType;
        [Header("Zoom Out")]
        //[SerializeField, HideInInspector] private LeanTweenType _zoomEasingType;
        [SerializeField, HideInInspector] private float _startOrthoSize;
        [SerializeField, HideInInspector] private float _finishOrthoSize;
        [Header("Offset Change")]
        //[SerializeField, HideInInspector] private LeanTweenType _offsetEasingType;
        [SerializeField, HideInInspector] private Vector2 _startOffset;
        [SerializeField, HideInInspector] private Vector2 _finishOffset;
        [Header("Stop Camera Movement")]
        [SerializeField, HideInInspector] private Transform _newFollow;
        [SerializeField, HideInInspector] private bool _stopCameraMovement;

        private Collider2D _coll;
        private float _distance;
        private void Awake()
        {
            _coll = GetComponent<Collider2D>();
            _distance = _coll.bounds.size.x;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            bool checkPlayerFromLeft = IsPlayerFromLeft(other);
            switch (_triggerType)
            {
                case CameraTriggerType.Zoom:
                    var zoomMessage = new OnZoomCameraTrigger(
                        _rightToLeft ? 1 : -1,
                        _rightToLeft == checkPlayerFromLeft ? _startOrthoSize : _finishOrthoSize,
                        _rightToLeft == checkPlayerFromLeft ? _finishOrthoSize : _startOrthoSize,
                        _distance + (other.bounds.size.x / 2f),
                        _easingType
                        );
                    EventConnector.Publish("OnZoomCameraTrigger", zoomMessage);
                    break;
                case CameraTriggerType.Offset:
                    var offsetMessage = new OnOffsetCameraTrigger(
                        _rightToLeft ? 1 : -1,
                        _rightToLeft == checkPlayerFromLeft ? _startOffset : _finishOffset,
                        _rightToLeft == checkPlayerFromLeft ? _finishOffset : _startOffset,
                        _distance + (other.bounds.size.x / 2f),
                        _easingType
                        );
                    EventConnector.Publish("OnOffsetCameraTrigger", offsetMessage);
                    break;
                case CameraTriggerType.Attachment:
                    var stopMoveMessage = new OnStopCameraMoveTrigger(
                        _rightToLeft ? 1 : -1,
                        _stopCameraMovement,
                        _newFollow
                        );
                    EventConnector.Publish("OnStopCameraMovement", stopMoveMessage);
                    break;
                default:
                    break;
            }

            //float playerSizeX = other.bounds.size.x/2f;
            //bool playerFromLeft = IsPlayerFromLeft(other);
            //OnEnterCameraTrigger val;
            //_ = _rightToLeft == playerFromLeft ? val = new(_rightToLeft ? 1 : -1, _startZoomScale, _finishZoomScale, _startOffset, _finishOffset, _distance + playerSizeX) :
            //    val = new(_rightToLeft ? 1: -1, _finishZoomScale, _startZoomScale, _finishOffset, _startOffset, _distance + playerSizeX);

            //EventConnector.Publish("OnEnterCameraTrigger", val);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            //EventConnector.Publish("OnExitCameraTrigger", new OnExitCameraTrigger());
            EventConnector.Publish("OnCameraTriggerExit", new OnCameraTriggerExit(_triggerType));
        }

        private bool IsPlayerFromLeft(Collider2D target)
        {
            Vector2 normal = target.transform.position - transform.position;

            return normal.x > 0;
        }
    }
#if UNITY_EDITOR
    [CanEditMultipleObjects, CustomEditor(typeof(CameraTrigger))]
    public class CameraTriggerEditor : Editor
    {
        private SerializedProperty _triggerType;
        
        private SerializedProperty _startOrthoSize;
        private SerializedProperty _finishOrthoSize;
        
        private SerializedProperty _startOffset;
        private SerializedProperty _finishOffset;
        private GUIStyle _style;
        private SerializedProperty _stopCameraMovement;
        private SerializedProperty _newFollow;

        private void OnEnable()
        {
            _triggerType = serializedObject.FindProperty("_triggerType");
            _startOrthoSize = serializedObject.FindProperty("_startOrthoSize");
            _finishOrthoSize = serializedObject.FindProperty("_finishOrthoSize");
            _startOffset = serializedObject.FindProperty("_startOffset");
            _finishOffset = serializedObject.FindProperty("_finishOffset");
            _stopCameraMovement = serializedObject.FindProperty("_stopCameraMovement");
            _newFollow = serializedObject.FindProperty("_newFollow");
            _style = new()
            {
                richText = true
            };
        }
        public override void OnInspectorGUI()
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            base.OnInspectorGUI();
            switch (_triggerType.enumValueIndex)
            {
                case 0:
                    EditorGUILayout.LabelField("<color=white><b>Zoom Out Camera Trigger</b></color>", _style);
                    _startOrthoSize.floatValue = EditorGUILayout.FloatField("Start Ortho Size" ,_startOrthoSize.floatValue);
                    //var buttonStart = GUILayout.Button("Set Current Ortho Size As Start");
                    _finishOrthoSize.floatValue =  EditorGUILayout.FloatField("Finish Ortho Size", _finishOrthoSize.floatValue);
                    //var buttonFinish = GUILayout.Button("Set Current Ortho Size As Finish");
                    //if (buttonStart)
                    //{
                    //    var orthoSize = Camera.main.orthographicSize;
                    //    _startOrthoSize.floatValue = orthoSize;
                    //}
                    //if (buttonFinish)
                    //{
                    //    var orthoSize = Camera.main.orthographicSize;
                    //    _finishOrthoSize.floatValue = orthoSize;
                    //}
                    break;
                case 1:
                    EditorGUILayout.LabelField("<color=white><b>Offset Camera Trigger</b></color>", _style);
                    _startOffset.vector2Value = EditorGUILayout.Vector2Field("Start Offset Camera", _startOffset.vector2Value);
                    _finishOffset.vector2Value = EditorGUILayout.Vector2Field("Finish Offset Camera", _finishOffset.vector2Value);
                    break;
                case 2:
                    EditorGUILayout.LabelField("<color=white><b>Movement Camera Trigger</b></color>", _style);
                    _stopCameraMovement.boolValue = EditorGUILayout.Toggle("Stop Camera Movement?", _stopCameraMovement.boolValue);
                    _newFollow.objectReferenceValue = EditorGUILayout.ObjectField("New Object To Follow" , _newFollow.objectReferenceValue, typeof(Transform), true);
                    break;
                default:
                    break;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

}
