using System.Collections;
using System.Collections.Generic;
using Kamikaze.Backend;
using UnityEngine;

namespace Kamikaze.Frontend
{	
	public class HandPanel : MonoBehaviour
	{
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
			card.dummy = dummy;
			dummy.gameObject.SetActive(true);
			card.gameObject.SetActive(true);
			cards.Add(card);
            card.owner = player;
		}
	}

}
