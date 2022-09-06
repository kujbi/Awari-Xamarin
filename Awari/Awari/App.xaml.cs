using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Awari.Model;
using Awari.Persistence;
using Awari.View;
using Awari.ViewModel;




namespace Awari
{
    public partial class App : Application
    {
        private IAwariDataAccess _awariDataAccess;
        private AwariGameModel _awariGameModel;
        private AwariViewModel _awariViewModel;
        private GamePage _gamePage;
        private SettingsPage _settingsPage;

        private IStore _store;
        private StoredGameBrowserModel _storedGameBrowserModel;
        private StoredGameBrowserViewModel _storedGameBrowserViewModel;
        private LoadGamePage _loadGamePage;
        private SaveGamePage _saveGamePage;
        private NavigationPage _mainPage;

        public App()
        {
            _awariDataAccess = DependencyService.Get<IAwariDataAccess>(); // az interfész megvalósítását automatikusan megkeresi a rendszer

            _awariGameModel = new AwariGameModel(_awariDataAccess);
            _awariGameModel.GameOver += new EventHandler<AwariEventArgs>(AwariViewModel_GameOver);

            _awariViewModel = new AwariViewModel(_awariGameModel);
            _awariViewModel.NewGame += new EventHandler(AwariViewModel_NewGame);
            _awariViewModel.LoadGame += new EventHandler(AwariViewModel_LoadGame);
            _awariViewModel.SaveGame += new EventHandler(AwariViewModel_SaveGame);
            _awariViewModel.ExitGame += new EventHandler(AwariViewModel_ExitGame);

            _gamePage = new GamePage();
            _gamePage.BindingContext = _awariViewModel;

            _settingsPage = new SettingsPage();
            _settingsPage.BindingContext = _awariViewModel;

            // a játékmentések kezelésének összeállítása
            _store = DependencyService.Get<IStore>(); // a perzisztencia betöltése az adott platformon
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameLoading);
            _storedGameBrowserViewModel.GameSaving += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameSaving);

            _loadGamePage = new LoadGamePage();
            _loadGamePage.BindingContext = _storedGameBrowserViewModel;

            _saveGamePage = new SaveGamePage();
            _saveGamePage.BindingContext = _storedGameBrowserViewModel;

            // nézet beállítása
            _mainPage = new NavigationPage(_gamePage); // egy navigációs lapot használunk fel a három nézet kezelésére

            MainPage = _mainPage;
        }

        protected override void OnStart()
        {
            _awariGameModel.NewGame();
            _awariViewModel.TableSetting();
        }

        protected override void OnSleep()
        {
            try
            {
                Task.Run(async () => await _awariGameModel.SaveGameAsync("SuspendedGame"));
            }
            catch { }
        }

        protected override void OnResume()
        {
            try
            {
                Task.Run(async () =>
                {
                    await _awariGameModel.LoadGameAsync("SuspendedGame");
                    _awariViewModel.TableSetting();
                });
            }
            catch { }
        }

        private void AwariViewModel_NewGame(object sender, EventArgs e)
        {
            _awariGameModel.NewGame();          
        }

        private async void AwariViewModel_LoadGame(object sender, System.EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_loadGamePage); // átnavigálunk a lapra
        }

        private async void AwariViewModel_SaveGame(object sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_saveGamePage); // átnavigálunk a lapra
        }

        private async void AwariViewModel_ExitGame(object sender, EventArgs e)
        {
            await _mainPage.PushAsync(_settingsPage); // átnavigálunk a beállítások lapra
        }


        private async void StoredGameBrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await _awariGameModel.LoadGameAsync(e.Name);
                _awariViewModel.TableSetting();

                // sikeres betöltés
                await _mainPage.PopAsync(); // visszanavigálunk a játék táblára
                await MainPage.DisplayAlert("Awari játék", "Sikeres betöltés.", "OK");
             
            }
            catch
            {
                await MainPage.DisplayAlert("Awari játék", "Sikertelen betöltés.", "OK");
            }
        }

        private async void StoredGameBrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk

            try
            {
                // elmentjük a játékot
                await _awariGameModel.SaveGameAsync(e.Name);
                await MainPage.DisplayAlert("Awari játék", "Sikeres mentés.", "OK");
            }
            catch
            {
                await MainPage.DisplayAlert("Awari játék", "Sikertelen mentés.", "OK");
            }
        }

        private async void AwariViewModel_GameOver(object sender, AwariEventArgs e)
        {

            if (e.WhoWon == 0) //Red Player won.
            {
                await MainPage.DisplayAlert("Awari játék", "Gratulálok a piros játékos nyert!","OK");
            }
            else if (e.WhoWon == 1) //Blue Player won.
            {
                await MainPage.DisplayAlert("Awari játék", "Gratulálok a kék játékos nyert!", "OK");
            }
            else if (e.WhoWon == 2) //Tie
            {
                await MainPage.DisplayAlert("Awari játék", "Gratulálok játékosok, döntetlen lett!", "OK");

            }
            else if (e.WhoWon == 3)
            {
                await MainPage.DisplayAlert("Awari játék", "A játék során váratlan hiba lépett fel.", "OK");

            }
        }

    }
}
