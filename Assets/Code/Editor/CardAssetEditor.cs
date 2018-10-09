using System.Collections;
using System.Collections.Generic;
using System.IO;
using Kamikaze.Backend;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

[CustomEditor(typeof(CardAsset))]
public class CardAssetEditor : Editor
{
    
    [SerializeField] private bool isScriptCreated; 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!isScriptCreated)
        {
            if (GUILayout.Button("Create Script"))
            {
                CreateScript();
            }
        }
    }

    private void CreateScript()
    {
        var script = Instantiate(((CardAsset) target).scriptTemplate);
        //var script = new TextAsset {name = "Script.cs"};
        AssetDatabase.AddObjectToAsset(script, target);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(target));
    }
}
