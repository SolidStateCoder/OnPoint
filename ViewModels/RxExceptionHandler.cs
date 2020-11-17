using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Concurrency;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// Determines how <see cref="IObservable{T}"/> exceptions are handled by Reactive UI.
    /// </summary>
    public class RxExceptionHandler : IObserver<Exception>
    {
        public void OnNext(Exception value)
        {
            if (Debugger.IsAttached) Debugger.Break();
            RxApp.MainThreadScheduler.Schedule(() => { throw value; });
        }

        public void OnError(Exception error)
        {
            if (Debugger.IsAttached) Debugger.Break();
            RxApp.MainThreadScheduler.Schedule(() => { throw error; });
        }

        public void OnCompleted()
        {
            if (Debugger.IsAttached) Debugger.Break();
            RxApp.MainThreadScheduler.Schedule(() => { throw new NotImplementedException(); });
        }
    }
}