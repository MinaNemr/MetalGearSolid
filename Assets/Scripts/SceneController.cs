using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour {
  
	// Use this for initialization
	void Start () {
       
	}

    public void startScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void resume()
    {
        Time.timeScale = 1;
        startScene(1);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 0;
            startScene(0);
        }
	}

}
