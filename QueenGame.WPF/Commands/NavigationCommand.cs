using QueenGame.WPF.Services;
using QueenGame.WPF.ViewModels;

namespace QueenGame.WPF.Commands
{
    public class NavigationCommand<TViewModel> : BaseCommand where TViewModel : BaseViewModel
    {
        private NavigationService<TViewModel> navigationService;

        public NavigationCommand(NavigationService<TViewModel> navigationService)
        {
            this.navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            navigationService?.Navigate();
        }
    }
}
