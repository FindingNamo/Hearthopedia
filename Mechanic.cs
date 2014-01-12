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
                return _mechanicName ?? (_mechanicName = GetMechanicName(Id));
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
                return _mechanicDescription ?? (_mechanicDescription = GetMechanicDescription(Id));
            }
        }

        public Mechanic(int mechanicId)
        {
            Id = mechanicId;
        }

        public string GetMechanicDescription(int mechanicId)
        {
            switch (mechanicId)
            {
                case 0:
                    return "";
                case 71: // Affected by spell damage.
                    return "Damage increased by spell damage.";
                case 1: // Battlecry
                    return "Does something when you play it from your hand.";
                case 52: // Buff Stats
                    return "Increases the health and/or attack of a character or weapon.";
                case 57: // Can't Attack
                    return "";
                case 61: // Change Costs
                    return "Changes the amount of mana crystals required to play certain cards.";
                case 62: // Change Stats
                    return "Sets characters' attack or health to a specific value.";
                case 2: // Charge
                    return "Can attack immediately.";
                case 53: // Choose One
                    return "Select one effect from the list of options.";
                case 3: // Combo
                    return "A bonus if you already played a card this turn.";
                case 56: // Copy
                    return "Creates a duplicate of another card or minion.";
                case 18: // Counter
                    return "A card that is Countered has no effect.";
                case 65: // Damage All
                    return "Inflicts some amount of damage to your characters as well as your enemy's characters.";
                case 66: // Damage Enemies
                    return "Inflicts some amount of damage to your enemy's characters.";
                case 64: // Deal Damage
                    return "Inflicts some amount of damage to character(s).";
                case 4: // Deathrattle
                    return "Does something when it dies.";
                case 54: //Destroy
                    return "Removes something from the battlefield, regardless of its remaining health or effects.";
                case 51: //Discard
                    return "Removes cards before they can be played.";
                case 5: //Divine Shield
                    return "The first time this minion takes damage, ignore it.";
                case 50: //Draw Cards
                    return "Draws additional card(s) from the deck.";
                case 6: //Enrage
                    return "While damaged, this minion has a new power.";
                case 7: //Freeze
                    return "Frozen characters lose their next attack.";
                case 68: //Gain Armor
                    return "Increases the armor value of a hero.";
                case 8: //Grant Charge
                    return "";
                case 19: //Immune
                    return "Can't be damaged.";
                case 70: //Immune To Spell Damage
                    return "Not affected by increased spell damage.";
                case 63: //Mana Crystals
                    return "Changes the amount of available mana crystals for a player.";
                case 9: //Overload: X
                    return "You have X less mana next turn.";
                case 69: //Poisonous
                    return "Destroy any minion damaged by this minion.";
                case 60: //Restore Health
                    return "Increases a character's remaining health, but not beyond its maximum health.";
                case 67: //Return to Hand
                    return "Removes a card from play and puts it back into the hand of cards not yet played.";
                case 11: //Secret
                    return "Hidden until a specific action occurs.";
                case 12: //Silence
                    return "Removes all card text and enchantments.";
                case 10: //Spell Damage
                    return "Your spell cards deal # extra damage.";
                case 13: // Stealth
                    return "Can't be attacked or targeted until it deals damage.";
                case 55: //Summon
                    return "Places another minion on the battlefield.";
                case 58: //Take Control
                    return "Forces a minion to be controlled by its enemy.";
                case 15: //Taunt
                    return "Enemies must attack this minion.";
                case 59: //Transform
                    return "Causes one minion to become another minion.";
                case 17: //Windfury
                    return "Can attack twice each turn.";
                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// Get a string name of a mechanic id
        /// </summary>
        private string GetMechanicName(int mechanic)
        {
            switch (mechanic)
            {
                case 0:
                    return "None";
                case 71:
                    return "Affected By Spell Damage";
                case 1:
                    return "Battlecry";
                case 52:
                    return "Buff Stats";
                case 57:
                    return "Can't Attack";
                case 61:
                    return "Change Cost";
                case 62:
                    return "Change Stats";
                case 2:
                    return "Charge";
                case 53:
                    return "Choose One";
                case 3:
                    return "Combo";
                case 56:
                    return "Copy";
                case 18:
                    return "Counter";
                case 65:
                    return "Damage All";
                case 66:
                    return "Damage Enemies";
                case 64:
                    return "Deal Damage";
                case 4:
                    return "Deathrattle";
                case 54:
                    return "Destroy";
                case 51:
                    return "Discard";
                case 5:
                    return "Divine Shield";
                case 50:
                    return "Draw Cards";
                case 6:
                    return "Enrage";
                case 7:
                    return "Freeze";
                case 68:
                    return "Gain Armor";
                case 8:
                    return "Grant Charge";
                case 19:
                    return "Immune";
                case 70:
                    return "Immune To Spell Damage";
                case 63:
                    return "Mana Crystals";
                case 9:
                    return "Overload: X";
                case 69:
                    return "Poisonous";
                case 60:
                    return "Restore Health";
                case 67:
                    return "Return to Hand";
                case 11:
                    return "Secret";
                case 12:
                    return "Silence";
                case 10:
                    return "Spell Damage";
                case 13:
                    return "Stealth";
                case 55:
                    return "Summon";
                case 58:
                    return "Take Control";
                case 15:
                    return "Taunt";
                case 59:
                    return "Transform";
                case 17:
                    return "Windfury";
                default:
                    return "Unknown";
            }
        }
    }
}
