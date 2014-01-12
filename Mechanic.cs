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
            ConfigureMechanic(Id);
        }

        public void ConfigureMechanic(int mechanicId)
        {
            switch (mechanicId)
            {
                case 0:
                    _mechanicName = "None";
                    _mechanicDescription = "";
                    break;
                case 71:
                    _mechanicName = "Affected by spell damage";
                    _mechanicDescription = "Damage increased by spell damage.";
                    break;
                case 1:
                    _mechanicName = "Battlecry";
                    _mechanicDescription = "Does something when you play it from your hand.";
                    break;
                case 52:
                    _mechanicName = "Buff Stats";
                    _mechanicDescription = "Increases the health and/or attack of a character or weapon.";
                    break;
                case 57:
                    _mechanicName = "Can't Attack";
                    _mechanicDescription = "";
                    break;
                case 61:
                    _mechanicName = "Change Costs";
                    _mechanicDescription = "Changes the amount of mana crystals required to play certain cards.";
                    break;
                case 62:
                    _mechanicName = "Change Stats";
                    _mechanicDescription = "Sets characters' attack or health to a specific value.";
                    break;
                case 2:
                    _mechanicName = "Charge";
                    _mechanicDescription = "Can attack immediately.";
                    break;
                case 53:
                    _mechanicName = "Choose One";
                    _mechanicDescription = "Select one effect from the list of options.";
                    break;
                case 3:
                    _mechanicName = "Combo";
                    _mechanicDescription = "A bonus if you already played a card this turn.";
                    break;
                case 56: 
                    _mechanicName = "Copy";
                    _mechanicDescription = "Creates a duplicate of another card or minion.";
                    break;
                case 18:
                    _mechanicName = "Counter";
                    _mechanicDescription = "A card that is Countered has no effect.";
                    break;
                case 65:
                    _mechanicName = "Damage All";
                    _mechanicDescription = "Inflicts some amount of damage to your characters as well as your enemy's characters.";
                    break;
                case 66:
                    _mechanicName = "Damage Enemies";
                    _mechanicDescription = "Inflicts some amount of damage to your enemy's characters.";
                    break;
                case 64:
                    _mechanicName = "Deal Damage";
                    _mechanicDescription = "Inflicts some amount of damage to character(s).";
                    break;
                case 4:
                    _mechanicName = "Deathrattle";
                    _mechanicDescription = "Does something when it dies.";
                    break;
                case 54:
                    _mechanicName = "Destroy";
                    _mechanicDescription = "Removes something from the battlefield, regardless of its remaining health or effects.";
                    break;
                case 51:
                    _mechanicName = "Discard";
                    _mechanicDescription = "Removes cards before they can be played.";
                    break;
                case 5:
                    _mechanicName = "Divine Shield";
                    _mechanicDescription = "The first time this minion takes damage, ignore it.";
                    break;
                case 50:
                    _mechanicName = "Draw Cards";
                    _mechanicDescription = "Draws additional card(s) from the deck.";
                    break;
                case 6:
                    _mechanicName = "Enrage";
                    _mechanicDescription = "While damaged, this minion has a new power.";
                    break;
                case 7:
                    _mechanicName = "Freeze";
                    _mechanicDescription = "Frozen characters lose their next attack.";
                    break;
                case 68:
                    _mechanicName = "Gain Armor";
                    _mechanicDescription = "Increases the armor value of a hero.";
                    break;
                case 8:
                    _mechanicName = "Grant Charge";
                    _mechanicDescription = "";
                    break;
                case 19:
                    _mechanicName = "Immune";
                    _mechanicDescription = "Can't be damaged.";
                    break;
                case 70:
                    _mechanicName = "Immune To Spell Damage";
                    _mechanicDescription = "Not affected by increased spell damage.";
                    break;
                case 63:
                    _mechanicName = "Mana Crystals";
                    _mechanicDescription = "Changes the amount of available mana crystals for a player.";
                    break;
                case 9: 
                    _mechanicName = "Overload: X";
                    _mechanicDescription = "You have X less mana next turn.";
                    break;
                case 69: 
                    _mechanicName = "Poisonous";
                    _mechanicDescription = "Destroy any minion damaged by this minion.";
                    break;
                case 60: 
                    _mechanicName = "Restore Health";
                    _mechanicDescription = "Increases a character's remaining health, but not beyond its maximum health.";
                    break;
                case 67: 
                    _mechanicName = "Return to Hand";
                    _mechanicDescription = "Removes a card from play and puts it back into the hand of cards not yet played.";
                    break;
                case 11:
                    _mechanicName = "Secret";
                    _mechanicDescription = "Hidden until a specific action occurs.";
                    break;
                case 12:
                    _mechanicName = "Silence";
                    _mechanicDescription = "Removes all card text and enchantments.";
                    break;
                case 10: 
                    _mechanicName = "Spell Damage";
                    _mechanicDescription = "Your spell cards deal # extra damage.";
                    break;
                case 13: 
                    _mechanicName = "Stealth";
                    _mechanicDescription = "Can't be attacked or targeted until it deals damage.";
                    break;
                case 55: 
                    _mechanicName = "Summon";
                    _mechanicDescription = "Places another minion on the battlefield.";
                    break;
                case 58: 
                    _mechanicName = "Take Control";
                    _mechanicDescription = "Forces a minion to be controlled by its enemy.";
                    break;
                case 15: 
                    _mechanicName = "Taunt";
                    _mechanicDescription = "Enemies must attack this minion.";
                    break;
                case 59: 
                    _mechanicName = "Transform";
                    _mechanicDescription = "Causes one minion to become another minion.";
                    break;
                case 17: 
                    _mechanicName = "Windfury";
                    _mechanicDescription = "Can attack twice each turn.";
                    break;
                default:
                    _mechanicName = "Unknown";
                    _mechanicDescription = "Unknown";
                    break;
            }
        }
    }
}
