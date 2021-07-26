using UnityEngine;
using UnityEngine.UI;

public class Dots : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject[] dots;
    public Color initialColor;
    public Color activeColor = new Color(113f, 41f, 0f, 1f);

    private void Start()
    {
        initialColor = dots[0].transform.Find("Knob").GetComponent<Image>().color;
        UpdateActiveDot(0);
    }

    public void UpdateActiveDot(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }

        for (int i = 0; i < dots.Length; i++)
        {
            if (i == index) dots[i].transform.Find("Knob").GetComponent<Image>().color = activeColor;
            else dots[i].transform.Find("Knob").GetComponent<Image>().color = initialColor;
        }
    }
}
