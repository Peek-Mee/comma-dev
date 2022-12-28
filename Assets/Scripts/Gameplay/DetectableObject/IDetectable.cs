using UnityEngine;

namespace Comma.Gameplay.DetectableObject
{
    public interface IDetectable
    {
        public void Interact();
        public Vector3 GetPosition();
        public string GetObjectId();
    }
}