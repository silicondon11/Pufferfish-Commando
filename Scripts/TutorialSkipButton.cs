using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSkipButton : MonoBehaviour
{
    private Button button;
    private MusicController musicController;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(SkipTutorial);
        }

        musicController = GameObject.Find("Audio").GetComponent<MusicController>();
    }


    public void SkipTutorial()
    {
        StartCoroutine(musicController.PlaySoundEffect("ButtonClick"));

        TutorialManager tut = FindObjectOfType<TutorialManager>();

        tut.SkipTutorial();
    }

}
