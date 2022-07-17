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
        public GameObject collidersMap;

        public List<DiceRoll> dicesPrefab;
        public List<DiceRoll> currentDiceGroup;
        public int currentDie;
        public List<Transform> dicePositions;

        public void InitPlay()
        {
            StartCoroutine(HideMainMenu(() =>
                StartCoroutine(GivePlayerGroupOfDice())
            ));
        }

        public IEnumerator GivePlayerGroupOfDice()
        {
            currentDiceGroup = new List<DiceRoll>();
            currentDie = 0;
            for (int i = 0; i < dicePositions.Count; i++)
            {
                DiceRoll newDie = Instantiate(dicesPrefab[UnityEngine.Random.Range(0, dicesPrefab.Count)], dicePositions[i].position, dicePositions[i].rotation);
                currentDiceGroup.Add(newDie);
                camController.ChangeCamera(CamerasBoard.DIE_CAM);
                if (i == 0)
                    newDie.useMouseInput = true;
                camController.ChangeTarget(newDie.transform);

                yield return new WaitForSeconds(2f);
            }
            camController.ChangeTarget(currentDiceGroup[0].transform);
            camController.ChangeCamera(CamerasBoard.GENERAL_CAM);

            yield return new WaitForSeconds(2);

            camController.ChangeCamera(CamerasBoard.FOCUS_CAM);
            showEnemyCard.Init(camController);
            enemyCards.SpawnEnemyCards(() =>
            {
                currentDiceGroup.ForEach(die =>
                {
                    die.Init();
                    die.onDiceRolled += (dieNumber, other) => GetCardAndDieNumber(dieNumber, other, die);
                });
            });
        }

        IEnumerator HideMainMenu(Action onComplete)
        {
            while (mainMenu.alpha > 0)
            {
                mainMenu.alpha -= 0.5f * Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            onComplete?.Invoke();
        }

        public void GetCardAndDieNumber(int dieNumber, Collision other, DiceRoll die)
        {
            camController.ChangeCamera(CamerasBoard.DIE_CAM);
            StartCoroutine(WaitForSeconds(2, () =>
             {
                 collidersMap.SetActive(false);
                 camController.ChangeCamera(CamerasBoard.SHOW_ENEMY_CARD_CAM);
                 showEnemyCard.SetCardInPosition(dieNumber, other, die, (enemyIndex) =>
                   {
                       //Next die
                       currentDie++;
                       currentDiceGroup[currentDie].useMouseInput = true;
                       camController.ChangeTarget(currentDiceGroup[currentDie].transform);
                       collidersMap.SetActive(true);
                   });
             }));
        }
        IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}
