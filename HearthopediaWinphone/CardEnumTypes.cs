using System;
using System.ComponentModel;
using System.Runtime.Serialization;

public class FriendlyNameAttribute : Attribute
{
    public string FriendlyName;

    public FriendlyNameAttribute(string name)
    {
        this.FriendlyName = name;
    }
}

#if NETFX_CORE
public class DescriptionAttribute : Attribute
{
    public string Description;

    public DescriptionAttribute(string description)
    {
        this.Description = description;
    }
}
#else
#endif

public enum CardTypes
{
    Hero = 3,
    Minion = 4,
    Spell = 5,
    Weapon = 7,

    [FriendlyNameAttribute("Hero Power")]
    HeroPower = 10,
}

public enum CardSet
{
    Basic = 2,
    Expert = 3,
    Reward = 4,
    Missions = 5,
    Promotions = 11,
}

public enum CardRace
{
    None = 0,
    Beast = 20,
    Demon = 15,
    Dragon = 24,
    Murloc = 14,
    Pirate = 23,
    Totem = 21,
}

public enum CardQuality
{
    Free = 0,
    Common = 1,
    Magic = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5,
}

public enum CardClass
{
    Everyone = 0,
    Warrior = 1,
    Paladin = 2,
    Hunter = 3,
    Rogue = 4,
    Priest = 5,
    //??? - 6
    Shaman = 7,
    Mage = 8,
    Warlock = 9,
    //??? - 10
    Druid = 11,
}

// These are now parsed from cards.txt
////[FriendlyName("")]
////[Description("")]
//public enum CardMechanic
//{
//    [FriendlyName("None")]
//    [Description("None")]
//    None = 0,

//    [FriendlyName("Affected by spell damage")]
//    [Description("Damage increased by spell damage.")]
//    AffectedBySpellDamage = 71,

//    [FriendlyName("Battle Cry")]
//    [Description("Does something when you play it from your hand.")]
//    BattleCry = 1,

//    [FriendlyName("Buff Stats")]
//    [Description("Increases the health and/or attack of a character or weapon.")]
//    BuffStats = 52,

//    [FriendlyName("Can't Attack")]
//    CantAttack = 57,

//    [FriendlyName("Change Costs")]
//    [Description("Changes the amount of mana crystals required to play certain cards.")]
//    ChangeCosts = 61,

//    [FriendlyName("Change Stats")]
//    [Description("Sets a character's attack or health to a specific value.")]
//    ChangeStats = 62,

//    [Description("Can attack immediately.")]
//    Charge = 2,

//    [FriendlyName("Choose One")]
//    [Description("Select one effect from the list of options.")]
//    ChooseOne = 53,

//    [Description("A bonus if you already played a card this turn.")]
//    Combo = 3,

//    [Description("Creates a duplicate of another card or minion.")]
//    Copy = 56,

//    [Description("A card that is countered has no effect.")]
//    Counter = 18,

//    [FriendlyName("Damage All")]
//    [Description("Inflicts some amount of damage to your characters as well as your enemy's characters.")]
//    DamageAll = 65,

//    [FriendlyName("Damage Enemies")]
//    [Description("Inflicts some amount of damage to your enemy's characters.")]
//    DamageEnemies = 66,

//    [FriendlyName("Deal Damage")]
//    [Description("Inflicts some amount of damage to character(s).")]
//    DealDamage = 64,

//    [FriendlyName("Death Rattle")]
//    [Description("Does something when it dies.")]
//    DeathRattle = 4,

//    [Description("Removes something from the battlefield, regardless of its remaining health or effects.")]
//    Destroy = 54,

//    [Description("Removes cards before they can be played.")]
//    Discard = 51,

//    [FriendlyName("Divine Shield")]
//    [Description("The first time this minion takes damage, ignore it.")]
//    DivineShield = 5,

//    [FriendlyName("Draw Cards")]
//    [Description("Draws additional card(s) from the deck.")]
//    DrawCards = 50,

//    [Description("While damaged, this minion has a new power.")]
//    Enrage = 6,

//    [Description("Frozen characters lose their next attack.")]
//    Freeze = 7,

//    [FriendlyName("Gain Armor")]
//    [Description("Increases the armor value of a hero.")]
//    GainArmor = 68,

//    [FriendlyName("Grant Charge")]
//    GrantCharge = 8,

//    [Description("Can't be damaged.")]
//    Immune = 19,

//    [FriendlyName("Immune to Spell Damage")]
//    [Description("Not affected by increased spell damage.")]
//    ImmuneToSpellDamage = 70,

//    [FriendlyName("Mana Crystals")]
//    [Description("Changes the amount of available mana crystals for a player.")]
//    ManaCrystals = 63,

//    [FriendlyName("Overload X")]
//    [Description("You have X less mana next turn.")]
//    OverloadX = 9,

//    [Description("Destroy any minion damaged by this minion.")]
//    Poisonous = 69,

//    [FriendlyName("Restore Health")]
//    [Description("Increases a character's remaining health, but not beyond its maximum health.")]
//    RestoreHealth = 60,
    
//    [FriendlyName("Return to Hand")]
//    [Description("Removes a card from play and puts it back into the hand of cards not yet played.")]
//    ReturnToHand = 67,

//    [Description("Hidden until a specific action occurs.")]
//    Secret = 11,

//    [Description("Removes all card text and enchantments.")]
//    Silence = 12,

//    [FriendlyName("Spell Damage")]
//    [Description("Your spell cards deal # extra damage.")]
//    SpellDamage = 10,

//    [Description("Can't be attacked or targeted until it deals damage.")]
//    Stealth = 13,

//    [Description("Places another minion on the battlefield.")]
//    Summon = 55,

//    [FriendlyName("Take Control")]
//    [Description("Forces a minion to be controlled by its enemy.")]
//    TakeControl = 58,

//    [Description("Enemies must attack this minion.")]
//    Taunt = 15,

//    [Description("Causes one minion to become another minion.")]
//    Transform = 59,

//    [Description("Windfury")]
//    Windfury = 17,
//}