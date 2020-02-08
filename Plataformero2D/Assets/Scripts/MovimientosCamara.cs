using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientosCamara : MonoBehaviour
{
    //Variables para indicar el movimiento de la camara

    public bool vertical;
    public bool horizontal;
    public bool zoomOut;


    //variables para indicar el valor del movimiento de la camara

    public float valorVertical;
    public float valorHorizontal;
    public float valorZoomOut;

    public CinemachineVirtualCamera camaraVirtual;//Referencia a la camara virtual del cinemachine

    public float tiempo;//variable para indicar el tiempo en el que se va a mover la camara


    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Si hay un tiempo mayo a 0, si hay un boolenao activo y su valor es mayor a 0 (Llamar metodo y restablecer valores en un timepo indicado)
        if(tiempo > 0)
        {
            if (vertical && valorVertical > 0 )
            {
                moverVerticalmente();
                Invoke("restablecer", tiempo);
            }

            if (horizontal && valorHorizontal > 0)
            {
                moverHorizontalmente();
                Invoke("restablecer", tiempo);
            }

            if (zoomOut && valorZoomOut > 0)
            {
                zoomAfuera();
                Invoke("restablecer", tiempo);
            }

        }

           
    }


    //Metodo para modificar el valor de Y en la camara
    void moverVerticalmente()
    {
        var scriptPantalla = camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();
        scriptPantalla.m_ScreenY = valorVertical;
    }

    //Metodo para modificar el valor de X en la camara
    void moverHorizontalmente()
    {
        var scriptPantalla = camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();
        scriptPantalla.m_ScreenX = valorHorizontal;
    }

    //Metodo para modificar el valor de CameraDistance en la camara
    void zoomAfuera()
    {
        var scriptPantalla = camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();
        scriptPantalla.m_CameraDistance = valorZoomOut;
    }

    //Metodo para restablecer los valores iniciales
    void restablecer()
    {
        var scriptPantalla = camaraVirtual.GetCinemachineComponent<CinemachineFramingTransposer>();
        scriptPantalla.m_ScreenY = 0.5f;
        scriptPantalla.m_ScreenX = 0.5f;
        scriptPantalla.m_CameraDistance = 9.4f;
    }

    

}
