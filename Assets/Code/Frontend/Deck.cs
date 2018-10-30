using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Kamikaze.Frontend
{
    public class Deck : MonoBehaviour
    {
        public float spacing;
        public List<Card> cards;
        
        public void AddCard(Card card)
        {
            card.transform.localPosition = Vector3.up * spacing * cards.Count;
            cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            cards.Remove(card);
        }

        public Card DrawCard()
        {
            var c = cards.Last();
            return c;
        }
    }
}