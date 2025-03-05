using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : CoduckStudio.Utils.Singleton<CameraManager>
{
    public CinemachineCamera cinemachineCamera;
    public CinemachineFollow cinemachineCameraFollow;
    public Camera mainCamera;

    void Awake()
    {
        SetLevelBuildingTarget();
    }

    public void SetLevelBuildingTarget()
    {
        cinemachineCameraFollow.enabled = false;
        cinemachineCamera.transform.position = new Vector3(0, 0, -10);
    }
    
    public void SetPlayerTarget()
    {
        cinemachineCameraFollow.enabled = true;
    }
}
