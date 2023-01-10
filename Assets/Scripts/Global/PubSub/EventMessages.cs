using Comma.Gameplay.Environment;
using System.Collections;
using UnityEngine;

namespace Comma.Global.PubSub
{
    public struct OnPlayerUsePortal
    {
        public Vector2 Destination { get; private set; }

        public OnPlayerUsePortal(Vector2 destination)
        {
            Destination = destination;
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
    public struct OnCameraChangeTrigger
    {
        public CameraChangeData Data { get; private set; }
        public OnCameraChangeTrigger(CameraChangeData data)
        {
            Data = data;
        }
    }
}