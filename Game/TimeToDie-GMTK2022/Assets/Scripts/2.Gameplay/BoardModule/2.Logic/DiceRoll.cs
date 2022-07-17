using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeToDie
{

    public class DiceRoll : MonoBehaviour
    {
        #region ----Fields----
        private const float POS_Y_PICKUP_DIE = 1.213232f;
        public DiceAngleSO diceAngles;
        public int dieRoll;
        public float epsilonDeg = 1;

        private Camera cam;
        private bool shouldRotate = false;
        #endregion ----Fields----

        #region ----Methods---
        #region <<<Unity Methods>>>
        public void Awake()
        {
            cam = Camera.main;
        }

        public void Update()
        {
            DieToMouse();
            if (transform.hasChanged && !shouldRotate)
            {
                dieRoll = GetRollOfDie(transform.up);
                transform.hasChanged = false;
            }
        }
        #endregion <<<Unity Methods>>>

        #region <<<Dice to mouse>>>
        public void DieToMouse()
        {
            var mousePositionInWorld = cam.ScreenToViewportPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
                GetComponent<Rigidbody>().isKinematic = true;

            if (Input.GetMouseButton(0))
            {
                float valueX = DeClamp(0, 1, -1.5f, 1.3f, mousePositionInWorld.x);
                float valueY = DeClamp(1, 0, -7.3f, -8.1f, mousePositionInWorld.y);
                this.transform.localPosition = new Vector3(valueX, POS_Y_PICKUP_DIE, valueY);
                shouldRotate = true;
            }

            if (Input.GetMouseButtonUp(0))
                GetComponent<Rigidbody>().isKinematic = false;

            if (shouldRotate)
                this.transform.rotation = Random.rotation;
        }

        public float DeClamp(float OldMin, float OldMax, float NewMin, float NewMax, float valueToTest)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((valueToTest - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

        private void OnCollisionEnter(Collision collision)
        {
            shouldRotate = false;
        }
        #endregion <<<Dice to mouse>>>

        #region <<<Get dice value>>>
        [ContextMenu("Now")]
        public void SetNewVector()
        {
            diceAngles.diceAngles.Add(Vector3.Angle(this.transform.up, Vector3.up));
        }
        public int GetRollOfDie(Vector3 referenceVectorUp)
        {
            float min = float.MaxValue;
            int selectedSide = 0;
            for (int i = 0; i < diceAngles.diceAngles.Count; i++)
            {
                float a = Vector3.Angle(referenceVectorUp, Vector3.up);
                float difference = Mathf.Abs(a - diceAngles.diceAngles[i]);
                if (difference <= epsilonDeg && difference < min)
                {
                    min = difference;
                    selectedSide = i;
                }
            }
            return (min != float.MaxValue) ? selectedSide + 1 : -1;
        }
        #endregion <<<Get dice value>>>
        #endregion ----Methods---
    }
}