using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class EnumerableOption<TEnum> : INotifyPropertyChanged
    {
        public EnumerableOption(string name, TEnum enumValue, bool defaultvalue)
        {
            Name = name;
            EnumValue = enumValue;
            _value = defaultvalue;
        }

        public string Name { get; set; }
        public TEnum EnumValue { get; set; }
        
        private bool _value;
        public bool Value 
        { 
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            } 
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));

            FilterManager.Instance.Dirty = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}