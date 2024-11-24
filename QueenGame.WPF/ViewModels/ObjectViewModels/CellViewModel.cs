using QueenGame.Game.Models;
using QueenGame.Utils.AppColorConverter;
using QueenGame.WPF.Commands;
using QueenGame.WPF.Stores;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace QueenGame.WPF.ViewModels.ObjectViewModels
{
    public class CellViewModel: BaseViewModel
    {
        private Cell cell;
        private readonly LevelStore levelStore;
        private readonly LevelViewModel levelViewModel;

        //public int ImgWidth { get; set; }

        private Brush originalColor;
        private Brush darkenedColor;
        
        private Brush _Color;
        public Brush Color
        {
            get { return _Color; }
            set 
            { 
                _Color = value; 
                OnPropertyChanged(nameof(Color));
            }  
        }
               
        private CellState _defaultCellState;
        public CellState CellState => cell.State;

        private ImageSource _Filling;
        public ImageSource Filling
        {
            get { return _Filling; }
            set
            { 
                _Filling = value;
                OnPropertyChanged(nameof(Filling));
            }
        }

        public ICommand ChangeCellStateCommand { get; }

        public CellViewModel(
            Cell cell,
            LevelStore levelStore,
            LevelViewModel levelViewModel)
        {
            this.cell = cell;
            this.levelStore = levelStore;
            this.levelViewModel = levelViewModel;
            _defaultCellState = cell.State;

            System.Drawing.Color? color = levelStore.GetGroupColor(cell.Group);
            if (color != null)
            {
                originalColor = new SolidColorBrush(color.ToMediaColor() ?? Colors.Transparent);
                darkenedColor = new SolidColorBrush(AppColorConverter.Darken(color.Value).ToMediaColor());
            }
            else 
            { 
                originalColor = new SolidColorBrush(Colors.Transparent);
                darkenedColor = new SolidColorBrush(Colors.Transparent);
            
            }
            Color = originalColor;

            ChangeCellStateCommand = new ChangeCellStateCommand(this, levelViewModel);

            LoadCurrentCellStateFilling();
        }

        public void ChangeCellState() 
        {
            if (cell.IsStateChangable)
            {

                if (levelStore.CurrentLevelState == LevelState.LOADED)
                {
                    levelViewModel.Duration = 1;
                    levelStore.StartGame();
                }

                switch (CellState)
                {
                    case CellState.NOT_SELECTED:
                        levelStore.ChangeCellSate(cell.Row, cell.Column, CellState.MARKED);
                        break;
                    case CellState.SELECTED:
                        levelStore.ChangeCellSate(cell.Row, cell.Column, CellState.NOT_SELECTED);
                        break;
                    case CellState.MARKED:
                        levelStore.ChangeCellSate(cell.Row, cell.Column, CellState.SELECTED);
                        break;
                    case CellState.UNAVAILABLE:
                        break;
                }

                LoadCurrentCellStateFilling();

                OnPropertyChanged(nameof(CellState));
            }
        }

        private void LoadCurrentCellStateFilling()
        {
            switch (cell.State)
            {
                case CellState.NOT_SELECTED:
                    Filling = (ImageSource)Application.Current.FindResource("CellNotSelectedSvgDrawingImage"); 
                    break;
                case CellState.MARKED:
                    Filling = (ImageSource)Application.Current.FindResource("CellMarkedSvgDrawingImage");
                    break;
                case CellState.SELECTED:
                    Filling = (ImageSource)Application.Current.FindResource("CellSelectedSvgDrawingImage");
                    break;
                case CellState.UNAVAILABLE:
                    break;
            }
        }
    }
}
