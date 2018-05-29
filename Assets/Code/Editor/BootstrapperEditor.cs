using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bootstrapper))]
public class BootstrapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Bootstrapper b = target as Bootstrapper;
        base.OnInspectorGUI();

        if (GUILayout.Button("Begin"))
        {
            b.Begin();
        }
    }
}
