using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour {
    public AudioClip[] sounds;
    AudioSource audio;
    public Canvas canvas1;
	public Canvas canvas2;
	public Canvas canvas3;
    void PlaySound(int clip)
    {   
        audio.clip = sounds[clip];
        audio.Play();
    }
    public void quit()
    {
        Application.Quit();
    }

    public void restart()
    {
        //ScoreController.score = 0;
        SceneManager.LoadScene(0);
    }

    public void resume()
    {
        //PlaySound(0);
        Time.timeScale = 1;
        canvas1.enabled = false;
    }

    // Use this for initialization
    void Start()
    {
        canvas1 = canvas1.GetComponent<Canvas>();
		//canvas2 = canvas2.GetComponent<Canvas>();
		//canvas3 = canvas3.GetComponent<Canvas>();
        canvas1.enabled = false;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 0;
            canvas1.enabled = true;
        }

		if (Input.GetKeyDown("r"))
		{
			
			//canvas2.enabled = true;
		}

		if (Input.GetKeyDown("f"))
		{
			
			//canvas3.enabled = true;
		}

    }
}
