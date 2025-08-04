using System;
using UnityEngine;

namespace Game.Core.Environment
{
    public sealed class ClockObject : MonoBehaviour
    {
        private const float HoursToDegrees = 360f / 12f;
        private const float MinutesToDegrees = 360f / 60f;
        private const float SecondsToDegrees = 360f / 60f;

        [ SerializeField ] private Transform _hours;
        [ SerializeField ] private Transform _minutes;
        [ SerializeField ] private Transform _seconds;
        [ SerializeField ] private bool _analog = true;

        private void Update()
        {
            if ( _analog )
            {
                TimeSpan timespan = DateTime.Now.TimeOfDay;
                _hours.localRotation = Quaternion.Euler( 0, 0, (float)timespan.TotalHours * HoursToDegrees );
                _minutes.localRotation = Quaternion.Euler( 0, 0, (float)timespan.TotalMinutes * MinutesToDegrees );
                _seconds.localRotation = Quaternion.Euler( 0, 0, (float)timespan.TotalSeconds * SecondsToDegrees );
            }
            else
            {
                DateTime time = DateTime.Now;
                _hours.localRotation = Quaternion.Euler( 0, 0, time.Hour * HoursToDegrees );
                _minutes.localRotation = Quaternion.Euler( 0, 0, time.Minute * MinutesToDegrees );
                _seconds.localRotation = Quaternion.Euler( 0, 0, time.Second * SecondsToDegrees );
            }
        }
    }
}