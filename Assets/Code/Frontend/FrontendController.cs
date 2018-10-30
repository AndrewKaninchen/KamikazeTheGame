using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading.Tasks;
using Kamikaze.Backend;
using TMPro;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Kamikaze.Frontend
{
    [Serializable]
    public class PlayerObjects
    {
        public CameraController cam;
        public HandPanel hand;
        public List<Token> tokens;
        public Deck deck;
    }

    public partial class FrontendController : MonoBehaviour
    {
        public Dictionary<Backend.Player, PlayerObjects> playerObjects = new Dictionary<Player, PlayerObjects>();
        
        #region Backend Objects
        private Backend.GameController gameController;
        #endregion

        #region Components & Objects
        public PlayerObjects p1Objects;
        public PlayerObjects p2Objects;
        public PlayerObjects currentPlayerObjects;
        public PlayerObjects otherPlayerObjects;
        public Animator suaVez;
        public GameObject endTurnButton;
        public Canvas screenSpaceCanvas;
        public TextMeshProUGUI centeredText;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private GameObject dummyPrefab;
        [SerializeField] private GameObject tokenPrefab;

        #endregion

        #region Events
        public event Action OnTurnEnded;
        #endregion
        
        public void Initialize(Player p1, Player p2)
        {
            this.gameController = gameController;
            playerObjects.Add(p1, p1Objects);
            playerObjects.Add(p2, p2Objects);
        }

        public void Start()
        {
            p1Objects.hand.frontendController = this;
            p2Objects.hand.frontendController = this;
            
            currentPlayerObjects = p1Objects;
            otherPlayerObjects = p2Objects;

            p1Objects.hand.playerObjects = p1Objects;
            p2Objects.hand.playerObjects = p2Objects;
        }

        public void DisplayEndTurnButton()
        {
            endTurnButton.SetActive(true);
        }

        public void HideEndTurnButton()
        {
            endTurnButton.SetActive(false);
        }

        public void EndTurn()
        {
            OnTurnEnded?.Invoke();
        }

        public IEnumerator ChangePlayer()
        {
            HideEndTurnButton();

            otherPlayerObjects = currentPlayerObjects;
            currentPlayerObjects = currentPlayerObjects == p1Objects ? p2Objects : p1Objects;

            suaVez.SetTrigger("VAI");
            yield return new WaitForSeconds(1.5f);

            otherPlayerObjects.cam.gameObject.SetActive(false);
            currentPlayerObjects.cam.gameObject.SetActive(true);
            yield return new WaitForSeconds(.3f);

            foreach (var t in currentPlayerObjects.tokens)
                t.Color = Token.TokenColor.Friendly;

            foreach (var t in otherPlayerObjects.tokens)
                t.Color = Token.TokenColor.Enemy;

            suaVez.SetTrigger("DESCE");
            yield return new WaitForSeconds(.7f);
        }
//
//        public Token CreateToken(Backend.FieldCard card)
//        {
//            var i = Instantiate(tokenPrefab).GetComponent<Token>();
//            i.Initialize(card);
//            i.gameObject.SetActive(false);
//            return i;
//        }
        
        public Card CreateCard(Backend.Card backend)
        {
            var i = Instantiate(cardPrefab, playerObjects[backend.owner].deck.transform).GetComponent<Card>();
            i.Initialize(playerObjects[backend.owner], this, backend, Instantiate(dummyPrefab).GetComponent<DummyCard>());
            return i;
        }

        public async Task DisplayText(string text)
        {
            centeredText.text = text;
            centeredText.gameObject.SetActive(true);
            await Task.Delay(1000);
            centeredText.gameObject.SetActive(false);
        }
    }

    
}