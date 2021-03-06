﻿using Autofac;
using OnPoint.Universal;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace OnPoint.ViewModels
{
    /// <summary>
    /// A standard <see cref="ViewModelBase"/> that has one "child".
    /// </summary>
    /// <typeparam name="T">The type of the child <see cref="Content"/></typeparam>
    public class ContentVM<T> : ViewModelBase
    {
        /// <summary>
        /// The single child VM.
        /// </summary>
        public T Content { get => _Content; set => this.RaiseAndSetIfChanged(ref _Content, value); }
        private T _Content = default;

        // IObservables that can be used to control when ICommands can be executed.
        protected IObservable<bool> WhenContentNull { get; }
        protected IObservable<bool> WhenContentNotNull { get; }
        protected IObservable<bool> WhenContentNull_NotBusy { get; }
        protected IObservable<bool> WhenContentNotNull_NotBusy { get; }

        public ContentVM(T content) : this(content, null, 0, null, null) { }

        public ContentVM(T content = default, ILifetimeScope lifetimeScope = default, uint viewModelTypeId = default, IScreen screen = default, string urlPathSegment = default) : base(lifetimeScope, viewModelTypeId, screen, urlPathSegment)
        {
            Content = content;
            WhenContentNull = this.WhenAny(vm => vm.Content, content => content.Value == null);
            WhenContentNotNull = this.WhenAny(vm => vm.Content, content => content.Value != null);
            WhenContentNull_NotBusy = this.WhenAny(vm => vm.Content, vm => vm.IsBusy, (content, busy) => content.Value == null && !busy.Value);
            WhenContentNotNull_NotBusy = this.WhenAny(vm => vm.Content, vm => vm.IsBusy, (content, busy) => content.Value != null && !busy.Value);
        }

        protected override Task<ExecutionResultMessage> Activated(CompositeDisposable disposable)
        {
            this.WhenAnyValue(vm => vm.Content)
                .Subscribe(ContentHasChanged);

            return base.Activated(disposable);
        }

        protected virtual void ContentHasChanged(T content) => Debug("ContentHasChanged {content}.");
    }
}