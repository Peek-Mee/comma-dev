using System.Collections;
using UnityEngine;

namespace Comma.Global.PubSub
{
   public struct OnPlayerUsePortal
    {
        public Vector3 Destination { get; private set; }

        public OnPlayerUsePortal(Vector3 destination)
        {
            Destination = destination;
        }
    }
}