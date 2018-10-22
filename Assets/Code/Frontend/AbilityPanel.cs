using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kamikaze.Backend;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace Kamikaze.Frontend
{
    public class AbilityPanel : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        
        [Space, SerializeField] private Transform buttonRoot;
        private Dictionary<Ability, Button> abilityButtons = new Dictionary<Ability, Button>();
        private TaskCompletionSource<Ability> completionSource;
        
        public Task<Ability> Display(FieldCard card)
        {
            completionSource = new TaskCompletionSource<Ability>();
            
            ClearButtons();
            CreateButtons(card);
            SubscribeButtons();

            return completionSource.Task;
        }

        private void CreateButtons(FieldCard card)
        {
            foreach (var ability in card.Abilites)
            {
                var instance = Instantiate(buttonPrefab, buttonRoot).GetComponent<Button>();
                instance.GetComponentInChildren<TextMeshProUGUI>().text = ability.Name;
                abilityButtons.Add(ability, instance);
            }
        }
        
        private void ClearButtons()
        {
            foreach (var abilityButton in abilityButtons)
                Destroy(abilityButton.Value); // TODO: Eventualmente substituir isso por uma pool
            abilityButtons.Clear();
        }

        private void SubscribeButtons()
        {
            foreach (var abilityButton in abilityButtons)
            {
                abilityButton.Value.interactable = abilityButton.Key.Condition();
                abilityButton.Value.onClick.AddListener(() => ResolveSelection(abilityButton.Key));
            }
        }

        private void ResolveCancelation()
        {
            completionSource.SetCanceled();
        }
        
        private void ResolveSelection(Ability selected)
        {
            completionSource.SetResult(selected);
        }
    }
}
