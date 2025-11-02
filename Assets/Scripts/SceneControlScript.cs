using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class SceneControlScript : MonoBehaviour
{
    public GameObject[] notes;
    private NoteLogic[] noteLogics;
    private int currentPosition = 0;
    private string path = "output.txt";
    private bool isActive = false;
    private Coroutine playbackRoutine;
    private bool answerSubmitted = false;

    private int alpha = 1;
    private int beta = 1;
    private int gamma = 0;

    void Start()
    {

        noteLogics = notes.Select(note => note.GetComponent<NoteLogic>()).ToArray();
        LoadInputData();
        BeginPlayback();
    }

    private void LoadInputData()
    {
        if (File.Exists("input.txt"))
        {
            string input = File.ReadAllText("input.txt");
            string[] values = input.Trim().Split(' ');

            for (int i = 0; i < Mathf.Min(values.Length, noteLogics.Length); i++)
            {
                if (int.TryParse(values[i], out int maxPlays))
                {
                    noteLogics[i].maxPlays = maxPlays;
                }
            }
        }
    }

    public void BeginPlayback()
    {
        if (playbackRoutine != null)
            StopCoroutine(playbackRoutine);

        playbackRoutine = StartCoroutine(PlaybackSequence());
    }

    private IEnumerator PlaybackSequence()
    {
        isActive = true;
        gamma = 0;

        while (isActive)
        {
            if (noteLogics[currentPosition].CanPlay())
            {
                noteLogics[currentPosition].PlayNote();
                gamma++;

                yield return new WaitForSeconds(1.0f);

                CalculateNextPosition();
            }
            else
            {
                isActive = false;
                if (!answerSubmitted)
                {
                    StartCoroutine(DelayedShutdown(2f));
                }
                yield break;
            }
        }
    }

    private void CalculateNextPosition()
    {
        int delta = (alpha + beta) % 7;
        alpha = beta;
        beta = delta;
        currentPosition = delta;
    }

    public void StopPlayback()
    {
        isActive = false;
        if (playbackRoutine != null)
            StopCoroutine(playbackRoutine);
    }

    public void ContinuePlayback()
    {
        if (!isActive)
            BeginPlayback();
    }

    public int GetNoteLimit(int index) => noteLogics[index].maxPlays;
    public NoteLogic[] GetAllNotes() => noteLogics;
    public int GetTotalNotes() => notes.Length;
    public bool IsPlaying() => isActive;

    public void setAnswer(int answer)
    {
        File.WriteAllText(path, answer.ToString());
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator DelayedShutdown(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!answerSubmitted)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}