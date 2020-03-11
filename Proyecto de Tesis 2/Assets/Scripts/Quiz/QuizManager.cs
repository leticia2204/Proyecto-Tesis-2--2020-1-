using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class QuizManager : MonoBehaviour
{
    [SerializeField] private AudioClip correctSound = null;
    [SerializeField] private AudioClip incorrectSound = null;
    [SerializeField] private Color correctColor = Color.black;
    [SerializeField] private Color incorrectColor = Color.black;
    [SerializeField] private float waitTime = 0.0f;

    private QuizBD quizbd = null;
    private QuizUI quizui = null;
    private AudioSource audioSource = null;

    private void Start()
    {
        quizbd = GameObject.FindObjectOfType<QuizBD>();
        quizui = GameObject.FindObjectOfType<QuizUI>();
        audioSource = GetComponent<AudioSource>();
        NextQuestion();
    }
    private void NextQuestion()
    {
        quizui.Construct(quizbd.GetRandom(), GiveAnswer);
    }

    private void GiveAnswer(OptionButton optionButton)
    {
        StartCoroutine(GiveAnswerRoutime(optionButton));
    }
    

    private IEnumerator GiveAnswerRoutime(OptionButton optionButton)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
        audioSource.clip = optionButton.Option.correct ? correctSound : incorrectSound;
        optionButton.SetColor(optionButton.Option.correct ? correctColor : incorrectColor);

        audioSource.Play();
        yield return new WaitForSeconds(waitTime);

        if (optionButton.Option.correct)
            NextQuestion();
        else
            GameOver();
    }

    private void GameOver()
    {
        //Logica de gameOver
    }
}
