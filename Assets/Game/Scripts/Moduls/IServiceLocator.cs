namespace Game
{
    public interface IServiceLocator
    {
        void Clear();
        void Register< T >( T service );
        T GetAs< T >();
    }
}