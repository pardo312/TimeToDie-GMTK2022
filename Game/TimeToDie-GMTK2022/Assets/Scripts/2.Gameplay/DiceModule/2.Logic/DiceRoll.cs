using System;
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
        public bool useMouseInput = false;
        public float epsilonDeg = 1;
        public Action<int, Collision> onDiceRolled;

        private Camera cam;
        private Rigidbody rb;
        private bool ready = false;
        #endregion ----Fields----

        #region ----Methods---
        #region <<<Unity Methods>>>
        public void Init()
        {
            ready = true;
            cam = Camera.main;
            rb = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            if (!ready)
                return;

            if (useMouseInput)
                DieToMouse();
        }

        #endregion <<<Unity Methods>>>

        #region <<<Dice to mouse>>>
        public void DieToMouse()
        {
            var mousePositionInWorld = cam.ScreenToViewportPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (waitToStopCoroutine == null)
                    return;
                StopCoroutine(waitToStopCoroutine);
                waitToStopCoroutine = null;
            }
            else if (Input.GetMouseButton(0))
            {
                float valueX = DeClamp(.2f, .6f, -.6f, .3f, Mathf.Clamp(mousePositionInWorld.x, .2f, .6f));
                float valueY = DeClamp(.4f, .8f, -7.6f, -7f, Mathf.Clamp(mousePositionInWorld.y, .4f, .8f));
                this.transform.localPosition = new Vector3(valueX, POS_Y_PICKUP_DIE, valueY);
                this.transform.rotation = UnityEngine.Random.rotation;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                rb.isKinematic = false;
                useMouseInput = false;
                waitToStopCoroutine = StartCoroutine(WaitForDieToStop());
            }
        }
        Coroutine waitToStopCoroutine = null;

        public IEnumerator WaitForDieToStop()
        {

            yield return new WaitUntil(() => currentCollision != null);
            var lastCollision = currentCollision;
            while (currentCollision != null)
            {
                lastCollision = currentCollision;
                currentCollision = null;
                yield return new WaitForSeconds(1f);
            }
            dieRoll = GetRollOfDie(transform.up);
            rb.isKinematic = true;
            onDiceRolled?.Invoke(dieRoll, lastCollision);
            currentCollision = null;

        }
        private Collision currentCollision = null;
        private void OnCollisionEnter(Collision other)
        {
            currentCollision = other;
        }

        public float DeClamp(float OldMin, float OldMax, float NewMin, float NewMax, float valueToTest)
        {
            float OldRange = (OldMax - OldMin);
            float NewRange = (NewMax - NewMin);
            float NewValue = (((valueToTest - OldMin) * NewRange) / OldRange) + NewMin;

            return (NewValue);
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