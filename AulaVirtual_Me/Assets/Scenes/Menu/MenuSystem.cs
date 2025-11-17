using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public GameObject menu;
    public GameObject menuNiveles;
    public void Jugar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MenuNiveles()
    {
        menu.SetActive(false);
        menuNiveles.SetActive(true);
    }

    public void MenuVolver()
    {
        menu.SetActive(true);
        menuNiveles.SetActive(false);
    }
    public void Salir()
    {
        Application.Quit();
    }
}