using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
[RequireComponent(typeof(Image))]
public class DummyCard : MonoBehaviour
{
    public LayoutElement LayoutElement 
    {
        get
        {
            if (layoutElementComponent == null)
                layoutElementComponent = GetComponent<LayoutElement>();
            return layoutElementComponent;
        }
    }
    public Image Image 
    {
        get
        {
            if (imageComponent == null)
                imageComponent = GetComponent<Image>();
            return imageComponent;
        }
    }
    
    private LayoutElement layoutElementComponent;
    private Image imageComponent;
}
