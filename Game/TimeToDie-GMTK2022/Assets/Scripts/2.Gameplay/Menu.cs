using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] Transform panel;
    public void Home(int target)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(target);
    }

    public void OpenClose(bool state)
    {
        panel.gameObject.SetActive(state);
    }
}
