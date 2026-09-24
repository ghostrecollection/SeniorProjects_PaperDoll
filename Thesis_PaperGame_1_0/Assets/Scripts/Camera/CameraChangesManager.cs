using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraChangesManager : MonoBehaviour
{
    [Header("Camera Switch Set Up")]
    [Space(10)]
    public int priorityNum;
    [SerializeField] public CinemachineCamera[] allCameras;
    [SerializeField] public CinemachineCamera currentCam;
    public UnityEvent trigger;

    // --- START ---
    public void Start()
    {
        allCameras = FindObjectsByType<CinemachineCamera>();
    }


    // --- ON TRIGGER EXIT ---
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (var cam in allCameras)
            {
                cam.Priority = 0;
            }

            currentCam.Priority = priorityNum;
            trigger.Invoke();
        }
    }
    
    public void ChangeCamera()
    {
        foreach (var cam in allCameras)
        {
            cam.Priority = 0;
        }

        currentCam.Priority = priorityNum;
    }
    
}
