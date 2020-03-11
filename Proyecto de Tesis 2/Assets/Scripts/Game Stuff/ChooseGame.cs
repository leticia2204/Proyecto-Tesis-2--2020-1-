using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseGame : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {

    }

    //Ir al primer juego
    public void IrJuegoTema1()
    {
        //SceneManager.LoadScene("04JuegoTema1");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Ir al segundo juego juego
    public void IrJuegoTema2()
    {
        //SceneManager.LoadScene("05JuegoTema2");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void IrJuegoTema3()
    {
        //SceneManager.LoadScene("06JuegoTema3");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
}
