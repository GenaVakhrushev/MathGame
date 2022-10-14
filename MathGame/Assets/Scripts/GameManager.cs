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
            currentEquationNum++;

            if (currentEquationNum >= Instance.maxEquationsCount)
            {
                Instance.boxesAnimator.SetBool("Win", true);

                return;
            }

            answerBox.MarkCorrect();

            EquationDisplay.HideX();
        }
        else
        {
            answerBox.MarkWrong();
        }
    }

    public static void MoveToNextEquasion()
    {
        Instance.boxesAnimator.SetTrigger("NextEquation");

        OnMoveToNextEquasion.Invoke();
    }
}
