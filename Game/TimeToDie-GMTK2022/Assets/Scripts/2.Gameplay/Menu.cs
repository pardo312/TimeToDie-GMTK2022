using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] Transform panel;
    public void Home()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OpenClose(bool state)
    {
        panel.gameObject.SetActive(state);
    }
}
