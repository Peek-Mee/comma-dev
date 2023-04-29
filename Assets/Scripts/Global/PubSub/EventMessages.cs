﻿using UnityEngine;

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
    public struct OnEnterCameraTrigger
    {
        public int MoveToRight { get; private set; }
        public float StartScale { get; private set; }
        public float FinishScale { get; private set; }
        public Vector2 StartOffset { get; private set; }
        public Vector2 FinishOffset { get; private set; }
        public float Distance { get; private set; }
        public OnEnterCameraTrigger(int toRight, float startScale, float finishScale, Vector2 startOff, Vector2 finishOff, float distance)
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