using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerBox : Box, IClickable
{
    private void Start()
    {
        base.Start();

        GameManager.OnMoveToNextEquasion.AddListener(ResetColor);
    }

    public void OnClick()
    {
        GameManager.OnAnswerSelected(this);
    }

    public void MarkCorrect()
    {
        SetColor(Color.green, Color.green, 5f);

        StartCoroutine(BoxesAnimations.Instance.MoveRightAnswerText(Text, 0.5f, 0.5f));
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
