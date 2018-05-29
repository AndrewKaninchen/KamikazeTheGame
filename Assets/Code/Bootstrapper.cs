using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kamikaze.Backend;

public class Bootstrapper : MonoBehaviour
{
    public List<Card> cardsP1;
    public List<Card> cardsP2;        
	
    public void Begin ()
    {
        Game.Initialize(cardsP1, cardsP2);
    }
}