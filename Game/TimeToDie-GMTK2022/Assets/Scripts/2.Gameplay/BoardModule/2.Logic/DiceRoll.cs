using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeToDie
{
    public class DiceRoll : MonoBehaviour
    {
        #region ----Fields----
        public List<int> diceRolls = new List<int>() { 1, 2, 3, 4, 5, 6 };

        public List<float> lookup = new List<float>();
        public int diceRoll;
        public Camera cam;
        #endregion ----Fields----

        #region ----Methods---
        public void Update()
        {
            DieToMouse();
            if (transform.hasChanged && !shouldRotate)
            {
                diceRoll = GetRollOfDie(transform.up);
                transform.hasChanged = false;
            }
        }

        public const float posY = 1.213232f;
        public void DieToMouse()
        {
            var mousePositionInWorld = cam.ScreenToViewportPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
                GetComponent<Rigidbody>().isKinematic = true;

            if (Input.GetMouseButton(0))
            {
                float valueX = DeClamp(0, 1, -1.5f, 1.3f, mousePositionInWorld.x);
                float valueY = DeClamp(1, 0, -7.3f, -8.1f, mousePositionInWorld.y);
                this.transform.localPosition = new Vector3(valueX, posY, valueY);
                shouldRotate = true;
            }

            if (Input.GetMouseButtonUp(0))
                GetComponent<Rigidbody>().isKinematic = false;

            if (shouldRotate)
                this.transform.rotation = Random.rotation;
        }

        public bool shouldRotate = false;
        private void OnCollisionEnter(Collision collision)
        {
            shouldRotate = false;
        }

        public float DeClamp(float OldMin, float OldMax, float NewMin, float NewMax, float valueToTest)
        {

            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((valueToTest - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
        }

        [ContextMenu("Now")]
        public void SetNewVector()
        {
            lookup.Add(Vector3.Angle(this.transform.up, Vector3.up));
        }

        public float epsilonDeg = 100;
        public int GetRollOfDie(Vector3 referenceVectorUp)
        {
            float min = float.MaxValue;
            int selectedSide = 0;
            for (int i = 0; i < lookup.Count; i++)
            {
                float a = Vector3.Angle(referenceVectorUp, Vector3.up);
                float difference = Mathf.Abs(a - lookup[i]);
                if (difference <= epsilonDeg && difference < min)
                {
                    min = difference;
                    selectedSide = i;
                }
            }
            return (min != float.MaxValue) ? selectedSide + 1 : -1;
        }
        #endregion ----Methods---
    }
}