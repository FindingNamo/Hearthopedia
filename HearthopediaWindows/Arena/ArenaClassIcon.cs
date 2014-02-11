using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Arena
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

        private void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
