namespace Game.Managers.InputManager
{
    public struct BindingInfo
    {
        public string Device; //"Keyboard"
        public string Path; //"<Keyboard>/w"
        public string Key; //"W"

        public BindingInfo( string device, string path, string key )
        {
            Device = device;
            Path = path;
            Key = key;
        }
    }
}