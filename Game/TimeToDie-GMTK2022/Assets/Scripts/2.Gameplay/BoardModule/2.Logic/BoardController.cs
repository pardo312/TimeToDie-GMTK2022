using System;
using System.Collections;
using System.Collections.Generic;
using TimeToDie.BoardModule;
using TimeToDie.DataManagerModule;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimeToDie
{
    public class BoardController : MonoBehaviour
    {
        [Header("Cards")]
        public SpawnEnemyCardsOnBoard enemyCards;
        public ShowEnemyCard showEnemyCard;

        [Header("Cams/UI")]
        public CameraController camController;
        public CanvasGroup mainMenu;
        public GameObject collidersMap;
        public CanvasGroup fadeGroup;
        public TMP_Text levelValue;

        [Header("Dices")]
        public List<DiceRoll> dicesPrefab;
        public List<DiceRoll> currentDiceGroup;
        public List<Transform> dicePositions;
        public int currentDie;
        public void Start()
        {
            if (DataManager.instance.currentLevel != -1)
            {
                levelValue.text = DataManager.instance.currentLevel.ToString();
                mainMenu.gameObject.SetActive(false);
                StartCoroutine(GivePlayerGroupOfDice());
            }
        }

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
            showEnemyCard.Init(camController, currentDiceGroup.Count);
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
                 showEnemyCard.SetCardInPosition(dieNumber, other, die, OnCardShowned, OnEnemiesSelected);
             }));
        }

        public void OnCardShowned(int enemyIndex)
        {
            //Next die
            currentDie++;
            currentDiceGroup[currentDie].useMouseInput = true;
            camController.ChangeTarget(currentDiceGroup[currentDie].transform);
            collidersMap.SetActive(true);
        }

        public void OnEnemiesSelected(List<EnemyInitData> enemiesData)
        {
            //Fade to black
            LeanTween.value(0, 1, 1f).setOnUpdate((float value) => fadeGroup.alpha = value).setOnComplete(() =>
            {
                //GO-TO-Gameplay
                DataManager.instance.enemiesData = enemiesData;
                SceneManager.LoadScene("Gameplay");
            });
        }

        IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}
