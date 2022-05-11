using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class TestSceneController : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextAsset file;
    public TMP_Text phraseObject;
    public AudioClip correctPhrase;
    public AudioClip victorySound;
    public int N; // Amount of phrases in the experiment
    
    private AudioSource audioSource;
    private DateTime start;
    private DateTime end;
    private Random random = new Random();
    private GameObject statsGameObject;
    private int n;
    private bool firstKeystroke;
    private float wpm;
    private float error;
    private string currentString;
    private string rawInput;
    private string[] phrases;

    // Sample random line from phrase set
    private void SetRandomPhrase()
    {
        string newPhrase = phrases[random.Next(0, phrases.Length)].ToLower();
        phraseObject.text = newPhrase;
        
        // Update counter
        GameObject.Find("Counter").GetComponent<TMP_Text>().text = String.Format("{0}/{1}", n, N);
    }
    
    // Calculate current cumulative WPM
    private void UpdateWPM(DateTime start, DateTime end, string input)
    {
        float diff = (float)(end - start).TotalSeconds;
        wpm += (input.Length - 1) / diff * 60 * 0.2f;
    }
    
    // Calculate current cumulative Erroneus Keystrokes Error Rate (EKS ER)
    private void UpdateError()
    {
        // Get diff between all inputs and phrase
        List<char> diff = rawInput.ToList();
        List<char> phrase = currentString.ToList();
        
        for (int i = diff.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < phrase.Count; j++)
            {
                if (diff[i].Equals(phrase[j]))
                {
                    diff.RemoveAt(i);
                    phrase.RemoveAt(j);
                    break;
                }
            }
        }
        
        diff.ForEach(c => Debug.Log(c));
        
        Debug.Log(diff.Count);
        Debug.Log(inputField.text);
        Debug.Log(inputField.text.Length);
        
        
        error += (float) diff.Count / inputField.text.Length * 100;
    }

    public void AddKeystroke(string input)
    {
        rawInput += input;
    }

    private void EndExperiment()
    {
        // Disable text field
        phraseObject.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        
        // Show results
        statsGameObject.SetActive(true);
        
        audioSource.PlayOneShot(victorySound);
        
        TMP_Text wpmGameObject = GameObject.Find("WPM").GetComponent<TMP_Text>();
        wpmGameObject.text = "Words per Minute\n" + Math.Round(wpm / N);
        
        TMP_Text errorGameObject = GameObject.Find("Error Rate").GetComponent<TMP_Text>();
        errorGameObject.text = "Error Rate\n" + Math.Round(error / N);
        
    }

    public void RestartExperiment()
    {
        rawInput = "";
        inputField.text = "";
        wpm = 0;
        error = 0;
        firstKeystroke = false;
        n = 0;
        
        // Enable text field
        phraseObject.gameObject.SetActive(true);
        inputField.gameObject.SetActive(true);
        
        // Hide results
        statsGameObject.SetActive(false);
        
        SetRandomPhrase();
    }

    void Start()
    {
        // Load phrase set
        phrases = Regex.Split(file.text, Environment.NewLine);
        inputField.ActivateInputField();
        statsGameObject = GameObject.Find("Stats");
        statsGameObject.SetActive(false);
        
        audioSource = gameObject.AddComponent<AudioSource>();
        
        SetRandomPhrase();
    }

     void Update()
    {
        currentString = inputField.text;
        
        // Timer starts at first keystroke
        if (!currentString.Equals("") && !firstKeystroke)
        {
            firstKeystroke = true;
            start = DateTime.Now;
        }
        
        // Phrase is correctly copied onto text field
        if (phraseObject.text.Equals(currentString))
        {
            end = DateTime.Now;
            UpdateWPM(start, end, currentString);
            UpdateError();
            
            audioSource.PlayOneShot(correctPhrase);
            n++;
            if (n == N)
            {
                EndExperiment();
            }

            rawInput = "";
            inputField.text = "";
            SetRandomPhrase();
        }

        // Always focus text field
        inputField.ActivateInputField();
    }
}