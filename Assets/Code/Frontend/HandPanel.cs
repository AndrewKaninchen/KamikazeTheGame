using System.Collections.Generic;
using Kamikaze.Backend;
using UnityEngine;
using UnityEngine.Serialization;

namespace Kamikaze.Frontend
{	
	public class HandPanel : MonoBehaviour
	{
		public FrontendController frontendController;
		public CardAsset templateCard; //TODO: tirar isso e criar uma inicialização de verdade pras cartas
		
        [FormerlySerializedAs("player")] public PlayerObjects playerObjects;

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

		public void AddCard(Card card)
		{
			card.dummy.transform.SetParent(dummyRoot);
			cards.Add(card);
		}
		
		public void DrawCard()
		{
			var dummy = Instantiate(dummyPrefab, dummyRoot).GetComponent<DummyCard>();
			var card = Instantiate(cardPrefab, drawOrigin.position, drawOrigin.rotation, cardRoot).GetComponent<Card>();
			card.Initialize(playerObjects, frontendController, Backend.Card.CreateCard(templateCard.associatedType, null, null, null, card.GetComponent<Card>(), null, null), dummy);
			
			//TODO: resolver essa lambança fazendo uma fábrica melhor que gera as duas cartas simultaneamente. Talvez valha a pena retornar uma 3-upla com BackendCard, FrontendCard, FrontendToken
			card.BackendCard.FrontendCard = card;
			card.BackendCard.FrontendToken = card.Token;
			
			
			cards.Add(card);
		}
	}

}
