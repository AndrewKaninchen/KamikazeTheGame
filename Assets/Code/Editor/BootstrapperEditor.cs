using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Kamikaze
{
    [CustomEditor(typeof(Bootstrapper))]
    public class BootstrapperEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var b = target as Bootstrapper;
            base.OnInspectorGUI();
            if (b == null || b.gameController == null || !b.gameController.IsGameStarted)
            {
                if (GUILayout.Button("Begin"))
                {
                    if (b != null) b.Begin();
                }
            }
        }
    }
}