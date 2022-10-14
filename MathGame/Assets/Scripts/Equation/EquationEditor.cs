using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Equation)), CanEditMultipleObjects]
public class EquationEditor : Editor
{
    private Equation equation;

    private int correctAnswerIndex = 0;

    private void OnEnable()
    {
        equation = (Equation)target;

        equation.UpdateNumbersList();
        
        correctAnswerIndex = equation.XOrderNumber;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed)
        {
            equation.UpdateNumbersList();
        }

        correctAnswerIndex = EditorGUILayout.Popup("X", correctAnswerIndex, equation.Numbers.ToArray());
        if (equation.Numbers.Count > 0)
        {
            equation.SetX(equation.Numbers[correctAnswerIndex], correctAnswerIndex);
        }
    }
}