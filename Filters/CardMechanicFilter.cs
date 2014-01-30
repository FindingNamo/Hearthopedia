using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hearthopedia.Filters
{
    public class CardMechanicFilter : ICardFilter
    {
        public List<MechanicOption> FilterOptions { get; set; }


        private CardMechanicFilter()
        {
            FilterOptions = AssembleMechanicFilterOptions();
        }

        private List<MechanicOption> AssembleMechanicFilterOptions()
        {
            List<MechanicOption> returnList = new List<MechanicOption>();
            foreach(Mechanic m in DataManager.Instance.Mechanics)
            {
                returnList.Add(new MechanicOption(m.name, m.Id, true/*defaultValue*/));
            }
            return returnList;
        }

        private static CardMechanicFilter _instance;
        public static CardMechanicFilter Instance
        {
            get
            {
                return _instance ?? (_instance = new CardMechanicFilter());
            }
        }

        public bool Check(Card card)
        {
            foreach(Mechanic mechanic in card.MechanicData)
            {
                if (CheckMechanic(mechanic.Id))
                    return true;
            }
                
            return false;
        }

        private bool CheckMechanic(int mechanicId)
        {
            foreach (MechanicOption option in FilterOptions)
            {
                // This is really bad... but hey, they're all ints... right?
                if (option.Value && option.MechanicId == mechanicId)
                    return true;
            }
            return false;
        }


        public void SetUncheckedAll()
        {
            foreach (MechanicOption option in FilterOptions)
            {
                option.Value = false;
            };
        }

        public void SetCheckedAll()
        {
            foreach (MechanicOption option in FilterOptions)
            {
                option.Value = true;
            };
        }
    }
}
