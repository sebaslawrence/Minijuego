using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : MonoBehaviour
{
    [SerializeField] private float upForce;
    [SerializeField] private Transform groundCheck; //posicion dentro del circulo
    [SerializeField] private LayerMask ground; //capa del suelo
    [SerializeField] private float radius; //radio circum
    [SerializeField] private Collider2D colliderAgachado; //nuevo collider al agacharse
    [SerializeField] private Collider2D colliderGeneral; // collider general

    private Rigidbody2D dinoRb;
    private Animator dinoAnimator;
    private int vidascount;
    public BlinkColor blinkColorScriptHit;


    // Start is called before the first frame update
    void Start()
    {
        dinoRb = GetComponent<Rigidbody2D>();
        dinoAnimator = GetComponent<Animator>();
        colliderGeneral.enabled = true;
        colliderAgachado.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool crouch = false;
        colliderGeneral.enabled = true;
        colliderAgachado.enabled = false;
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, ground); //crea circulo para checkear si toca suelo
        dinoAnimator.SetBool("isGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.W)) //salto
        {
            if (isGrounded)
            {
                dinoRb.AddForce(Vector2.up * upForce);
            }
        }

        if (Input.GetKey(KeyCode.S)) //agacharse
        {
            if (isGrounded)
            {
                crouch = true;
                colliderAgachado.enabled = true;
                colliderGeneral.enabled = false;
            }
        }

        dinoAnimator.SetBool("Agachado", crouch);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))  //colision con obstáculo
        {

            StartCoroutine(blinkColorScriptHit.Blink());
            GameManager.Instance.sonidoPerderVida();
            GameManager.Instance.PerderVida();
            vidascount = GameManager.Instance.getVidas();
            

            if (vidascount == 0)
            {
                GameManager.Instance.ShowGameOverScreen();
                dinoAnimator.SetTrigger("Die");
                Time.timeScale = 0f; //parar tiempo
            }

        }

        if (collision.gameObject.CompareTag("Life"))  //colision con vidas
        {
            Time.timeScale = 0f;                       //parar tiempo
            Destroy(collision.gameObject.gameObject);  //destruir vida recogida
            GameManager.Instance.ShowQuizScreen();     //Mostrar quiz
            GameManager.Instance.StartQuiz();          //Iniciar Quiz
        }
    }
}
