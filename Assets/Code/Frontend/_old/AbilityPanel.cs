using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Kamikaze.Backend;


namespace Kamikaze.Frontend_Old
{
    public class AbilityPanel : MonoBehaviour
    {
        public GameObject abilityButtonPrefab;
        private Dictionary<Backend.Ability, Button> abilityButtons = new Dictionary<Backend.Ability, Button>();
        private Card currentInspectedCard;

        private void Start()
        {
            Hide();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                Hide();
            }
        }

        public void Show(Card card)
        {
            if (card != currentInspectedCard)
               UpdateAbilities(card);
            UpdateAbilitiesAvailability();
            gameObject.SetActive(true);
            transform.position = card.transform.position + Vector3.down * 75f;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void UpdateAbilities(Card card)
        {
            currentInspectedCard = card;
            foreach (Button b in abilityButtons.Values)            
                Destroy(b.gameObject);
            abilityButtons.Clear();

            foreach (Backend.Ability a in currentInspectedCard.BackendCard.Abilites)
            {
                var b = CreateAbilityButton();
                b.onClick.AddListener(() => a.Body());
                abilityButtons.Add(a, b);
            }
        }

        private void UpdateAbilitiesAvailability()
        {
            foreach (Backend.Ability a in currentInspectedCard.BackendCard.Abilites)
                abilityButtons[a].interactable = a.Condition();
        }

        private Button CreateAbilityButton()
        {
            return Instantiate(abilityButtonPrefab, transform).GetComponent<Button>();
        }
    }
}