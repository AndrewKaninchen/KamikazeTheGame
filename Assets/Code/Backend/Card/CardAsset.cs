using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TypeReferences;

namespace Kamikaze.Backend
{
    [CreateAssetMenu(menuName = "Card", fileName = "New Card")]
    public class CardAsset : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] public string scriptAssetPath;
        [SerializeField] public TextAsset scriptAsset;
#endif
        
        public Texture2D image;
        public Texture2D spriteImage;
        public string cardName;
        public string cardDescription;
        
        
        [ClassExtends(typeof(Card))]
        public ClassTypeReference associatedType;
        
        public Dictionary<ClassTypeReference, string> abilityDescriptions;
    }
}