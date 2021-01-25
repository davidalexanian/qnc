namespace Converter.Wpf
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    public class AsyncCommand : DependencyObject, ICommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;

        public bool IsExecuting {
            get => (bool)this.GetValue(IsExecutingProperty);
            set => this.SetValue(IsExecutingProperty, value);
        }
        public static readonly DependencyProperty IsExecutingProperty =
            DependencyProperty.Register(nameof(IsExecuting), typeof(bool), typeof(AsyncCommand),
                new PropertyMetadata(false, (obj, e) => (obj as AsyncCommand).RaiseCanExecute()));

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => !IsExecuting && canExecute();

        public async void Execute(object parameter)
        {
            IsExecuting = true;
            try {
                await execute();
            }
            finally {
                IsExecuting = false;
            }
        }

        public void RaiseCanExecute() => CanExecuteChanged?.Invoke(this, new IsExecutingChangedEventArgs(IsExecuting));
    }

    public class IsExecutingChangedEventArgs : EventArgs {
        public IsExecutingChangedEventArgs(bool isExecuting) {
            this.IsExecuting = isExecuting;
        }
        public bool IsExecuting { get; }
    }
}
