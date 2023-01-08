using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Movements
{
    public class MovementHandler : MonoBehaviour
    {
        public Vector3 Movement { get; private set; }

        private readonly List<IMovementModifier> _modifiers = new();

        public void AddModifier(IMovementModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(IMovementModifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        private void Update()
        {
            Movement = Vector3.zero;
            
            foreach (var modifier in _modifiers)
            {
                Movement += modifier.Value;
            }
        }
    }
}