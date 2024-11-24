using QueenGame.WPF.ViewModels;
using QueenGame.WPF.ViewModels.ObjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Commands
{
    public class ChangeCellStateCommand : BaseCommand
    {
        private CellViewModel cellViewModel;
        private readonly LevelViewModel levelViewModel;

        public ChangeCellStateCommand(CellViewModel cellViewModel, LevelViewModel levelViewModel)
        {
            this.cellViewModel = cellViewModel;
            this.levelViewModel = levelViewModel;

            levelViewModel.PropertyChanged += OnLevelViewModelPropertyChanged;
        }

        private void OnLevelViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LevelViewModel.LevelState):
                case nameof(LevelViewModel.IsRequestedBack):
                    OnCanExecuteChanged();
                    break;
            }
        }

        public override void Execute(object? parameter)
        {            
            cellViewModel.ChangeCellState();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && (levelViewModel.LevelState == Stores.LevelState.LOADED
                    || levelViewModel.LevelState == Stores.LevelState.STARTED)
                && !levelViewModel.IsRequestedBack;
        }
    }
}
