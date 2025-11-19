using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution : MonoBehaviour
{
    public SceneControlScript controller;
    private bool solutionExecuted = false;

    public override void Start()
    {
        controller = gameObject.GetComponent<SceneControlScript>();
    }

    public override void Update()
    {
        if (!solutionExecuted && !controller.IsPlaying())
        {
            ExecuteSolution();
            solutionExecuted = true;
        }
    }

    private void ExecuteSolution()
    {
        int calculatedAnswer = CalculateTotalTacts();

        controller.setAnswer(calculatedAnswer);
    }

    private int CalculateTotalTacts()
    {
        return 0;
    }
}
