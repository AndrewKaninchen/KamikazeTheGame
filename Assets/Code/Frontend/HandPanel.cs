using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Kamikaze.Frontend
{
    public class HandPanel : MonoBehaviour
    {
        public HorizontalLayoutGroup layoutGroup;
        public List<Card> cards;

        public Card currentHoveredCard;
        public Card currentDraggedCard;

        private bool _interactable;
        public bool Interactable { get => _interactable; set { _interactable = value; foreach (Card c in cards) c.interactable = value; } }

        private void Start()
        {
            foreach (Card c in GetComponentsInChildren<Card>())
                AddCard(c);
        }

        public void AddCard (Card card)
        {
            card.handPanel = this;
            cards.Add(card);
            card.transform.SetParent(transform);
        }

        public void RemoveCard(Card card)
        {
            card.handPanel = null;
            cards.Remove(card);
        }
                
        public void SetInteractableCards (IEnumerable<Card> cards)
        {
            foreach (var c in this.cards)
                c.interactable = cards.Contains(c);
        }

        public void SetHighlightedCards (IEnumerable<Card> cards)
        {
            foreach (var c in this.cards)
                c.isHighlighted = cards.Contains(c);
        }
    }
}