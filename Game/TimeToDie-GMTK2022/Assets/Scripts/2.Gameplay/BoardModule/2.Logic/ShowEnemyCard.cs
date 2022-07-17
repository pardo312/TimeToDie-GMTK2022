using System;
using System.Collections;
using System.Collections.Generic;
using TimeToDie.BoardModule;
using UnityEngine;

namespace TimeToDie
{
    public class ShowEnemyCard : MonoBehaviour
    {
        private int currentPosition = 0;
        public List<Transform> listOfCardsPositions;
        public List<GameObject> listOfEnemies;
        public CameraController camController;
        public void Init(CameraController _camController)
        {
            camController = _camController;
        }
        public void SetCardInPosition(int dieNumber, Collision other)
        {
            if (currentPosition < listOfCardsPositions.Count)
            {
                LeanTween.move(other.gameObject, listOfCardsPositions[currentPosition], 1f).setOnComplete(() =>
                {
                    SpawnEnemyInCard(listOfCardsPositions[currentPosition]);
                    currentPosition++;
                    if (currentPosition >= listOfCardsPositions.Count)
                    {
                        //Endboard, pass data
                        Debug.Log("lol");
                    }
                });
            }

        }

        public void SpawnEnemyInCard(Transform other)
        {
            GameObject enemy = Instantiate(listOfEnemies[UnityEngine.Random.Range(0, listOfEnemies.Count)], other.position, Quaternion.identity);
            LeanTween.moveY(enemy, enemy.transform.position.y + 1, 1f).setOnComplete(() =>
            {
                camController.ChangeCamera(CamerasBoard.GENERAL_CAM);
            });
        }

    }
}
