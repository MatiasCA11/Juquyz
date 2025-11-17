using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSelector : MonoBehaviour
{
    public Camera playerCamera;      // Cámara que emite el Raycast
    public float rayDistance = 10f;  // Distancia máxima del Raycast
    public float gazeTime = 2f;      // Tiempo necesario para confirmar selección

    private float gazeTimer = 0f;    // Temporizador de enfoque
    private Transform targetObject = null; // Objeto actualmente bajo el Raycast

    void Update()
    {
        // Crear el Raycast desde la cámara
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Dibujar el Raycast en la escena (para depuración)
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            //Debug.Log("Raycast golpeó: " + hit.transform.name); // Verificar qué objeto detectó

            // Verificar si es un objeto de respuesta
            if (hit.transform.CompareTag("Answer"))
            {
                if (targetObject != hit.transform)
                {
                    // Cambiar el objetivo y reiniciar el temporizador
                    gazeTimer = 0f;
                    targetObject = hit.transform;
                }

                // Incrementar el temporizador
                gazeTimer += Time.deltaTime;

                // Confirmar selección si se mira por suficiente tiempo
                if (gazeTimer >= gazeTime)
                {
                    QuestionManager manager = FindObjectOfType<QuestionManager>();
                    if (manager != null)
                    {
                        manager.CheckAnswer(hit.transform.gameObject);
                    }

                    // Reiniciar el temporizador para evitar múltiples selecciones
                    ResetGaze();
                }
            }
            else
            {
                ResetGaze(); // Reiniciar si el objeto no es válido
            }
        }
        else
        {
            ResetGaze(); // Reiniciar si el Raycast no golpea nada
        }
    }

    private void ResetGaze()
    {
        gazeTimer = 0f;
        targetObject = null;
    }
}
