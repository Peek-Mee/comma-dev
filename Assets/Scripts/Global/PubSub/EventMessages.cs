using System.Collections;
using Comma.Gameplay.Environment;
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
        public bool Jumping { get; private set; }

        public OnPlayerJump(bool jumping)
        {
            Jumping = jumping;
        }
    }

    public struct OnPlayerInteract
    {
        
    }

    public struct OnPlayerSprint
    {
        public bool Sprinting { get; private set; }

        public OnPlayerSprint(bool sprinting)
        {
            Sprinting = sprinting;
        }
    }
    public struct OnCutSceneTrigger
    {
        public CutSceneData Data { get; private set; }
        public OnCutSceneTrigger(CutSceneData data)
        {
            Data = data;
        }
    }
}