using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Kamikaze.Backend;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

[CustomEditor(typeof(CardAsset))]
public class CardAssetEditor : Editor
{
    private CardAsset cardAsset;
    
    public override void OnInspectorGUI()
    {
        cardAsset = target as CardAsset;
        base.OnInspectorGUI();

        if (cardAsset.scriptAsset == null)
        {
            if (GUILayout.Button("Create Script"))
            {
                CreateScript();
            }
        }
        else
        {
            if (GUILayout.Button("Delete Script"))
            {
                DestroyImmediate(cardAsset.scriptAsset, true);
                cardAsset.scriptAsset = null;
                var cardAssetPath = AssetDatabase.GetAssetPath(target);
                AssetDatabase.ImportAsset(cardAssetPath);
                //AssetDatabase.DeleteAsset(scriptAssetPath);
            }
        }
    }

    private void CreateScript()
    {
        //var script = Instantiate(((CardAsset) target).scriptTemplate);
        cardAsset.scriptAsset = new TextAsset {name = "Script.cs"};
        AssetDatabase.AddObjectToAsset(cardAsset.scriptAsset, target);
        cardAsset.scriptAssetPath = AssetDatabase.GetAssetPath(cardAsset.scriptAsset);
        var cardAssetPath = AssetDatabase.GetAssetPath(target);
        AssetDatabase.ImportAsset(cardAssetPath);
    }
}
