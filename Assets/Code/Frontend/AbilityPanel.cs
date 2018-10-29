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
        private static  GameObject prefab;
        private static AbilityPanel instance;

        //TODO: essa desgraça de Singleton
        private static AbilityPanel Instance => instance != null ? instance : Instantiate(prefab).GetComponent<AbilityPanel>();

        
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
            Debug.Log(card);
            foreach (var ability in card.Abilites)
            {
                var i = Instantiate(buttonPrefab, buttonRoot).GetComponent<Button>();
                i.GetComponentInChildren<TextMeshProUGUI>().text = ability.Name;
                abilityButtons.Add(ability, i);
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
                abilityButton.Value.onClick.AddListener (
                () =>
                {
                    gameObject.SetActive(false);
                    ResolveSelection(abilityButton.Key);
                });
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
