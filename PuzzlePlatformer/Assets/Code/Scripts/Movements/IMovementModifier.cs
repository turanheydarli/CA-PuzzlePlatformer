using UnityEngine;

namespace Code.Scripts.Movements
{
    public interface IMovementModifier
    {
        public Vector3 Value { get; }
    }
}