using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    [HideInInspector]
    public Vector3 textOffset;

    protected TMP_Text text;
    public TMP_Text Text => text;
    protected Renderer renderer;

    public Material material => renderer.material;

    protected Color defaultColor;
    protected Color emissionDefaultColor;

    public Color DefaultColor => defaultColor;
    public Color EmissionDefaultColor => emissionDefaultColor;

    public string Value => text.text;

    protected void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        renderer = GetComponentInChildren<Renderer>();

        textOffset = text.transform.localPosition;

        defaultColor = material.GetColor("_BaseColor");
        emissionDefaultColor = material.GetColor("_EmissionColor");
    }

    public void SetColor(Color baseColor, Color emmisionColor, float emissionLevel)
    {
        material.SetColor("_BaseColor", baseColor);
        material.SetColor("_EmissionColor", emmisionColor * emissionLevel);
    }
}
