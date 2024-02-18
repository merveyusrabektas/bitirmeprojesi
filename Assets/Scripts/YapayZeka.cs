using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class YapayZeka : MonoBehaviour
{
    Transform oyuncu;
    public NavMeshAgent dusman;
    public Animator animasyon;
    public GameObject sandik;
    public AudioClip top_sesi;
    public AudioClip top_temas_sesi;
    public AudioClip havaya_ucma_sesi;
    public AudioSource ses;
    public GameObject gulle_temas_efekti;
    public GameObject ates_etme_alevi_efekti;
    public GameObject havaya_ucma_efekti;
    public GameObject gulle;
    public Transform[] toplar;
    public float can = 100;
    float top_vurus_gucu = 10.0f;
    bool isik_temas = false;
    bool sag_isik = false;
    bool sol_isik = false;

    void Start()
    {
        oyuncu = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("TakipEt", 0f, 0.5f); 
    }

    void Update()
    {
        Isik();
        Ates();
    }

    void TakipEt()
    {
        if (can > 0)
        {
            float mesafe = Vector3.Distance(transform.position, oyuncu.position);

            if (mesafe > 50 && !isik_temas)
            {
                dusman.destination = oyuncu.position;
            }
            else if (mesafe < 50 && !isik_temas)
            {
                transform.Rotate(0, -10 * Time.deltaTime, 0);
            }
        }
    }

    void Isik()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.right * 50, Color.red);

        if (Physics.Raycast(transform.position, transform.right, out hit, 50))
        {
            if (hit.collider.gameObject.tag == "Player" && !isik_temas)
            {
                sag_isik = true;
                sol_isik = false;
                isik_temas = true;
                InvokeRepeating("Ates", 0, 3f);
            }
        }
        else if (Physics.Raycast(transform.position, -transform.right, out hit, 50))
        {
            if (hit.collider.gameObject.tag == "Player" && !isik_temas)
            {
                sag_isik = false;
                sol_isik = true;
                isik_temas = true;
                InvokeRepeating("Ates", 0, 3f);
            }
        }
        else
        {
            sag_isik = false;
            sol_isik = false;
            isik_temas = false;
            CancelInvoke("Ates");
        }
    }

    void Ates()
    {
        int baslangicIndeksi = sag_isik ? 0 : 8;
        int bitisIndeksi = sag_isik ? 7 : 15;

        for (int i = baslangicIndeksi; i <= bitisIndeksi; i++)
        {
            StartCoroutine(Firlat(i));
        }
    }

    IEnumerator Firlat(int deger)
    {
        float zaman = Random.Range(0.1f, 1.0f);
        yield return new WaitForSeconds(zaman);

        GameObject yeni_top = Instantiate(gulle, toplar[deger].position, Quaternion.identity);
        Destroy(yeni_top, 5f);

        yeni_top.GetComponent<Rigidbody>().velocity = toplar[deger].forward * 30;

        GameObject patlama_partikul = Instantiate(ates_etme_alevi_efekti, yeni_top.transform.position, Quaternion.identity);
        Destroy(patlama_partikul.gameObject, 1.5f);

        ses.PlayOneShot(top_sesi, 0.3f);
    }

    void OnTriggerEnter(Collider nesne)
    {
        if (nesne.gameObject.CompareTag("Gulle"))
        {
            Destroy(nesne.gameObject);
            can -= top_vurus_gucu;
            ses.PlayOneShot(top_temas_sesi, 0.5f);

            GameObject patlama_partikul = Instantiate(gulle_temas_efekti, nesne.gameObject.transform.position, Quaternion.identity);
            Destroy(patlama_partikul, 1.5f);

            if (can <= 0)
            {
                GetComponent<BoxCollider>().enabled = false;
                can = 0;
                ses.PlayOneShot(havaya_ucma_sesi);

                GameObject yeni_havaya_ucma_efekti = Instantiate(havaya_ucma_efekti, transform.position, Quaternion.identity);
                Destroy(yeni_havaya_ucma_efekti, 3.90f);

                GameObject yeni_sandik = Instantiate(sandik, transform.position, Quaternion.Euler(-90, 0, 0));
                yeni_sandik.GetComponent<Rigidbody>().AddTorque(Vector3.up * 70.0f);

                Olum();
            }
        }
    }

    void Olum()
    {
        CancelInvoke();
        dusman.enabled = false;
        animasyon.enabled = false;
        GetComponent<Rigidbody>().velocity = Vector3.down * 2.0f;
        GetComponent<Rigidbody>().AddTorque(Vector3.left * 100.0f);
        Destroy(gameObject, 20f);
    }
}