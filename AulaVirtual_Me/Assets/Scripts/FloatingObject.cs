using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatAmplitude = 0.5f;  // Altura máxima de flotación
    public float floatSpeed = 1f;       // Velocidad de flotación

    private Vector3 startPosition;

    void Start()
    {
        // Guardar la posición inicial del objeto
        startPosition = transform.position;
    }

    void Update()
    {
        // Calcular el desplazamiento vertical usando una función senoidal
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;

        // Actualizar la posición del objeto
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
