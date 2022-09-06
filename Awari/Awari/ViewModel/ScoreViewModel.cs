using System;
using System.Collections.Generic;
using System.Text;
using Awari.Model;

namespace Awari.ViewModel
{
    public class ScoreViewModel : ViewModelBase
    {

        private string _bgColor = "White";
        private string _value = "0";
        private Int32 x = 0;

        public Int32 X
        {
            get => x;
            set { x = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }

        public string BgColor
        {
            get => _bgColor;
            set { _bgColor = value; OnPropertyChanged(); }
        }

        public DelegateCommand ScoreClickCommand { get; set; }

        /*
        public ScoreViewModel(Int32 _x,AwariGameModel model)
        {
            ScoreClickCommand = new DelegateCommand(_ => ScoreClicked?.Invoke(this, null));
            x = _x;
            _model = model;
            _value = _model.Table.GetValue(x).ToString();
            if (0<=x && x<=_model.Table.TableSize/2 )
            {
                BgColor = "Red";
            }
            else if (_model.Table.TableSize / 2 < x && x <=_model.Table.TableSize+1 )
            {
                BgColor = "Blue";
            }
            
        }*/


    }
}
