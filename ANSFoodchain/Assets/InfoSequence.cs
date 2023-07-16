using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct TextLine
{
    public string line;
    public float timeLength;
}

public class InfoSequence : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Animator textAnimator;
    [SerializeField] private TextLine[] textLines;
    [SerializeField] private string nextSceneName;
    [SerializeField] private float countdownDuration = 5f;

    private int currentIndex = 0;
    private bool isPaused = false;
    private string lastTextLine = "";

    public LevelLoader levelLoader;

    private void Start()
    {
        textAnimator = gameObject.GetComponent<Animator>();
        displayText = gameObject.GetComponent<TextMeshProUGUI>();

        StartCoroutine(DisplayTextSequence());
    }

    private IEnumerator DisplayTextSequence()
    {
        for (int i = 0; i < textLines.Length; i++)
        {
            string line = textLines[i].line;
            lastTextLine = line;
            float timeLength = textLines[i].timeLength > 0 ? textLines[i].timeLength : 5f;

            // Trigger the fade out animation
            textAnimator.Play("TextFadeOut");

            yield return new WaitForSeconds(GetAnimationLength("TextFadeOut"));

            // Update the display text
            displayText.text = line;

            // Trigger the fade in animation
            textAnimator.Play("TextFadeIn");

            // Pause if necessary
            while (isPaused)
            {
                yield return null;
            }

            yield return new WaitForSeconds(GetAnimationLength("TextFadeIn") + timeLength);

            
        }

        // Fade out the text sequence
        textAnimator.Play("TextFadeOut");

        yield return new WaitForSeconds(GetAnimationLength("TextFadeOut") + 1f);
        textAnimator.Play("TextFadeIn");
        // Countdown sequence
        for (float count = countdownDuration; count > 0; count--)
        {
            displayText.text = count.ToString();
            yield return new WaitForSeconds(1f);
        }

        // Load the next scene
        
        levelLoader.LoadLevel(nextSceneName);
    }

    private float GetAnimationLength(string animationName)
    {
        RuntimeAnimatorController ac = textAnimator.runtimeAnimatorController;

        foreach (AnimationClip clip in ac.animationClips)
        {
            if (clip.name.Equals(animationName))
            {
                return clip.length;
            }
        }

        return 0f;
    }

    private void Update()
    {
        if (HandControl.gameState == HandControl.GameState.WaitPlayerReturn)
        {
            isPaused = true;
            displayText.text = "";
        }
        else if (isPaused)
        {
            isPaused = false;
            displayText.text = lastTextLine;

            // Play the fade in animation to replay the last text
            textAnimator.Play("TextFadeIn");
        }
    }
}
