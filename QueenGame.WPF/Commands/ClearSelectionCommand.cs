﻿using QueenGame.WPF.Stores;
using QueenGame.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueenGame.WPF.Commands
{
    public class ClearSelectionCommand : BaseCommand
    {
        private LevelViewModel viewModel;
        private LevelStore levelStore;

        public ClearSelectionCommand(LevelViewModel viewModel, LevelStore levelStore)
        {
            this.viewModel = viewModel;
            this.levelStore = levelStore;

            viewModel.PropertyChanged += OnLevelViewModelPropertyChanged;
        }

        private void OnLevelViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(LevelViewModel.LevelState):
                    OnCanExecuteChanged();
                    break;
            }
        }

        public override void Execute(object? parameter)
        {
            levelStore.ResetCellField();
            viewModel.OnCellsChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter)
                && (viewModel.LevelState == Stores.LevelState.LOADED
                    || viewModel.LevelState == Stores.LevelState.STARTED);
        }
    }
}
