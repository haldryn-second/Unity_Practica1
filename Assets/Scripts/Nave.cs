using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nave : MonoBehaviour
{
    [SerializeField] float speed= 4;
    [SerializeField] float velocidadDisparo = 2;
    private Rigidbody2D rb2d;
    [SerializeField] Transform prefabDisparo;
    [SerializeField] Transform prefabExplosion;
    public Animator animator;
    private AudioSource[] audios;
    private AudioSource audio_disp;
    private AudioSource audio_expl;
    private float old_pos;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        old_pos = transform.position.y;
        audios = GetComponents<AudioSource>();
        audio_disp = audios[0];
        audio_expl = audios[1];
    }

    // Update is called once per frame
    void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        transform.position += move * speed * Time.deltaTime;


        if (transform.position.x <= -6.1f) {
            transform.position = new Vector3(-6.1f, transform.position.y,0);
        } else if (transform.position.x >= 6.1f) {
            transform.position = new Vector3(6.1f, transform.position.y,0);
        }
        

        if (transform.position.y <= -4.5f) {
            transform.position = new Vector3(transform.position.x, -4.5f,0);
        } else if (transform.position.y >= 4.5f) {
            transform.position = new Vector3(transform.position.x, 4.5f,0);
        }

        if(old_pos < transform.position.y){
            animator.SetBool("up", true);
            animator.SetBool("down", false);
        }else if(old_pos > transform.position.y){
            animator.SetBool("up", false);
            animator.SetBool("down", true);
        }else{
            animator.SetBool("up", false);
            animator.SetBool("down", false);
        }

        old_pos = transform.position.y;

        if (Input.GetButtonDown("Fire1"))
            {
                audio_disp.Play();
                Transform disparo = Instantiate(prefabDisparo, transform.position, Quaternion.identity);
                disparo.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(velocidadDisparo, 0, 0);
            }
    }

    //NAVE COLISIONANDO CON ENEMIGO
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Enemigo")||(other.tag == "Disparo_enemigo"))
        {
            audio_expl.Play();
            Destroy(other.gameObject);
            Destroy(gameObject,3f);
        }
    }

    void OnDestroy()
    {
        Transform explosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(explosion.gameObject, 1f);
    }
}
