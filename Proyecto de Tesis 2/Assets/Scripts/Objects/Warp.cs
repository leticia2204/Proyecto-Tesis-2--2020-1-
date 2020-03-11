using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Warp : MonoBehaviour
{
    //almacena punto de destino
    public GameObject target;
    //almacena el mapa destino
    public GameObject targetMap;
    //Controla inicio de la transicion
    bool start = false;
    //transicion de entrada o salida
    bool isFadeIn = false;
    //opacidad inicial del cuadrado de transicion
    float alpha = 0;
    //transicion de 1 seg
    float fadeTime = 1f;

    GameObject area;
    void Awake()
    {
        Assert.IsNotNull(target);
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        Assert.IsNotNull(targetMap);
        // Buscamos el Area para mostrar el texto
        area = GameObject.FindGameObjectWithTag("Area");
    }
    IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Animator>().enabled = false;
            other.GetComponent<Player>().enabled = false;

            FadeIn();
            yield return new WaitForSeconds(fadeTime);

            other.transform.position = target.transform.GetChild(0).transform.position;
            Camera.main.GetComponent<MainCamara>().SetBound(targetMap);

            FadeOut();
            other.GetComponent<Animator>().enabled = true;
            other.GetComponent<Player>().enabled = true;

            StartCoroutine(area.GetComponent<Area>().ShowArea(targetMap.name));
        }
        

    }

    // Dibujaremos un cuadrado con opacidad encima de la pantalla simulando una transición
    void OnGUI()
    {

        // Si no empieza la transición salimos del evento directamente
        if (!start)
            return;

        // Si ha empezamos creamos un color con una opacidad inicial a 0
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);

        // Creamos una textura temporal para rellenar la pantalla
        Texture2D tex;
        tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.black);
        tex.Apply();

        // Dibujamos la textura sobre toda la pantalla
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);

        // Controlamos la transparencia
        if (isFadeIn)
        {
            // Si es la de aparecer le sumamos opacidad
            alpha = Mathf.Lerp(alpha, 1.1f, fadeTime * Time.deltaTime);
        }
        else
        {
            // Si es la de desaparecer le restamos opacidad
            alpha = Mathf.Lerp(alpha, -0.1f, fadeTime * Time.deltaTime);
            // Si la opacidad llega a 0 desactivamos la transición
            if (alpha < 0) start = false;
        }

    }

    // Método para activar la transición de entrada
    void FadeIn()
    {
        start = true;
        isFadeIn = true;
    }

    // Método para activar la transición de salida
    void FadeOut()
    {
        isFadeIn = false;
    }
}
