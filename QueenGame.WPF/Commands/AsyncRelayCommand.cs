namespace QueenGame.WPF.Commands
{
    public class AsyncRelayCommand : AsyncBaseCommand
    {
        private Func<Task>? onCallbackAction;

        public AsyncRelayCommand(Func<Task>? onCallbackAction, Action<Exception>? onExceptionAction) : base(onExceptionAction)
        {
            this.onCallbackAction = onCallbackAction;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (onCallbackAction != null)
            {
                await onCallbackAction();
            }
        }
    }
}
