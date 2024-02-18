using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class Lvl_kontrol : MonoBehaviour
{
    public int level;
    public TextMeshProUGUI lvl_txt;
    public GameObject dusman_gemisi;
    public Transform[] dogma_koordinatlari;
    public float dusman_cani = 0;
    public float toplam_dusman_cani = 0;
    public Image canban;

    private void Awake()
    {
        LevelKontrol();
        DusmanGemisiEkle();
    }

    void LevelKontrol()
    {
        if (PlayerPrefs.HasKey("lvl"))
        {
            level = PlayerPrefs.GetInt("lvl");
        }
        else
        {
            level = 1;
            PlayerPrefs.SetInt("lvl", level);
        }

        lvl_txt.text = "LEVEL: " + level;
    }

    void DusmanGemisiEkle()
    {
        for (int i = 0; i < level; i++)
        {
            int rastgele = Random.Range(0, dogma_koordinatlari.Length);
            Vector3 yeni_pozisyon = dogma_koordinatlari[rastgele].position;
            yeni_pozisyon.z += i * 30;
            GameObject yeni_dusman_gemisi = Instantiate(dusman_gemisi, yeni_pozisyon, Quaternion.identity);
            YapayZeka yapayZekaComponent = yeni_dusman_gemisi.GetComponent<YapayZeka>();
            if(yapayZekaComponent != null)
            {
                dusman_cani += yapayZekaComponent.can;
                toplam_dusman_cani = dusman_cani;
                canban.fillAmount = dusman_cani / toplam_dusman_cani;
            }
        }
    }

    public void CanAzalt(float miktar)
    {
        dusman_cani -= miktar;
        canban.fillAmount = dusman_cani / toplam_dusman_cani;

        if (dusman_cani <= 0)
        {
            Invoke("DigerLevel", 10.0f);
        }
    }

    void DigerLevel()
    {
        level++;
        PlayerPrefs.SetInt("lvl", level);
        SceneManager.LoadScene("SampleScene");
    }
}
