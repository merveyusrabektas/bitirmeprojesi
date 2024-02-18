using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gemi_kontrol : MonoBehaviour
{
    public Animator anim;
    public Camera kamera;
    public TMPro.TextMeshProUGUI altin_txt;
    public Light gunes;
    public Image canbar;
    public Image gullebar;
    public Transform[] toplar;

    public GameObject gulle;
    public GameObject ates_etme_alevi_efekti;
    public GameObject gulle_temas_efekti;
    public GameObject havaya_ucma_efekti;
    public AudioClip top_sesi;
    public AudioClip top_temas_sesi;
    public AudioClip sandik_alma_sesi;
    public AudioSource ses;

    int hiz = 8;
    bool sag_ok = false;
    bool sol_ok = false;
    float can = 100;
    float toplam_can = 100;
    float dusman_top_vurus_gucu = 1.0f;
    float gulle_miktari = 8;
    float toplam_gulle = 8;
    int altin = 250;

    void Update()
    {
        hareket();
        parlaklik();
        top_doldur();
        ates_et();
        oyundan_cik();
    }

    void hareket()
    {
        float ileri = Input.GetAxis("Vertical") * hiz * Time.deltaTime;
        float don = Input.GetAxis("Horizontal") * hiz * Time.deltaTime;

        transform.Translate(0, 0, ileri);
        transform.Rotate(0, don, 0);

        if (ileri != 0)
        {
            anim.SetBool("hareket", true);
        }
        else
        {
            anim.SetBool("hareket", false);
        }
    }

    void parlaklik()
    {
        if (gunes.intensity > 1)
        {
            gunes.intensity -= 0.3f;
        }
    }

    void top_doldur()
    {
        if (gulle_miktari < 8)
        {
            gulle_miktari += Time.deltaTime;
            gullebar.fillAmount = gulle_miktari / toplam_gulle;

            if (gulle_miktari >= 8)
            {
                gullebar.gameObject.SetActive(false);
                gulle_miktari = 8;
            }
        }
    }

    void ates_et()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gulle_miktari == 8)
            {
                StartCoroutine(firlat(0));
                gulle_miktari = 0;
                gullebar.gameObject.SetActive(true);
                gullebar.fillAmount = gulle_miktari / toplam_gulle;
            }
        }
    }

    IEnumerator firlat(int deger)
    {
        float zaman = Random.Range(0.1f, 1.0f);
        yield return new WaitForSeconds(zaman);

        GameObject yeni_gulle = Instantiate(gulle, toplar[deger].position, Quaternion.identity);
        yeni_gulle.GetComponent<Rigidbody>().velocity = toplar[deger].forward * 30;
        Destroy(yeni_gulle, 5f);

        GameObject patlama_partikul = Instantiate(ates_etme_alevi_efekti, yeni_gulle.transform.position, Quaternion.identity);
        Destroy(patlama_partikul.gameObject, 1.5f);

        ses.PlayOneShot(top_sesi, 0.3f);
    }

    void oyundan_cik()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void sag_btn()
    {
        sag_ok = !sag_ok;
    }

    public void sol_btn()
    {
        sol_ok = !sol_ok;
    }
}
