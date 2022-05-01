using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jugador : MonoBehaviour
{
    public float vel;
    Rigidbody2D rb;
    public float saltar;

    public Transform checkSuelo;
    public bool ensuelo;
    public LayerMask layer;
    public bool DobleSalto;
    float h;
    Animator anim;
    private string currentAnimaton;

    const string PLAYER_IDLE = "atacke1";
    const string PLAYER_DE = "idle";
    const string PLAYER_CAYENDO = "oayendo";
    const string PLAYER_SALTADOS = "saltodos";
    const string PLAYER_SALTAR = "saltar";

    public bool atcakando;
    public float fireRate = 0.5f;
    public float nextFire = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        salto();
        if (ensuelo)
        {
            DobleSalto = true;
            
        }
    }

    private void FixedUpdate()
    {
        moviminto();
        ensuelo = Physics2D.OverlapCircle(checkSuelo.position, 0.1f, layer);
        atacan();
        cayendo();
    }

    // inicio movimiento
    void moviminto()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.position += new Vector3(vel * Time.deltaTime, 0.0f, 0.0f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            this.transform.position += new Vector3(-vel * Time.deltaTime, 0.0f, 0.0f);
        }

        // animaciones
        h = Input.GetAxisRaw("Horizontal");
        if (h > 0 && ensuelo)
        {
            anim.SetBool("correr", true);
        }
        else if (h < 0 && ensuelo)
        {
            anim.SetBool("correr", true);
        }
        else
        {
            
            anim.SetBool("correr", false);
        }
    }
    // final movimiento

    // inicio salto
    void salto()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (ensuelo)
            {
               // anim.SetTrigger("saltar");
                rb.AddForce(Vector2.up * saltar);
            }
            else if (DobleSalto)
            {
                //anim.SetTrigger("saltodos");
                rb.velocity = Vector2.zero;
                rb.AddForce(Vector2.up * saltar);
                DobleSalto = false;
            }

          
        }
    }
    // final salto
    void detectarCayendo()
    {
        anim.SetBool("tocandoSuelo", false);
    }
    //cayendo
    void cayendo()
    {
        if(rb.velocity.y < 1 && !ensuelo)
        {
            Debug.Log("cayendo");
            ChangeAnimationState(PLAYER_CAYENDO);
        }
        else if(rb.velocity.y > 1 && !ensuelo){
            if (DobleSalto)
            {
                ChangeAnimationState(PLAYER_SALTAR);
                Debug.Log("subiendo");
            }
            else if (!DobleSalto)
            {
                ChangeAnimationState(PLAYER_SALTADOS);
                Debug.Log("subiendodos");
            }
           
        }
        else if (ensuelo && DobleSalto)
        {
         
         ChangeAnimationState(PLAYER_DE);
        }
    }


    // atacando con time delta time
    void atacan()
    {
        atcakando = true;
        if(nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.J) && nextFire <= 0)
        {
            rb.velocity = Vector2.zero;
       ChangeAnimationState(PLAYER_IDLE);
            nextFire = fireRate;
            
        }

       
        

        
       
    }
    // final atacando
    // volver a  idle
    void detenerse()
    {
        //poner al final de la animaciona atacke1
        atcakando = false;
        if (!atcakando)
        {
          ChangeAnimationState(PLAYER_DE);
        }
        if (!atcakando)
        {
            atcakando = true;
        }
        
       
    }
    // final volve idle








    // mini controlado  animaciones
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        anim.Play(newAnimation);
        currentAnimaton = newAnimation;
    }






    //final
}
