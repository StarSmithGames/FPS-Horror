using System.Collections.Generic;

namespace Moduls.Light
{
    public static class LampUtils
    {
        public static void SetLightsEnabled( List< Lamp > lamps, bool trigger )
        {
            foreach ( var lamp in lamps )
            {
                lamp.Enable( trigger );
            }
        }
    }
}