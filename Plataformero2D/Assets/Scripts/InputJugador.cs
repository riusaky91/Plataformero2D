using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 LISTA DE VESIONES
1. Metodo para priorizar script, variables inputs, metodo limpiarInput, procesamientoInputs, variable listoPartaLimpiar.

*/


//Para ejecutar este componente primero que los demas
//inputs
[DefaultExecutionOrder(-100)]

public class InputJugador : MonoBehaviour
{

    [HideInInspector] public float horizontal;      //Horizontal
    [HideInInspector] public bool mantenerSalto;         //Bool mantener pulsado saltar
    [HideInInspector] public bool saltar;      //Bool pulsado saltar
    [HideInInspector] public bool mantenerAgachado;       //Bool mantener pulsado agachar
    [HideInInspector] public bool agacharse;	//Bool pulsado agachar


    bool listoPartaLimpiar; //resetea los valores booleanos al ya ser utilizados

    private void Update()
    {
        limpiarInput();//Reinicia el estado de los Inputs

        procesamientoInputs();

        horizontal = Mathf.Clamp(horizontal, -1f, 1f);//Pongo limite a los valores de horizontal
    }

    private void FixedUpdate()
    {
        listoPartaLimpiar = true;
    }


    private void limpiarInput()
    {
        if (listoPartaLimpiar)
        {
            return;
        }

        //Reinicio datos

        horizontal = 0f;      
        mantenerSalto = false;         
        saltar = false;     
        mantenerAgachado = false;       
        agacharse = false;

        listoPartaLimpiar = false;

    }

    //Este metodo se realiza para no perder las teclas oprimidas en el update de movimientoJugador

    private void procesamientoInputs()
    {
        horizontal += Input.GetAxis("Horizontal");//Acumulo el eje horizontal

        //Acumulando inputs

        saltar = saltar || Input.GetButtonDown("Jump");
        mantenerSalto = mantenerSalto || Input.GetButton("Jump");

        agacharse = agacharse || Input.GetButtonDown("Crouch");
        mantenerAgachado = mantenerAgachado || Input.GetButton("Crouch");
    }
}
