using Comma.Gameplay.Environment;
using UnityEngine;

namespace Comma.Global.PubSub
{
    public struct OnPlayerUsePortal
    {
        public Vector2 Destination { get; private set; }
        public int Layer { get; private set; }

        public OnPlayerUsePortal(Vector2 destination, int layer)
        {
            Destination = destination;
            Layer = layer;
        }
    }
    public struct OnPlayerMove
    {
        public Vector2 Direction { get; private set; }

        public OnPlayerMove(Vector2 direction)
        {
            Direction = direction;
        }
    }

    public struct OnPlayerJump
    {
    }

    public struct OnPlayerInteract
    { 
    }
    public struct OnPlayerNearPortal
    {
        public string Portal { get; private set; }
        public OnPlayerNearPortal(string portals)
        {
            Portal= portals;
        }
    }

    public struct OnPlayerSprint
    {
        public bool Sprint { get; private set; }

        public OnPlayerSprint(bool sprinting)
        {
            Sprint = sprinting;
        }
    }
    public struct OnPlayerSwapDown { }
    public struct OnZoomCameraTrigger
    {
        public int MoveToLeft {  get; private set; }
        public float StartOrthoSize { get; private set; }
        public float FinishOrthoSize { get; private set; }
        public float Distance { get; private set; }
        public LeanTweenType Easing { get; private set; }
        public OnZoomCameraTrigger(int moveToLeft, float startOrthoSize, float finishOrthoSize, float distance, LeanTweenType easing)
        {
            MoveToLeft = moveToLeft; 
            StartOrthoSize = startOrthoSize;
            FinishOrthoSize = finishOrthoSize;
            Distance = distance;
            Easing = easing;
        }
    }
    public struct OnOffsetCameraTrigger
    {
        public int MoveToLeft { get; private set; }
        public Vector2 StartOffset { get; private set; }
        public Vector2 FinishOffset { get; private set; }
        public float Distance { get; private set; }
        public LeanTweenType Easing { get; private set; }
        public OnOffsetCameraTrigger(int moveToLeft, Vector2 startOffste, Vector2 finishOffset, float distance, LeanTweenType easing)
        {
            MoveToLeft = moveToLeft;
            StartOffset = startOffste;
            FinishOffset = finishOffset;
            Distance = distance;
            Easing = easing;
        }
    }
    public struct OnStopCameraMoveTrigger
    {
        public int MoveToLeft { get; private set; }
        public bool StopCameraMovement { get; private set;}
        public Transform NewFollowObject { get; private set; }
        public OnStopCameraMoveTrigger(int moveToLeft, bool stopCameraMovement, Transform _newFollowObject)
        {
            MoveToLeft = moveToLeft;
            StopCameraMovement = stopCameraMovement;
            NewFollowObject = _newFollowObject;
        }
    }
    public struct OnCameraTriggerExit
    {
        public CameraTrigger.CameraTriggerType CameraTriggerType { get; private set; }
        public OnCameraTriggerExit(CameraTrigger.CameraTriggerType cameraTriggerType)
        {
            CameraTriggerType = cameraTriggerType;
        }
    }
    public struct OnEnterCameraTrigger
    {
        public int MoveToRight { get; private set; }
        public float StartScale { get; private set; }
        public float FinishScale { get; private set; }
        public Vector2 StartOffset { get; private set; }
        public Vector2 FinishOffset { get; private set; }
        public float Distance { get; private set; }
        /// <summary>
        /// construction struct
        /// </summary>
        /// <param name="toRight">Is trigger flow to the right?</param>
        /// <param name="startScale">Zoom out scale on start trigger</param>
        /// <param name="finishScale">Zoom out scale on finish trigger</param>
        /// <param name="startOff"></param>
        /// <param name="finishOff"></param>
        /// <param name="distance"></param>
        public OnEnterCameraTrigger(int toRight, float startScale, float finishScale, 
            Vector2 startOff, Vector2 finishOff, float distance)
        {
            MoveToRight = toRight;
            StartScale = startScale;
            FinishScale = finishScale;
            StartOffset = startOff;
            FinishOffset = finishOff;
            Distance = distance;
        }
    }
    public struct OnExitCameraTrigger { }
}