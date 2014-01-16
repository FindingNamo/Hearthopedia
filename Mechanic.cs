using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia
{
    public class Mechanic
    {
        public static readonly int MechanicIdNone = 0;

        /// <summary>
        /// The mechanic id
        /// </summary>
        public int Id;

        // For some reason, there was difficulty getting it to show up without a get pattern like this.
        private string _mechanicName;

        /// <summary>
        /// The name of this mechanic, ie: Charge, Combo
        /// </summary>
        public string MechanicName
        {
            get
            {
                return _mechanicName;
            }
        }

        private string _mechanicDescription;
        /// <summary>
        /// The description of this mechanic, ie: "Attacks immediately"
        /// </summary>
        public string MechanicDescription
        {
            get
            {
                return _mechanicDescription;
            }
        }

        public Mechanic(int mechanicId)
        {
            Id = mechanicId;
            _mechanicDescription = EnumUtilities.GetDescription<CardMechanic>((CardMechanic)mechanicId);
            _mechanicName = EnumUtilities.GetFriendlyName<CardMechanic>((CardMechanic)mechanicId);
        }
    }
}
