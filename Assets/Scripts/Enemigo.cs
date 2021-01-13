using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private float velocidadX = -0.2f;
    private float velocidadY = -5f;
    [SerializeField] Transform prefabDisparo;
    [SerializeField] Transform prefabExplosion;
    [SerializeField] float velocidadDisparo = 3;
    private AudioSource[] audios;
    private AudioSource audio_disp;
    private AudioSource audio_expl;
    //private bool disparoListo;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(rutinaDisparo());
        audios = GetComponents<AudioSource>();
        audio_disp=audios[0];
        audio_expl=audios[1];
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocidadX * Time.deltaTime, velocidadY * Time.deltaTime, 0);
        if ((transform.position.y < -4.5) || (transform.position.y > 4.5))
            velocidadY = -velocidadY;

        //if (disparoListo) StartCoroutine(rutinaDisparo());
    }

    IEnumerator rutinaDisparo(){
        //disparoListo=false;
        yield return new WaitForSeconds(Random.Range(0f, 3f));
        audio_disp.Play();
        Transform disparo = Instantiate(prefabDisparo, transform.position, Quaternion.identity);
        disparo.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-velocidadDisparo, 0, 0); 
        StartCoroutine(rutinaDisparo());
        //disparoListo=true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Nave" ) || (other.tag == "Disparo_aliado"))
        {
            audio_disp.Play();
            Destroy(other.gameObject);
            gameObject.SetActive(false);
            Destroy(gameObject,3f);
        }
    }

	void OnDestroy()
    {
        //AudioSource.PlayClipAtPoint(audio_expl, transform.position);
        Transform explosion = Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        Destroy(explosion.gameObject, 1f);
    }
}