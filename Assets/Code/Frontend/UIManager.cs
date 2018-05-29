﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kamikaze.Frontend
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance 
        {
            get { if (!_instance) _instance = FindObjectOfType<UIManager>(); return _instance; }
            set 
            {
                if (_instance != null)
                    Destroy(_instance.gameObject);
                _instance = value;
            }
        }

        public AbilityPanel abilityPanel;
    }
}