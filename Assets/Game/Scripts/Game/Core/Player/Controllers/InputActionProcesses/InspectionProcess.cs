using Game.Core.UI;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player.InputActionProcesses
{
    public sealed class InspectionProcess
    {
        private UIInfoButton _ui;
        private IInspectable _inspectable;

        private readonly InputActionProvider _provider;
        
        public InspectionProcess( Func< bool > breaker = null )
        {
            _provider = new( InputManager.Inputs.Player.Inspect, Inspection, 0.33f, breaker: breaker, progress: InspectionProgress, callback: InspectionFinished );
        }
        
        public void Enable( UIInfoButton ui, IInspectable inspectable )
        {
            _ui = ui ?? throw new ArgumentNullException( nameof(ui) );
            _inspectable = inspectable ?? throw new ArgumentNullException( nameof(inspectable) );
            _provider.Enable();
        }

        public void Disable()
        {
            _provider.Disable();
            _ui = null;
            _inspectable = null;
        }
        
        private void Inspection()
        {
            // _currentInspectable
        }

        private void InspectionProgress( float value )
        {
            _ui.SetFillAmount( value );
        }
        
        private void InspectionFinished( bool result )
        {
            _ui.SetFillAmount( 0 );
        }
    }
}