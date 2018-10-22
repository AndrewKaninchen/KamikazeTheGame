using System.Collections.Generic;
using Kamikaze.Backend;
using UnityEngine;

namespace Kamikaze.Frontend
{	
	public class HandPanel : MonoBehaviour
	{
		public FrontendController frontendController;
		public CardAsset templateCard; //TODO: tirar isso e criar uma inicialização de verdade pras cartas
		
        public Player player;

		public GameObject dummyPrefab;
		public GameObject cardPrefab;
		public List<Card> cards = new List<Card>();

		public Transform dummyRoot;
		public Transform cardRoot;
		public Transform drawOrigin;

		public uint startingCards;

		private void Start()
		{
//			for (var i = 0; i < startingCards; i++)
//			{
//				DrawCard();	
//			}
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Alpha1))
				DrawCard();
		}

		public void DrawCard()
		{
			var dummy = Instantiate(dummyPrefab, dummyRoot).GetComponent<DummyCard>();
			var card = Instantiate(cardPrefab, drawOrigin.position, drawOrigin.rotation, cardRoot).GetComponent<Card>();
			card.Initialize(player, frontendController, Backend.Card.CreateCard(templateCard.associatedType, null, null, null, null, null, null), dummy);
			cards.Add(card);
		}
	}

}
