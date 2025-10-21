using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace pokerpg
{
    [CreateAssetMenu(fileName = "New Card")]
    public class Card : ScriptableObject
    {
        
        public string cardName;

        public List<CardType> cardType;
        public int health;
        public int damage;
        public List<Buffs> buff;
        public int defense;
        public int gold;
        public enum CardType
        {
            Spades,
            Hearts,
            Clubs,
            Diamonds
        }
        public enum Buffs
        {
            dmg,
            hp,
            defense,
            gold
        }

    }
}