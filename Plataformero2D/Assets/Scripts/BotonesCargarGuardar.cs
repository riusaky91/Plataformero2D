using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotonesCargarGuardar : MonoBehaviour
{

    public Estado estado;

    public static bool cargar;



    public void Guardar()
    {
        
        
        GestorDeEstado.Guardar(estado);
    }

    public void Cargar()
    {
        
        GestorDeEstado.Cargar(estado);
    }
}
