using System;

public interface IObserver<T> where T : EventArgs
{
    void OnStateChanged(T eventArgs);
}

public interface IObservable<T>
{
    event Action<T> StateChanged;
}
