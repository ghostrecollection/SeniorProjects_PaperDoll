using UnityEngine;

public class CleaningBrushInteraction : MonoBehaviour, IInteractable
{
    // -------------------------------------
    [Header("Canvas Components:")]
    [Space(10)]
    [SerializeField] GameObject interactPrompt;


    // Specific outcomes for this type of interaction using my interaction system setup.
    public void Interact()
    {
        Debug.Log("Interacted With Object!");
    }

    public void OnHover()
    {
        // Turns on the canvas component when the mouse hovers.
        interactPrompt.SetActive(true);
    }

    public void OnHoverExit()
    {
        // Turns off canvas component when the mouse leaves.
        interactPrompt.SetActive(false);

    }


}
