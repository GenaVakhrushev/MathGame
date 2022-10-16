using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerBox : Box, IClickable
{
    private Collider colliderComponent;

    protected override void Start()
    {
        base.Start();

        GameManager.OnMoveToNextEquasion.AddListener(ResetColor);

        colliderComponent = GetComponentInChildren<Collider>();
    }

    public void OnClick()
    {
        GameManager.OnAnswerSelected(this);
    }

    public void MarkCorrect()
    {
        PlayerInput.Instance.enabled = false;

        SetColor(Color.green, Color.green, 5f);
    }

    public void MarkWrong()
    {
        SetColor(Color.red, Color.red, 5f);
    }

    public void ResetColor()
    {
        SetColor(defaultColor, emissionDefaultColor, 1);        
    }
}
