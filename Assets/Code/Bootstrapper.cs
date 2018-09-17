using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kamikaze.Backend;

namespace Kamikaze
{
    public class Bootstrapper : MonoBehaviour
    {
        public List<Backend.Card> cardsP1;
        public List<Backend.Card> cardsP2;        
	
        public void Begin ()
        {
            Game.Initialize(cardsP1, cardsP2);
        }
    }
}