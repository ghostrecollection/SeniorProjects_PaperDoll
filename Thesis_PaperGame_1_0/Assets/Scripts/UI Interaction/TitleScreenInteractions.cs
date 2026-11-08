using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenInteractions : MonoBehaviour
{
    //[SerializeField] GameObject optionsMenu;
    [SerializeField] Animator optionsMenuAnimator;

    // --- ON PLAY BUTTON ---
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1);
    }


    // --- ON OPTIONS BUTTON ---
    public void OnOptionsButton()
    {
        // Opens a new screen -- will add details later.
        Debug.Log("Accessed options menu!");
        optionsMenuAnimator.Play("OptionsMenu_Open");
    }


    // --- ON EXIT OPTION MENU ---
    public void OnExitOptionMenu()
    {
        Debug.Log("Closed options menu.");
        optionsMenuAnimator.Play("OptionsMenu_Close");
    }


    // --- ON EXIT BUTTON ---
    public void OnExitButton()
    {
        Application.Quit();
        Debug.Log("Application has closed.");
    }
}
