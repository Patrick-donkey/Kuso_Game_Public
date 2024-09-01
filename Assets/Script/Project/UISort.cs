using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISort : MonoBehaviour
{
    [SerializeField]
    GameObject[] obj;
    [SerializeField]
    List<GameObject> uiObjects;
    [SerializeField]
    Sprite[] img;
    [SerializeField]
    Button[] bt;    
    [SerializeField]
    RectTransform[] bttr;    
    public Vector2 size = new Vector2(60, 80); 
    public Vector2 position = new Vector2(0,0);
    float x = 3;
    float y = 3;
    int cardi = 1;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<img.Length; i++)
        {
            bt[i].image.sprite = img[i];
        }
        for (int i = 0; i<bt.Length; i++)
        {
            bttr[i] = bt[i].GetComponent<RectTransform>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            uiObjects.Sort((x, y) => y.name.CompareTo(x.name));

               for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].transform.SetSiblingIndex(i);
            }
        }
    }
    public void bt1()
    {
        GameObject imageGO = new GameObject("Image"+cardi.ToString());
        uiObjects.Add(imageGO);
        RectTransform imageRectTransform = imageGO.AddComponent<RectTransform>();
        Image imageComponent = imageGO.AddComponent<Image>();

        imageRectTransform.SetParent(obj[0].transform);
        imageRectTransform.localScale = Vector3.one;
        imageRectTransform.localPosition = obj[0].transform.localPosition;

        imageRectTransform.sizeDelta = size;
        imageRectTransform.anchoredPosition = obj[1].transform.localPosition+new Vector3(x,y,0);
        x += 3;
        y += 3;
        cardi++;
        imageComponent.sprite = bt[1].image.sprite;
    }
}
