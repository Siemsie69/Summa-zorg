using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Zorgdossier.Helpers;
using Zorgdossier.Models;
using Zorgdossier.Views;

namespace Zorgdossier.ViewModels
{
    internal class MainViewModel : ObservableObject
    {
        private IAppNavigation _appNavigation;
        private UserMessage _userMessage;
        private bool _isMenuExpanded = true;

        public MainViewModel(IAppNavigation appNavigation, UserMessage userMessage)
        {
            _appNavigation = appNavigation;
            _userMessage = userMessage;
            _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage);

            ShowHomeCommand = new RelayCommand(ExecuteShowHome);
            ShowDossiersCommand = new RelayCommand(ExecuteShowDossiers);
            ShowExplanationCommand = new RelayCommand(ExecuteShowExplanation);
            ShowCreditsCommand = new RelayCommand(ExecuteShowCredits);
            ToggleMenuCommand = new RelayCommand(ExecuteToggleMenu);
            ShowSettingsCommand = new RelayCommand(ExecuteShowSettings); 
        }

        public MainViewModel() { }

        public IAppNavigation AppNavigation
        {
            get => _appNavigation;
        }

        public UserMessage UserMessage
        {
            get => _userMessage;
            set
            {
                _userMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsMenuExpanded
        {
            get => _isMenuExpanded;
            set
            {
                _isMenuExpanded = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMenuCollapsed));
            }
        }

        public bool IsMenuCollapsed => !IsMenuExpanded;

        public ICommand ShowHomeCommand { get; }
        public ICommand ShowDossiersCommand { get; }
        public ICommand ShowExplanationCommand { get; }
        public ICommand ShowCreditsCommand { get; }
        public ICommand ToggleMenuCommand { get; }
        public ICommand ShowSettingsCommand { get; }

        private void ExecuteShowHome(object? obj)
        {
            _appNavigation.ActiveViewModel = new HomeViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowDossiers(object? obj)
        {
            _appNavigation.ActiveViewModel = new DossiersViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowExplanation(object? obj)
        {
            _appNavigation.ActiveViewModel = new ExplanationViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteShowCredits(object? obj)
        {
            _appNavigation.ActiveViewModel = new CreditsViewModel(_appNavigation, _userMessage);
        }

        private void ExecuteToggleMenu(object? obj)
        {
            var targetWidth = IsMenuExpanded ? new GridLength(80) : new GridLength(200);
            var menuColumn = (Application.Current.MainWindow as MainView)?.FindName("MenuColumn") as ColumnDefinition;

            if (menuColumn != null)
            {
                var gridLengthAnimation = new GridLengthAnimation
                {
                    From = menuColumn.Width,
                    To = targetWidth,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                var storyboard = new Storyboard();
                storyboard.Children.Add(gridLengthAnimation);

                Storyboard.SetTarget(gridLengthAnimation, menuColumn);
                Storyboard.SetTargetProperty(gridLengthAnimation, new PropertyPath(ColumnDefinition.WidthProperty));

                storyboard.Begin();
            }

            IsMenuExpanded = !IsMenuExpanded;
        }


        private void ExecuteShowSettings(object? obj)
        {
            _appNavigation.ActiveViewModel = new SettingsViewModel(_appNavigation, _userMessage);
        }
    }
}