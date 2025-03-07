using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : CoduckStudio.Utils.Singleton<CameraManager>
{
    public CinemachineCamera cinemachineCamera;
    public CinemachineFollow cinemachineCameraFollow;
    public Camera mainCamera;
    public CameraTarget cameraTarget;
    public Transform playerTransform;
    public Transform levelCenterTransform;

    void Awake()
    {
        SetLevelBuildingTarget();
    }

    public void SetLevelBuildingTarget()
    {
        cameraTarget.target = levelCenterTransform;
    }
    
    public void SetPlayerTarget()
    {
        cameraTarget.target = playerTransform;
    }
}
