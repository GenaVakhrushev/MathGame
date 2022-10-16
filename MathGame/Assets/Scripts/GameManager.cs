using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    private static Equation[] equations;
    private static int currentEquationNum = 0;
    public Equation CurrentEquation => equations[currentEquationNum];

    [SerializeField] private Animator boxesAnimator;

    [SerializeField] private int maxEquationsCount = 1;

    public static UnityEvent OnMoveToNextEquasion = new UnityEvent();

    void Start()
    {
        currentEquationNum = 0;

        equations = EquationsManager.GetRandomEquations(maxEquationsCount);

        EquationDisplay.DisplayEquationParams(equations[currentEquationNum]);
    }

    public static void OnAnswerSelected(AnswerBox answerBox)
    {
        if (answerBox.Value == equations[currentEquationNum].X)
        {
            answerBox.MarkCorrect();

            answerBox.StartCoroutine(BoxesAnimations.Instance.MoveRightAnswerText(answerBox.Text, 1f, 0.5f));

            EquationDisplay.HideX(equations[currentEquationNum].X);

            currentEquationNum++;
        }
        else
        {
            answerBox.MarkWrong();
        }
    }

    public static void MoveToNextEquasion()
    {
        if (currentEquationNum >= Instance.maxEquationsCount)
        {
            Instance.boxesAnimator.SetBool("Win", true);
        }
        else
        {
            Instance.boxesAnimator.SetTrigger("NextEquation");
        }

        PlayerInput.Instance.enabled = true;

        OnMoveToNextEquasion.Invoke();
    }
}
