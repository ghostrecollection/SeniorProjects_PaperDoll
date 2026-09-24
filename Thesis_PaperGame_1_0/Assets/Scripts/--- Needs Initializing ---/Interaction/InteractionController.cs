using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public static InteractionController instance { get; private set; }
    public Interactable interact;
}
