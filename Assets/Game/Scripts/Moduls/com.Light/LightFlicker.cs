using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Moduls.Light
{
    public static class LightFlicker
    {
        public static async UniTask FlickerAndTurnOff( List< UnityEngine.Light > lights, LightFlickerSettings settings )
        {
            Debug.LogError( "Start" );

            float startTime = Time.realtimeSinceStartup;

            while ( Time.realtimeSinceStartup - startTime < settings.FlickerDuration )
            {
                SetLightsEnabled( lights, false );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );

                SetLightsEnabled( lights, true );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );
            }

            Debug.LogError( "End" );

            SetLightsEnabled( lights, false );
        }

        private static void SetLightsEnabled( List< UnityEngine.Light > lights, bool state )
        {
            foreach ( var light in lights )
            {
                if ( light != null )
                {
                    light.enabled = state;
                }
            }
        }
    }
}