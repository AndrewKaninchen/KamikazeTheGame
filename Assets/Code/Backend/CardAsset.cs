using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Kamikaze.Backend
{
    [CreateAssetMenu(menuName = "Card", fileName = "New Card")]
    public class CardAsset : ScriptableObject
    {
        #if UNITY_EDITOR
        [SerializeField] public TextAsset scriptTemplate;
        #endif
        
        public Texture2D image;
        public string description;
        public Type associatedType = typeof(Kamikaze.Chris);
    }
}