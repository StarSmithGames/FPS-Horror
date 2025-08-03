using UnityEngine;

namespace Game.Core.World.InspectionSystem
{
    public static class InspectionAxisExtensions
    {
        public static Vector3 ToDirection( this InspectionAxis axis )
        {
            return axis switch
            {
                InspectionAxis.Up => Vector3.up,
                InspectionAxis.Down => Vector3.down,
                InspectionAxis.Forward => Vector3.forward,
                InspectionAxis.Back => Vector3.back,
                InspectionAxis.Right => Vector3.right,
                InspectionAxis.Left => Vector3.left,
                _ => Vector3.up
            };
        }
    }
}