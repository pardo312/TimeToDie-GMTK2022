using System;
using System.Collections;
using System.Collections.Generic;
using TimeToDie.BoardModule;
using TimeToDie.DataManagerModule;
using UnityEngine;

namespace TimeToDie
{
    public class ShowEnemyCard : MonoBehaviour
    {
        #region ----Fields----
        public List<Transform> listOfCardsPositions;
        public List<GameObject> listOfEnemies;

        private int currentPosition = 0;
        private int maxPosition = 10000;
        private CameraController camController;
        private List<EnemyInitData> enemiesData = new List<EnemyInitData>();
        #endregion ----Fields----

        #region ----Methods----
        public void Init(CameraController _camController, int _maxPosition = 10000)
        {
            camController = _camController;
            maxPosition = _maxPosition;
            enemiesData = new List<EnemyInitData>();
        }

        public void SetCardInPosition(int dieNumber, Collision other, DiceRoll die, Action<int> onFinishShowingEnemy, Action<List<EnemyInitData>> onEndSelectingEnemies)
        {
            if (currentPosition < listOfCardsPositions.Count)
            {
                LeanTween.move(die.gameObject, (listOfCardsPositions[currentPosition].position + new Vector3(0, 0, -.1f)), 1f);
                LeanTween.move(other.gameObject, listOfCardsPositions[currentPosition], 1f).setOnComplete(() =>
                {
                    SpawnEnemyInCard(listOfCardsPositions[currentPosition], (enemyIndex) =>
                    {
                        currentPosition++;
                        enemiesData.Add(new EnemyInitData() { enemyType = enemyIndex, numberOfEnemies = dieNumber });

                        if (currentPosition >= listOfCardsPositions.Count || currentPosition >= maxPosition)
                            onEndSelectingEnemies?.Invoke(enemiesData);
                        else
                            onFinishShowingEnemy?.Invoke(enemyIndex);
                    });
                });
            }
        }

        public void SpawnEnemyInCard(Transform other, Action<int> onFinishShowingEnemy)
        {
            int enemyIndex = UnityEngine.Random.Range(0, listOfEnemies.Count);
            GameObject enemy = Instantiate(listOfEnemies[enemyIndex], other);
            enemy.transform.position -= Vector3.up * .5f;
            LeanTween.moveY(enemy, enemy.transform.position.y + .5f, 2f).setOnComplete(() =>
            {
                StartCoroutine(WaitForSeconds(2, () =>
                {
                    onFinishShowingEnemy?.Invoke(enemyIndex);
                    camController.ChangeCamera(CamerasBoard.FOCUS_CAM);
                }));
            });
        }
        IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
        #endregion ----Methods----
    }
}
