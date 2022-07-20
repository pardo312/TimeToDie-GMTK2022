using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TimeToDie
{
    public class CameraController : MonoBehaviour
    {
        private CinemachineStateDrivenCamera cinemachineStateCamera;
        private Animator animator;
        public void Awake()
        {
            animator = GetComponent<Animator>();
            cinemachineStateCamera = GetComponent<CinemachineStateDrivenCamera>();
        }
        public void ChangeCamera(string cameraBoard)
        {
            animator.SetTrigger(cameraBoard);
        }
        public void ChangeTarget(Transform target, bool isLookAt = false)
        {
            if (isLookAt)
                cinemachineStateCamera.LookAt = target;
            else
                cinemachineStateCamera.Follow = target;
        }
    }

    public static class CamerasBoard
    {
        public const string GENERAL_CAM = "GeneralCam";
        public const string FOCUS_CAM = "FocusCam";
        public const string DIE_CAM = "DieCam";
        public const string SHOW_ENEMY_CARD_CAM = "ShowEnemyCardCam";
    }
}

