using PuzzlescapeGames.Services.AudioService;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Game.Managers.AudioManager
{
    [ CreateAssetMenu( fileName = "AudioManagerInstaller", menuName = "Installers/AudioManagerInstaller" ) ]
    public sealed class AudioManagerInstaller : ScriptableObjectInstaller< AudioManagerInstaller >
    {
        [ SerializeField ] private int _poolSize = 2;
        [ SerializeField ] private AudioMixer _audioMixer;
        [ SerializeField ] private AudioSettings _audioSettings;

        public override void InstallBindings()
        {
            new AudioServiceCustomInstaller( _poolSize, _audioMixer ).Install( Container );
            Container.BindInstance( _audioSettings );
            Container.BindInterfacesAndSelfTo< AudioManager >().AsSingle();
        }
    }
}