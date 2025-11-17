using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Para manejar textos en UI
using UnityEngine.SceneManagement;
using TMPro; // Importar TextMeshPro
using System.Linq; // Para funciones como OrderBy

public class QuestionManager : MonoBehaviour
{
    public TextMeshPro questionText;           // TextMeshPro para la pregunta
    public List<GameObject> answerCubes;       // Lista de cubos de respuestas
    public Color defaultColor = Color.yellow;   // Color inicial de los cubos
    public Color correctColor = Color.green;   // Color al seleccionar respuesta correcta
    public Color incorrectColor = Color.red;   // Color al seleccionar respuesta incorrecta

    private int currentQuestionIndex = 0;      // Índice de la pregunta actual
    private List<Question> questions = new List<Question>();

    /* Variables para el "TIMER" del Juego */
    private float countDownTimer = 1f; //Para ser mas preciso cuando disminuye el Timer
    [SerializeField] private GameObject timer; //Mostramos el tiempo en pantalla
    [SerializeField] private float timeRestant = 60f; //Tiempo que tendremos

    /* Variables para el "SCORE" del Juego */
    [SerializeField] private GameObject score;
    [SerializeField] private int scoreIndex = 0;
    [SerializeField] private int scoreMax = 5;

    /* Variables para el "AUDIO" del Juego */
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clipOptions;

    void Start()
    {
        // Crear preguntas y respuestas
        questions.Add(new Question("¿Cuántos éxodos tuvo Jujuy?", new[] { "4", "7", "1" }, 2));
        questions.Add(new Question("¿En que fecha la provincia de Jujuy declaró su autonomia política?", new[] { "18 de Noviembre", "7 de Mayo", "1 de Julio" }, 0));
        questions.Add(new Question("¿Cuantos departamentos posee Jujuy?", new[] { "19", "12", "16" }, 2));
        questions.Add(new Question("¿Quién es el gobernador actual de la provincia?", new[] { "Sadir", "Morales", "Camilo" }, 0));
        questions.Add(new Question("¿Con que otro nombre es conocido el 'Dia Grande de Jujuy'?", new[] { "Batalla de los Gauchos", "Gran Jujuy", "Batalla de León" }, 2));
        questions.Add(new Question("¿Cuál es la capital de Jujuy?", new[] { "Tilcara", "San Salvador", "Perico" }, 1));
        questions.Add(new Question("¿En qué año se realizó el Éxodo Jujeño?", new[] { "1810", "1812", "1816" }, 1));
        questions.Add(new Question("¿Qué famosa quebrada es Patrimonio de la Humanidad?", new[] { "Humahuaca", "Calilegua", "Tilcara" }, 0));
        questions.Add(new Question("¿Cuál es el plato típico elaborado con choclo, queso y envuelto en chala?", new[] { "Locro", "Humita", "Tamales" }, 1));
        questions.Add(new Question("¿Qué cerro en Jujuy es conocido por ser un...?", new[] { "Cerro Amarillo", "Cerro de los Siete Colores", "Cerro Blanco" }, 1));
        questions.Add(new Question("¿Qué carnaval tradicional es famoso en Jujuy?", new[] { "El Carnaval de la Quebrada", "El Carnaval de las flores", "Carnaval Andino" }, 0));
        questions.Add(new Question("¿Cuál es el nombre al famoso monumento de Humahuaca?", new[] { "Heroes de la Independencia", "Monumento al Indio", "Monumento al Carnaval" }, 0));
        questions.Add(new Question("¿Cuál es el animal más emblemático de la provincia?", new[] { "Puma", "Tucán", "Yaguareté" }, 2));
        questions.Add(new Question("¿Cuál de las siguientes leyendas pertenecen a Jujuy?", new[] { "El Familiar", "El Coquena", "Todas las anteriores" }, 2));
        questions.Add(new Question("¿Cuál es el nombre de la antigua fortaleza ubicada en la Quebrada?", new[] { "El Cabildo", "El Pucará de Tilcara", "Casabindo" }, 1));
        questions.Add(new Question("¿Qué festividad religiosa de gran arraigo en Jujuy consiste en venerar la tierra como madre?", new[] { "Ceremonia de la madre tierra", "Carnaval", "Ceremonia de la Pachamama" }, 2));
        questions.Add(new Question("¿Qué región geográfica de Jujuy alberga a especies como vicuñas, guanacos y llamas?", new[] { "Valle", "Quebrada", "Puna" }, 2));
        questions.Add(new Question("¿Qué personaje mítico, es travieso/a y que se dice que se lleva objetos, hace bromas y a veces asusta a los viajeros", new[] { "El Duende", "La Mulanima", "La Viuda" }, 0));


        // Barajar preguntas al inicio
        questions = questions.OrderBy(q => Random.value).ToList();

        // Mostrar la primera pregunta
        DisplayQuestion();
    }
    private void Update()
    {
        TimeForAnswer();
    }

    public void CheckAnswer(GameObject selectedCube)
    {
        // Identificar el índice del cubo seleccionado
        int selectedIndex = answerCubes.IndexOf(selectedCube);

        if (selectedIndex == questions[currentQuestionIndex].correctAnswerIndex)
        {
            // Respuesta correcta
            selectedCube.GetComponent<Renderer>().material.color = correctColor;
            audioSource.clip = clipOptions[0];
            audioSource.Play();
            Debug.Log("¡Correcto!");
            ScoreWin();
        }
        else
        {
            // Respuesta incorrecta
            selectedCube.GetComponent<Renderer>().material.color = incorrectColor;
            audioSource.clip = clipOptions[1];
            audioSource.Play();
            Debug.Log("Incorrecto");
        }

        // Cambiar de pregunta después de un breve retraso
        Invoke("NextQuestion", 2f);
    }

    void NextQuestion()
    {
        // Incrementar el índice de la pregunta actual
        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Count)
        {
            Debug.Log("¡Se han terminado las preguntas!");
            currentQuestionIndex = 0; // Reinicia el índice para comenzar de nuevo (opcional)

            // Vuelve a barajar las preguntas
            questions = questions.OrderBy(q => Random.value).ToList();
        }

        // Restablecer el color de los cubos
        foreach (var cube in answerCubes)
        {
            var renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = defaultColor;
            }
        }

        // Mostrar la siguiente pregunta
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        // Obtener la pregunta actual
        Question currentQuestion = questions[currentQuestionIndex];

        // Mostrar el texto de la pregunta
        questionText.text = currentQuestion.question;

        // Asignar respuestas a los cubos
        for (int i = 0; i < answerCubes.Count; i++)
        {
            TextMeshPro textMesh = answerCubes[i].GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = currentQuestion.answers[i];
            }
        }
    }

    void TimeForAnswer()
    {
        if (timer != null)
        {
            countDownTimer -= Time.deltaTime;
            if(countDownTimer <= 0f)
            {
                timeRestant -= 1f;
                timer.GetComponentInChildren<TextMeshPro>().text = timeRestant.ToString();
                countDownTimer = 1f;
            }
        }

        if(timeRestant < 0f)
        {
            Debug.Log("PERDISTEEE!!");
            SceneManager.LoadScene(3);
        }
    }

    void ScoreWin()
    {
        if(score != null)
        {
            scoreIndex++;
            score.GetComponentInChildren<TextMeshPro>().text = $"Score for  WIN: {scoreIndex.ToString()} / {scoreMax.ToString()}";

            if (scoreIndex == scoreMax)
            {
                Debug.Log("GANASTEEEEEE!!");
                SceneManager.LoadScene(2);
            }
        }
    }
}

[System.Serializable]
public class Question
{
    public string question;           // Texto de la pregunta
    public string[] answers;          // Opciones de respuesta
    public int correctAnswerIndex;    // Índice de la respuesta correcta

    public Question(string question, string[] answers, int correctAnswerIndex)
    {
        this.question = question;
        this.answers = answers;
        this.correctAnswerIndex = correctAnswerIndex;
    }
}
