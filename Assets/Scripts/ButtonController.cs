using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvas3;
    public Button newGame;
    public Button options;
    public Button muteBtn;

    public void mute()
    {
        if (muteBtn.GetComponentInChildren<Text>().text == "Mute Sound")
        {
            AudioListener.volume = 0;
            muteBtn.GetComponentInChildren<Text>().text = "Unmute Sound";
        }
        else
        {
            AudioListener.volume = 1;
            muteBtn.GetComponentInChildren<Text>().text = "Mute Sound";
        }


    }

    public void hideOptions()
    {
        canvas3.enabled = false;
        canvas1.enabled = false;
        canvas2.enabled = false;
        newGame.enabled = true;
        options.enabled = true;
    }
    public void showOptions()
    {
        canvas3.enabled = false;
        canvas1.enabled = true;
        canvas2.enabled = false;
        newGame.enabled = false;
        options.enabled = false;
    }

    public void showHowToPlay()
    {
        canvas3.enabled = false;
        canvas1.enabled = false;
        canvas2.enabled = true;
        newGame.enabled = false;
        options.enabled = false;
    }

    public void showCredits()
    {
        canvas3.enabled = true;
        canvas1.enabled = false;
        canvas2.enabled = false;
        newGame.enabled = false;
        options.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        canvas1 = canvas1.GetComponent<Canvas>();
        newGame = newGame.GetComponent<Button>();
        options = options.GetComponent<Button>();
        muteBtn = muteBtn.GetComponent<Button>();
        canvas1.enabled = false;
        canvas2.enabled = false;
        canvas3.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
