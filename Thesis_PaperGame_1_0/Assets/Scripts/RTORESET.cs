using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RTORESET : MonoBehaviour
{
    public void SceneLoad()
    {
        SceneManager.LoadScene(0);
    }
}

