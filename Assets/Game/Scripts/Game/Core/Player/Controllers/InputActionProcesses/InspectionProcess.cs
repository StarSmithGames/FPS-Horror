using Game.Core.UI;
using Game.Core.World.InspectionSystem;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player.InputActionProcesses
{
    public sealed class InspectionProcess
    {
        public event Action< IInspectable > OnCompleted;
        
        private UIInfoButton _ui;
        private IInspectable _inspectable;

        private readonly InputHolder _inputInspect;
        
        public InspectionProcess()
        {
            _inputInspect = new( InputManager.Inputs.Player.Inspect, Inspection );
        }
        
        public void Enable( UIInfoButton ui, IInspectable inspectable )
        {
            _ui = ui ?? throw new ArgumentNullException( nameof(ui) );
            _inspectable = inspectable ?? throw new ArgumentNullException( nameof(inspectable) );
            _inputInspect.Enable();
        }

        public void Disable()
        {
            _inputInspect.Disable();
            _ui = null;
            _inspectable = null;
        }
        
        private void Inspection()
        {
            OnCompleted?.Invoke( _inspectable );
        }
    }
}