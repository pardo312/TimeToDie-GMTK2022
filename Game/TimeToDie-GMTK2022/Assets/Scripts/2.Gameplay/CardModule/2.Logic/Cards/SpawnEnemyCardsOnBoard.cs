using System;
using System.Collections;
using UnityEngine;

namespace TimeToDie.BoardModule
{
    public class SpawnEnemyCardsOnBoard : MonoBehaviour
    {
        #region ----Fields----
        public const int MAX_CARDS_ON_ROW = 8;

        [Header("References")]
        public Transform paperTransform;
        public Transform cardDeckTransform;
        public GameObject cardPrefab;

        [Header("Animation")]
        [Range(.01f,1f)]public float animationTime;

        [Header("Options")]
        public int numberOfCards;
        #endregion ----Fields----

        #region ----Methods----
        [ContextMenu("Spawn cards")]
        public void SpawnEnemyCards(Action onComplete = null)
        {
            StartCoroutine(SpawnCards(onComplete ));
        }

        public IEnumerator SpawnCards(Action onComplete)
        {
            LeanTween.cancelAll();
            int numberOfRows = Mathf.CeilToInt(numberOfCards * 1f / MAX_CARDS_ON_ROW);

            for (int i = 0; i < numberOfRows; i++)
            {
                int numberOfColumns = i == numberOfRows - 1 ? (numberOfCards % MAX_CARDS_ON_ROW) : MAX_CARDS_ON_ROW;
                int halfNumberOfColumns = (int)(numberOfColumns * 1f / 2);

                for (int j = -halfNumberOfColumns; j < halfNumberOfColumns; j++)
                {
                    GameObject cardInstance = Instantiate(cardPrefab, paperTransform);
                    Vector3 initPos = cardInstance.transform.position;
                    Vector3 plusPos = new Vector3(j * cardInstance.transform.localScale.x, 0, -(i * cardInstance.transform.localScale.y));
                    Vector3 finalPos = initPos + plusPos;

                    cardInstance.transform.position = new Vector3(cardDeckTransform.position.x, cardDeckTransform.position.y, cardDeckTransform.position.z);
                    LeanTween.move(cardInstance, finalPos, animationTime);
                    yield return new WaitForSeconds(animationTime);
                }
            }
            onComplete?.Invoke();
        }
        #endregion ----Methods----
    }
}