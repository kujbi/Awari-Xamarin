using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Awari.Model;

namespace Awari.ViewModel
{
    public class AwariViewModel : ViewModelBase
    {

        private AwariGameModel _model;

        public Int32 sCC { get; set; } = 4;
        public Boolean second = false;
        public DelegateCommand NewGameCommand { get; private set; }
        public DelegateCommand LoadGameCommand { get; private set; }
        public DelegateCommand SaveGameCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        public ObservableCollection<ScoreViewModel> ScoreRed { get; set; } = new ObservableCollection<ScoreViewModel>();
        public ObservableCollection<ScoreViewModel> ScoreBlue { get; set; } = new ObservableCollection<ScoreViewModel>();

        public ObservableCollection<ScoreViewModel> Scores { get; set; } = new ObservableCollection<ScoreViewModel>();

        public Boolean IsGameFour
        {
            get { return _model.GameDifficulty == GameDifficulty.Negyes; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Negyes)
                    return;

                _model.GameDifficulty = GameDifficulty.Negyes;
                OnPropertyChanged("IsGameFour");
                OnPropertyChanged("IsGameEigth");
                OnPropertyChanged("IsGameTwelve");               
                sCC = 2;


            }
        }

        public Boolean IsGameEigth
        {
            get { return _model.GameDifficulty == GameDifficulty.Nyolcas; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Nyolcas)
                    return;

                _model.GameDifficulty = GameDifficulty.Nyolcas;
                OnPropertyChanged("IsGameFour");
                OnPropertyChanged("IsGameEigth");
                OnPropertyChanged("IsGameTwelve");                
                sCC = 4;              


            }
        }

        public Boolean IsGameTwelve
        {
            get { return _model.GameDifficulty == GameDifficulty.Tizenkettes; }
            set
            {
                if (_model.GameDifficulty == GameDifficulty.Tizenkettes)
                    return;

                _model.GameDifficulty = GameDifficulty.Tizenkettes;
                OnPropertyChanged("IsGameFour");
                OnPropertyChanged("IsGameEigth");
                OnPropertyChanged("IsGameTwelve");
                sCC = 6;
               
                
            }
        }

        public event EventHandler NewGame;
        public event EventHandler LoadGame;
        public event EventHandler SaveGame;
        public event EventHandler ExitGame;

        public AwariViewModel(AwariGameModel model)
        {

            _model = model;
            _model.GameOver += new EventHandler<AwariEventArgs>(Model_GameOver);
            


            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());
            TableSetting();
            RefereshTable();
        }


       
        private void Model_GameOver(object sender, AwariEventArgs e)
        {
            
        }
        private void OnNewGame()
        {
            if (NewGame != null)
            {
                NewGame(this, EventArgs.Empty);
                TableSetting();
                OnPropertyChanged(nameof(ScoreBlue));
                OnPropertyChanged(nameof(ScoreRed));
                OnPropertyChanged(nameof(Scores));
                OnPropertyChanged("sCC");
            }
        }

        private void OnLoadGame()
        {
            if (LoadGame != null)
            {
                //OnPropertyChanged hamarabb fut le, mint a LoadGame.
                LoadGame(this, EventArgs.Empty);
                OnPropertyChanged(nameof(ScoreBlue));
                OnPropertyChanged(nameof(ScoreRed));
                OnPropertyChanged(nameof(Scores));

            }
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
            {
                SaveGame(this, EventArgs.Empty);
                OnPropertyChanged(nameof(ScoreBlue));
                OnPropertyChanged(nameof(ScoreRed));
                OnPropertyChanged(nameof(Scores));
            }
        }

        private void OnExitGame()
        {
            if (ExitGame != null)
            {
                ExitGame(this, EventArgs.Empty);
            }
        }


        private void ButtonRestrict()
        {
            if (_model.CurrentPlayer == 0)
            {
                for (int i = 0 ; i < _model.Table.NNumber/2; i++)
                {
                    Scores[i].BgColor = "CornflowerBlue";
                }
                for (int i = _model.Table.NNumber / 2; i < _model.Table.NNumber; i++)
                {
                    Scores[i].BgColor = "Green";
                }              
            }
            if (_model.CurrentPlayer == 1)
            {
                for (int i = 0; i < _model.Table.NNumber / 2; i++)
                {
                    Scores[i].BgColor = "Green";
                }
                for (int i = _model.Table.NNumber / 2; i < _model.Table.NNumber; i++)
                {
                    Scores[i].BgColor = "Red";
                }
            }

        }

        private void myon_Click(Int32 x)
        {
            /*
            if (_model.CurrentPlayer == 1 && x >= _model.Table.NNumber/2)
            {
                return;
            }
            if (_model.CurrentPlayer == 0 && x < _model.Table.NNumber / 2)
            {
                return;
            }
            if (x >= _model.Table.NNumber / 2)
            {
                x=x - 3;
            }
            if (x < _model.Table.NNumber / 2)
            {
                x=_model.Table.NNumber - x;
            }
            if (second)
                second = _model.StonePacking(_model.CurrentPlayer, x, false);
            else
            {
                second = _model.StonePacking(_model.CurrentPlayer, x, true);
            }
            if (!second)
            {
                _model.ChangePlayer();
            }*/
                            
            if (_model.Table.NNumber+1==x || x == _model.Table.NNumber / 2)
            {
                return;
            }
            if (_model.CurrentPlayer == 0 && x > _model.Table.NNumber / 2)
            {
                return;
            }
            if (_model.CurrentPlayer == 1 && x < _model.Table.NNumber / 2)
            {
                return;
            }
            if (_model.Table.GetValue(x) == 0)
            {
                return;
            }
            
            if (second)
                second = _model.StonePacking(_model.CurrentPlayer, x, false);
            else
            {
                second = _model.StonePacking(_model.CurrentPlayer, x, true);
            }
            if (!second)
            {
                _model.ChangePlayer();
            }

            ButtonRestrict();
            RefereshTable();
            /*
            //RedScoreCup
            ScoreRed.Clear();
            var seged = new ScoreViewModel((_model.Table.TableSize / 2), _model);
            ScoreRed.Add(seged);
            
            //Blue Score Cup
            ScoreBlue.Clear();
            var seged2 = new ScoreViewModel(_model.Table.NNumber + 1, _model);
            ScoreBlue.Add(seged2);
            
            //ScoreCups
            Scores.Clear();
            for (int i = 0; i < _model.Table.NNumber / 2; i++)
            {
                var svm = new ScoreViewModel(i, _model);
                svm.ScoreClicked += myon_Click;
                Scores.Add(svm);
            }
            //Generating Blue cups
            for (int i = _model.Table.NNumber / 2 + 1; i < _model.Table.NNumber + 1; i++)
            {
                var svm = new ScoreViewModel(i, _model);
                svm.ScoreClicked += myon_Click;
                Scores.Add(svm);
            }
            */
            Boolean over = false;
            over = _model.IsGameOver;
            if (over)
            {
                /// Red Player Cup Score compared to Blue Player's Cup
                if (_model.Table.GetValue(_model.Table.NNumber / 2) > _model.Table.GetValue(_model.Table.TableSize - 1))
                {
                    _model.OnGameOver(0);
                }
                else if (_model.Table.GetValue(_model.Table.NNumber / 2) < _model.Table.GetValue(_model.Table.TableSize - 1))
                {
                    _model.OnGameOver(1);
                }
                else
                {
                    _model.OnGameOver(2);
                }
            }

        }

        public void TableSetting()
        {
            //RedScoreCup
            ScoreRed.Clear();
            ScoreRed.Add(new ScoreViewModel
            {
                BgColor = "Red",
                Text = _model.Table.GetValue(_model.Table.NNumber / 2).ToString(),
                X = _model.Table.NNumber / 2,
                ScoreClickCommand = new DelegateCommand(param => myon_Click(Convert.ToInt32(param)))
            });          

            //Blue Score Cup
            ScoreBlue.Clear();
            ScoreBlue.Add(new ScoreViewModel
            {
                BgColor = "CornflowerBlue",
                Text = _model.Table.GetValue(_model.Table.NNumber + 1).ToString(),
                X = _model.Table.NNumber + 1,
                ScoreClickCommand = new DelegateCommand(param => myon_Click(Convert.ToInt32(param)))
            });

            Scores.Clear();

            //Generating Blue cups first, cause GamePage loads the Scores from Scores List
            for (int i = _model.Table.NNumber; i > _model.Table.NNumber / 2; i--)
            {
                Scores.Add(new ScoreViewModel 
                { 
                    BgColor= "CornflowerBlue",
                    Text=_model.Table.GetValue(i).ToString(),
                    X=i,
                    ScoreClickCommand = new DelegateCommand(param => myon_Click(Convert.ToInt32(param)))                        
                });
            }
            //Generating Red cups          
            
            for (int i = 0; i < _model.Table.NNumber / 2; i++)
            {
                Scores.Add(new ScoreViewModel
                {
                    BgColor = "Red",
                    Text = _model.Table.GetValue(i).ToString(),
                    X = i,
                    ScoreClickCommand = new DelegateCommand(param => myon_Click(Convert.ToInt32(param)))
                    
                });

            }
            ButtonRestrict();
            RefereshTable();
            OnPropertyChanged(nameof(Scores));
            OnPropertyChanged(nameof(ScoreRed));
            OnPropertyChanged(nameof(ScoreBlue));
            return;
        }

        public void RefereshTable()
        {
            ScoreBlue[0].Text = _model.Table.GetValue(_model.Table.NNumber + 1).ToString();
            ScoreRed[0].Text = _model.Table.GetValue(_model.Table.NNumber/2).ToString();
            for (int i = 0; i <_model.Table.NNumber/2; i++)
            {
                Scores[i].Text = _model.Table.GetValue(_model.Table.NNumber - i).ToString();
            }
            for (int i = _model.Table.NNumber / 2; i < _model.Table.NNumber; i++)
            {
                Scores[i].Text = _model.Table.GetValue(i - (_model.Table.NNumber / 2)).ToString();
            }
           

        }
        
    }
}
