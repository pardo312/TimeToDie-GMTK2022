using System;
using System.Collections;
using System.Collections.Generic;
using TimeToDie.BoardModule;
using UnityEngine;

namespace TimeToDie
{
    public class BoardController : MonoBehaviour
    {
        public SpawnEnemyCardsOnBoard enemyCards;
        public CameraController camController;
        public CanvasGroup mainMenu;
        public ShowEnemyCard showEnemyCard;
        public List<DiceRoll> dice;

        public void InitPlay()
        {
            StartCoroutine(HideMainMenu());
        }

        IEnumerator HideMainMenu()
        {
            while (mainMenu.alpha > 0)
            {
                mainMenu.alpha -= 0.5f * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            camController.ChangeCamera(CamerasBoard.FOCUS_CAM);
            enemyCards.SpawnEnemyCards();
            showEnemyCard.Init(camController);
            dice.ForEach(die =>
            {
                die.Init();
                die.onDiceRolled += GetCardAndDieNumber;
            });
        }
        public void GetCardAndDieNumber(int dieNumber, Collision other)
        {
            camController.ChangeCamera(CamerasBoard.DIE_CAM);
            StartCoroutine(WaitForSeconds(1, () =>
             {
                 camController.ChangeCamera(CamerasBoard.SHOW_ENEMY_CARD_CAM);
                 showEnemyCard.SetCardInPosition(dieNumber,other);

             }));
            Debug.Log("Number:" + dieNumber);
        }
        IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}
