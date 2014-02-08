using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

using Hearthopedia;
using Hearthopedia.Arena;
using System.ComponentModel;

namespace HearthopediaWindows
{
    public class ArenaClassIcon : INotifyPropertyChanged
    {
        public CardClass Class { get; set; }

        public string ClassIconPath
        {
            get
            {
                return string.Format("/Assets/Arena/{0}.png", EnumUtilities.GetName(Class));
            }
        }

        private Visibility _visible = Visibility.Collapsed;
        public Visibility Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    OnPropertyChanged("Visible");
                }
            }
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
}

    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ArenaClassPicker : HearthopediaWindows.Common.LayoutAwarePage
    {
        public ObservableCollection<ArenaClassIcon> ArenaClassIconList{ get; set; }

        private List<ArenaClassIcon> _classList;
        private Random _random = new Random();
        public ArenaClassPicker()
        {
            ArenaClassIconList = new ObservableCollection<ArenaClassIcon>();
            _classList = new List<ArenaClassIcon>();

            Array classes = Enum.GetValues(typeof(CardClass));
            foreach (CardClass c in classes)
            {
                if (c != CardClass.Everyone)
                    _classList.Add(new ArenaClassIcon() { Class = c, Visible = Visibility.Visible});
            }

            // Shuffle the list
            for(int i = 0; i < 5; i++)
                _classList.Sort((u, v) => { return _random.Next(-1,1); });

            // Show the first 3
            for(int i = 0; i < 3; i ++)
                ArenaClassIconList.Add(_classList[i]);

            this.InitializeComponent();
            PickerGrid.DataContext = ArenaClassIconList;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 3; i < _classList.Count; i++)
                ArenaClassIconList.Add(_classList[i]);

            ((FrameworkElement)sender).Visibility = Visibility.Collapsed;
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ArenaClassIcon iconTapped = (ArenaClassIcon) ((FrameworkElement)sender).DataContext;
            this.Frame.Navigate(typeof(ArenaPage), (int)iconTapped.Class);
        }


    }
}
