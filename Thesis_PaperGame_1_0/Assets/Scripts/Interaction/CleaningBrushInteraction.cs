using UnityEngine;

public class CleaningBrushInteraction : MonoBehaviour, IInteractable
{
    // Specific outcomes for this type of interaction using my interaction system setup.
    public void Interact()
    {
        Debug.Log("Interacted With Object!");
    }

    public void OnHover()
    {
        
    }

    public void OnHoverExit()
    {
       
    }

    
}
