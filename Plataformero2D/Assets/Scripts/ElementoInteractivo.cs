using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//Uso de libreria para utilizar eventos de click y tactil en pantalla

public class ElementoInteractivo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler//heredo diferentes clases derivadas de evensystems
{
    public bool pulsado;

    public void OnPointerDown(PointerEventData eventData)//implementación de metodo cuando se pulsa
    {
        pulsado = true;
    }

    public void OnPointerUp(PointerEventData eventData)//implemnetación de metodo cuando se suelta
    {
        pulsado = false;
    }
}
