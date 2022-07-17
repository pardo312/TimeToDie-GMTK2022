using UnityEngine;

namespace TimeToDie.BoardModule
{
    public class SpawnEnemyCardsOnBoard : MonoBehaviour
    {
        #region ----Fields----
        public const int MAX_CARDS_ON_ROW = 8;

        public Transform paperTransform;
        public GameObject cardPrefab;
        public int numberOfCards;
        #endregion ----Fields----

        #region ----Methods----
        [ContextMenu("Spawn cards")]
        public void SpawnEnemyCards()
        {
            int numberOfRows = Mathf.CeilToInt(numberOfCards * 1f / MAX_CARDS_ON_ROW);

            for (int i = 0; i < numberOfRows; i++)
            {
                int numberOfColumns = i == numberOfRows - 1 ? (numberOfCards % MAX_CARDS_ON_ROW) : MAX_CARDS_ON_ROW;
                int halfNumberOfColumns = (int)(numberOfColumns * 1f / 2);

                for (int j = -halfNumberOfColumns; j < halfNumberOfColumns; j++)
                {
                    GameObject cardInstance = Instantiate(cardPrefab, paperTransform);
                    Vector3 plusPosition = new Vector3(j * cardInstance.transform.localScale.x, 0, -(i * cardInstance.transform.localScale.y));
                    cardInstance.transform.position += plusPosition;

                    Debug.Log($"-----------------Card#{i * (j + 4)}------------");
                    Debug.Log($"plus{plusPosition }");
                    Debug.Log($"final{cardInstance.transform.position}");
                }
            }
        }
        #endregion ----Methods----
    }
}