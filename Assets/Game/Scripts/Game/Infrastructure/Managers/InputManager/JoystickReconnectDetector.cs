using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Game.Managers.InputManager
{
    public sealed class JoystickReconnectDetector
    {
        public static event Action< string > OnConnected;
        public static event Action< string > OnDisconnected;
        
        public static bool IsConnected => ;

        private static float _pollInterval = 0.25f;
        private static float _nextPoll;
        private static Dictionary< int, string > _prev = new();

        public static void Initialize( CancellationToken cancellationToken = default )
        {
            Snapshot( _prev, Input.GetJoystickNames() );
            Tick( cancellationToken ).Forget();
        }

        private static async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                if ( Time.unscaledTime >= _nextPoll )
                {
                    _nextPoll = Time.unscaledTime + _pollInterval;
                    
                    var nowNames = Input.GetJoystickNames();
                    var now = new Dictionary< int, string >();
                    Snapshot( now, nowNames );

                    var allIndices = _prev.Keys.Union( now.Keys ).ToArray();
                    foreach ( var i in allIndices )
                    {
                        _prev.TryGetValue( i, out var was );
                        now.TryGetValue( i, out var cur );

                        bool had = !string.IsNullOrEmpty( was );
                        bool has = !string.IsNullOrEmpty( cur );

                        if ( had && !has ) // исчез
                        {
                            OnDisconnected?.Invoke( was );
                        }

                        if ( !had && has ) // появился
                        {
                            OnConnected?.Invoke( cur );
                        }

                        if ( had && has && was != cur ) // переопределился (драйвер/порт)
                        {
                            OnDisconnected?.Invoke( was );
                            OnConnected?.Invoke( cur );
                        }
                    }

                    _prev.Clear();
                    foreach ( var kv in now )
                    {
                        _prev[ kv.Key ] = kv.Value;
                    }
                }

                await UniTask.Yield();
            }
        }

        private static void Snapshot( Dictionary< int, string > dst, string[] names )
        {
            dst.Clear();
            for ( int i = 0; i < names.Length; i++ )
            {
                dst[ i ] = names[ i ];
            }
        }
    }
}