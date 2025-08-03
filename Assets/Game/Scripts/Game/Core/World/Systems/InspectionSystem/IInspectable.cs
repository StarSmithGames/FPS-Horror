using Game.Core.World.InteractionSystem;
using UnityEngine;

namespace Game.Core.World.InspectionSystem
{
    public interface IInspectable : IInteractable
    {
        InspectionSettings InspectionSettings { get; }
        
        Transform TransformInspection { get; }
    }
}