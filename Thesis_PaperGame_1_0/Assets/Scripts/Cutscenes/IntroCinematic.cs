using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroCinematic : MonoBehaviour
{
    // -------------------------------------
    [SerializeField] VideoPlayer introCinematicPlayer;
    bool checkForVideoEnd = false;
    // -------------------------------------


    // --- START ---
    void Start()
    {
        // Mouse state:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Bool to delay the check for video playing.
        checkForVideoEnd = false;
        // Delay Coroutine.
        StartCoroutine(Wait());
        
    }


    // --- UPDATE ---
    void Update()
    {
        // After delay:
        if (checkForVideoEnd)
        {
            // Check if the video is done.
            // Debug.Log(introCinematicPlayer.isPlaying);
            if (introCinematicPlayer.isPlaying == false)
            {
                StartCoroutine(SceneChangeDelay());
            }
        }
    }


    // --- WAIT ---
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        checkForVideoEnd = true;
    }


    // --- SCENE CHANGE DELAY ---
    IEnumerator SceneChangeDelay()
    {
        yield return new WaitForSeconds(1);

        // What happens when the video is over:
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2);
    }
}
