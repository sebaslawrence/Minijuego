using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Data;


[RequireComponent(typeof(AudioSource))] //Audio para quiz
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gamerOverScreen;
    [SerializeField] private GameObject winScreen;
    public BlinkColor blinkColorScriptred;
    public BlinkColor blinkColorScriptgreen;

    [SerializeField] private GameObject quizScreen;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float initialScrollSpeed;
    [SerializeField] private AudioClip m_perderVida = null;

    private int score;
    private float timer;
    private float scrollSpeed;
    private int scorePerSeconds = 5;


    private int vidascount = 1;
    public static GameManager Instance {  get; private set; }
    public HUD hud;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        Time.timeScale = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        UpdateSpeed();
    }

    public void ShowGameOverScreen() //activate gameover screen
    {
        gamerOverScreen.SetActive(true);
    }

    public void ShowWinScreen() //activate win screen
    {
        winScreen.SetActive(true);
    }

    public void RestartScene()  //restart game
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f; 
    }

    private void UpdateScore() //update game score
    {
        timer += Time.deltaTime;
        score = (int)(timer * scorePerSeconds);
        scoreText.text = string.Format("{0:00000}", score);
        
        if (score >= 10000000)
        {
            Time.timeScale = 0f;
            ShowWinScreen();
        }
    }

    private void UpdateSpeed() //speed increaser
    {
        float speedDivider = 25;
        scrollSpeed = initialScrollSpeed + timer / speedDivider;
    }

    public void PausarJuego()
    {
        StartCoroutine(PausaTemporal());
    }

    IEnumerator PausaTemporal()
    {
        Time.timeScale = 0f; // Detiene el tiempo del juego
        yield return new WaitForSecondsRealtime(0.5f); // Espera medio segundo en tiempo real
        Time.timeScale = 1f; // Reanuda el tiempo del juego
    }

    private bool vidaRestada = false;

    IEnumerator ReactivarVidaRestada()
    {
        yield return new WaitForSecondsRealtime(1f); // Espera un segundo en tiempo real
        vidaRestada = false; // Permite que se reste otra vida
    }

    public void PerderVida()
    {
        if (!vidaRestada)
        {
            vidaRestada = true; // Evita que se resten más vidas

            if (vidascount > 1)
            {
                StartCoroutine(PausaTemporal()); // Pausa el juego si hay más de una vida
            }

            vidascount -= 1; // Resta una vida
            hud.DesactivarVida(vidascount); // Actualiza el HUD

            StartCoroutine(ReactivarVidaRestada()); // Permite restar otra vida después de un tiempo
        }
    }

    public void RecuperarVida()
    {
        if (vidascount < 3)
        {
            hud.ActivarVida(vidascount);
            vidascount += 1;
        }
    }

    public int getVidas()
    {
        return vidascount;
    }

    public float GetScrollSpeed()
    {
        return scrollSpeed;
    }


    /// Quiz Part
    [SerializeField] private AudioClip m_correctSound = null;
    [SerializeField] private AudioClip m_incorrectSound = null;

    private Q_DataBase m_quizDB = null;
    private QuizUI m_quizUI = null;
    private AudioSource m_audioSource = null;

    public void StartQuiz()
    {
        m_quizDB = GameObject.FindObjectOfType<Q_DataBase>();
        m_quizUI = GameObject.FindObjectOfType<QuizUI>();
        m_audioSource = GetComponent<AudioSource>();

        NextQuestion();
    }

    private void NextQuestion()
    {
        m_quizUI.Construct(m_quizDB.GetRandom(), GiveAnswer);
    }

    private void GiveAnswer(OptionsButton optionButton)
    {
        if (m_audioSource.isPlaying)
            m_audioSource.Stop();

        // Establecer el clip de audio según la opción seleccionada
        m_audioSource.clip = optionButton.Option.correct ? m_correctSound : m_incorrectSound;

        // Reproducir el sonido
        m_audioSource.Play();

        if (optionButton.Option.correct)
        {
            StartCoroutine(blinkColorScriptgreen.Blink());
            RecuperarVida();
            Time.timeScale = 1f;
            quizScreen.SetActive(false);
            PausarJuego();
            scorePerSeconds = scorePerSeconds*2;
        }

        else
        {
            StartCoroutine(blinkColorScriptred.Blink());
            PerderVida();

            if (vidascount == 0)
            {
                quizScreen.SetActive(false);
                ShowGameOverScreen();
            }
            else
            {
                Time.timeScale = 1f;
                quizScreen.SetActive(false);
                PausarJuego();
            }
        }
    }


    public void sonidoPerderVida()
    {
        m_audioSource = GetComponent<AudioSource>();
        if (m_audioSource.isPlaying)
            m_audioSource.Stop();
        //Establecer sonido
        m_audioSource.clip = m_perderVida;
        // Reproducir el sonido
        m_audioSource.Play();
    }

    public void ShowQuizScreen() //gameover
    {
        quizScreen.SetActive(true);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(0);
    }

}
