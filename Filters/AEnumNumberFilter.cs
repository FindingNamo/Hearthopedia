using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Hearthopedia.Filters
{
    public abstract class AEnumNumberFilter<EnumType> : ANumberFilter where EnumType : struct, IConvertible
    {
        /// <summary>
        /// The supported operators for number typed filters.
        /// </summary>
        public List<string> SupportedOperators
        {
            get
            {
                return new List<string>()
                    {
                        "equal to",
                        "not equal to ",
                     };
            }
        }

        protected enum NumberFilterOperation
        {
            Equal,
            NotEqual,
        }

        /// <summary>
        /// The operation the user has chosen.
        /// </summary>
        protected NumberFilterOperation ChosenOperation;

        /// <summary>
        /// The value the user has chosen.
        /// </summary>
        protected int ChosenValue;

        /// <summary>
        /// The textbox the user has typed into.
        /// </summary>
        private ListPicker _filterListPicker;

        /// <summary>
        /// Gets a textbox to type in.
        /// </summary>
        public override FrameworkElement GetFilterValueXamlElement()
        {
            return _filterListPicker ?? (_filterListPicker = GenerateListPicker());
        }

        /// <summary>
        /// Setup and create the special textbox
        /// </summary>
        private ListPicker GenerateListPicker()
        {
            List<string> values = EnumUtilities.GetEnumNames<EnumType>();
            //TODO format this for numbers only?
            ListPicker listPicker = new ListPicker();
            listPicker.ItemsSource = values;
            listPicker.ExpansionMode = ExpansionMode.FullScreenOnly;
            listPicker.SelectionChanged += OnSelectionChanged;

            return listPicker;
        }

        protected void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            // TODO: SafeGuard this.
            ListPicker valueListPicker = (ListPicker) sender;

            if (valueListPicker.SelectedItem == null)
                return;

            // Gross, but i dont want to learn how to properly handle this.
            // We know all these enums are ints anyways
            ChosenValue = (int)((object)EnumUtilities.GetEnumValueFromEnumName<EnumType>((string)valueListPicker.SelectedItem));
        }

        /// <summary>
        /// Sets the operation index
        /// </summary>
        public override void SetOperationIndex(int selectedIndex)
        {
            ChosenOperation = (NumberFilterOperation) selectedIndex;
        }

        /// <summary>
        /// Simply call this with your number.
        /// </summary>
        /// <returns></returns>
        protected bool CheckEnum(int i)
        {
            switch(ChosenOperation)
            {
                case NumberFilterOperation.Equal:
                    return i == ChosenValue;
                case NumberFilterOperation.NotEqual:
                    return i != ChosenValue;

                default:
                    // Didn't set a valud operation?
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Unregister any events.
        /// </summary>
        public override void Dispose()
        {
            _filterListPicker.SelectionChanged -= OnSelectionChanged;
        }
    }
}
