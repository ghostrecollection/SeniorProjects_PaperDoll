using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPrototype : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(3);
        }
        
    }
}
