using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    public interface IMoveableObject : IDetectable
    {
       public void UnInteract();
        public void GetDetection(Rigidbody2D rigidbody, float direction);
    }
}