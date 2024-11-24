namespace QueenGame.WPF.Commands
{
    public abstract class AsyncBaseCommand : BaseCommand
    {
        private bool _IsExecuting;
        public bool IsExecuting 
        { 
            get { return _IsExecuting; }
            set 
            {                 
                _IsExecuting = value;
                OnCanExecuteChanged();
            }
        }

        private Action<Exception>? onExceptionAction;

        protected AsyncBaseCommand() : this(null) { }

        protected AsyncBaseCommand(Action<Exception>? onExceptionAction)
        {
            this.onExceptionAction = onExceptionAction;
        }

        public override bool CanExecute(object? parameter)
        {
            return !IsExecuting && base.CanExecute(parameter);
        }

        public override async void Execute(object? parameter)
        {
            IsExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                onExceptionAction?.Invoke(ex);
            }
            finally
            {
                IsExecuting = false;
            }
        }

        public abstract Task ExecuteAsync(object? parameter);
    }
}
