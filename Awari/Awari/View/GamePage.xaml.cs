using DLToolkit.Forms.Controls;
using Xamarin.Forms;

namespace Awari.View
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            InitializeComponent();
            FlowListView.Init(); // a külső vezérlő inicializálása
        }
    }
}
