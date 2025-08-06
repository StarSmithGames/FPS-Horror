using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Moduls.Light
{
    public static class LightFlicker
    {
        public static async UniTask FlickerAndGrowingIntensity( List< UnityEngine.Light > lights, LightFlickerSettings settings )
        {
            float startTime = Time.realtimeSinceStartup;
            float elapsed = 0f;

            // Сохраняем начальную интенсивность каждого света
            Dictionary< UnityEngine.Light, float > originalIntensities = new();
            foreach ( var light in lights )
            {
                if ( light != null )
                    originalIntensities[ light ] = light.intensity;
            }

            while ( elapsed < settings.FlickerDuration )
            {
                float t = elapsed / settings.FlickerDuration;

                foreach ( var light in lights )
                {
                    if ( light == null ) continue;

                    float start = originalIntensities[ light ];
                    float target = Mathf.Lerp( start, start * 3f, t );
                    light.intensity = target;
                }

                // ВЫКЛЮЧИТЬ
                SetLightsEnabled( lights, false );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );

                // ВКЛЮЧИТЬ
                SetLightsEnabled( lights, true );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );

                elapsed = Time.realtimeSinceStartup - startTime;
                await UniTask.Yield();
            }

            // В конце — выключаем и сбрасываем интенсивность
            foreach ( var light in lights )
            {
                if ( light == null ) continue;

                light.enabled = false;
                light.intensity = originalIntensities[ light ];
            }
        }

        public static async UniTask FlickerAndTurnOff( List< UnityEngine.Light > lights, LightFlickerSettings settings )
        {
            float startTime = Time.realtimeSinceStartup;

            while ( Time.realtimeSinceStartup - startTime < settings.FlickerDuration )
            {
                SetLightsEnabled( lights, false );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );

                SetLightsEnabled( lights, true );
                await UniTask.WaitForSeconds( Random.Range( settings.MinFlickerInterval, settings.MaxFlickerInterval ) );
            }

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