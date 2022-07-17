using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeToDie
{
    public class DiceRoll : MonoBehaviour
    {
        #region ----Fields----
        public List<int> diceRolls = new List<int>() { 1, 2, 3, 4, 5, 6 };

        public List<Vector3> lookup = new List<Vector3>();
        public int diceRoll;
        public Camera cam;
        #endregion ----Fields----

        #region ----Methods---
        #region <<<roll6DDice>>>
        public void Update()
        {
            DieToMouse();
            if (transform.hasChanged)
            {
                //int? diceRoll = -1;
                //diceRoll = CheckVector(transform.right, 1);
                //if (diceRoll == null)
                //{
                //    diceRoll = CheckVector(transform.up, 2);
                //    if (diceRoll == null)
                //        diceRoll = CheckVector(transform.forward, 3);
                //}

                diceRoll = GetRollOfDie();
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

            if(shouldRotate)
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
        public int? CheckVector(Vector3 vectorToCheck, int value)
        {
            if ((int)Vector3.Cross(Vector3.up, vectorToCheck).magnitude == 0)
            {
                Debug.Log($"value:{value} Dot:{Vector3.Dot(Vector3.up, vectorToCheck)}");
                if (Vector3.Dot(Vector3.up, vectorToCheck) > 0)
                    return diceRolls[value];
                else
                    return diceRolls[diceRolls.Count - value];
            }
            return null;
        }
        #endregion <<<roll6DDice>>>


        [ContextMenu("Now")]
        public void SetNewVector()
        {
            lookup.Add(transform.up);
        }

        public int GetRollOfDie(Vector3? referenceVectorUp = null, float epsilonDeg = 5f)
        {
            Vector3 referenceVectorUpValue;
            if (referenceVectorUp == null)
                referenceVectorUpValue = Vector3.up;
            else
                referenceVectorUpValue = referenceVectorUp.Value;

            Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUpValue);

            // Find smallest difference to object space direction
            float min = float.MaxValue;
            int selectedSide = 0;
            for (int i = 0; i < lookup.Count; i++)
            {
                float a = Vector3.Angle(referenceObjectSpace, lookup[i]);
                if (a <= epsilonDeg && a < min)
                {
                    min = a;
                    selectedSide = i;
                }
            }
            return (min < epsilonDeg) ? selectedSide : -1; // -1 as error code for not within bounds
        }
        #endregion ----Methods---
    }
}