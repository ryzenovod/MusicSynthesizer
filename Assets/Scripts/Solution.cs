using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution : MonoBehaviour
{
    public SceneControlScript controller;
    private bool solutionExecuted = false;

    public void Start()
    {
        controller = gameObject.GetComponent<SceneControlScript>();
    }

    public void Update()
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
        int totalNotes = controller.GetTotalNotes();
        int[] limits = new int[totalNotes];

        for (int i = 0; i < totalNotes; i++)
        {
            limits[i] = controller.GetNoteLimit(i);
        }

        int alpha = 1;
        int beta = 1;
        int currentPosition = 0;
        int gamma = 0;

        while (limits[currentPosition] > 0)
        {
            limits[currentPosition]--;
            gamma++;

            int delta = (alpha + beta) % totalNotes;
            alpha = beta;
            beta = delta;
            currentPosition = delta;
        }

        return gamma;
    }
}
