namespace Game
{
    public interface ICommand
    {
        bool CanExecute();
        void Execute();
        void Undo();
    }
}