using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeToDie
{
    public class CameraController : MonoBehaviour
    {
        public Animator animator;
        public void ChangeCamera(string cameraBoard)
        {
            animator.SetTrigger(cameraBoard);
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

