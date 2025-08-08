using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Moduls.Light
{
    public static class LightFlicker
    {
        public static async UniTask FlickerAndGrowingIntensity( List< Lamp > lamps, LightFlickerSettings settings )
        {
            float startTime = Time.realtimeSinceStartup;
            float elapsed = 0f;

            // Сохраняем начальную интенсивность каждого света
            Dictionary< Lamp, float > originalIntensities = new();
            foreach ( var lamp in lamps )
            {
                originalIntensities[ lamp ] = lamp.Light.intensity;
            }

            while ( elapsed < settings.FlickerDuration )
            {
                float t = elapsed / settings.FlickerDuration;

                foreach ( var lamp in lamps )
                {
                    float start = originalIntensities[ lamp ];
                    float target = Mathf.Lerp( start, start * 3f, t );
                    lamp.Light.intensity = target;
                }

                // ВЫКЛЮЧИТЬ
                LampUtils.SetLightsEnabled( lamps, false );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );

                // ВКЛЮЧИТЬ
                LampUtils.SetLightsEnabled( lamps, true );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );

                elapsed = Time.realtimeSinceStartup - startTime;
                await UniTask.Yield();
            }

            foreach ( var lamp in lamps )
            {
                lamp.Enable( false );
                lamp.Light.intensity = originalIntensities[ lamp ];
            }
        }

        public static async UniTask FlickerAndTurnOff( List< Lamp > lamps, LightFlickerSettings settings )
        {
            float startTime = Time.realtimeSinceStartup;

            while ( Time.realtimeSinceStartup - startTime < settings.FlickerDuration )
            {
                LampUtils.SetLightsEnabled( lamps, false );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );

                LampUtils.SetLightsEnabled( lamps, true );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );
            }

            LampUtils.SetLightsEnabled( lamps, false );
        }
    }
}