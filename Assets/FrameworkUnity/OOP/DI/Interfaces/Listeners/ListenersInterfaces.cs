namespace FrameworkUnity.OOP.Interfaces.Listeners
{
    public interface IGameListener { }

    public interface IInitGameListener : IGameListener
    {
        void OnInitGame();
    }

    public interface IDeInitGameListener : IGameListener
    {
        void OnDeInitGame();
    }

    public interface IPrepareGameListener : IGameListener
    {
        void OnPrepareGame();
    }

    public interface IStartGameListener : IGameListener
    {
        void OnStartGame();
    }

    public interface IFinishGameListener : IGameListener
    {
        void OnFinishGame();
    }

    public interface IPauseGameListener : IGameListener
    {
        void OnPauseGame();
    }

    public interface IWinGameListener : IGameListener
    {
        void OnWinGame();
    }

    public interface ILoseGameListener : IGameListener
    {
        void OnLoseGame();
    }

    public interface IResumeGameListener : IGameListener
    {
        void OnResumeGame();
    }

    public interface IUpdateGameListener : IGameListener
    {
        void OnUpdate(float deltaTime);
    }

    public interface IFixedUpdateGameListener : IGameListener
    {
        void OnFixedUpdate(float fixedDeltaTime);
    }

    public interface ILateUpdateGameListener : IGameListener
    {
        void OnLateUpdate(float deltaTime);
    }
}