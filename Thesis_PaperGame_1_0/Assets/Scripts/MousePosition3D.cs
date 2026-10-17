using UnityEngine;
using UnityEngine.InputSystem;

public class MousePosition3D : MonoBehaviour
{
    [SerializeField] private Camera cameraRoot;
    [SerializeField] private LayerMask layerMask;


    // --- UPDATE ---
    void Update()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cameraRoot.ScreenPointToRay(mousePos);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point;
            Debug.Log(mousePos);
        }

    }
}
