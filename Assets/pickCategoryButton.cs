using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class pickCategoryButton : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI categoryName;
    [SerializeField] private int buttonID;
    [SerializeField] List<string> boardsAbv = new List<string> { "a", "c", "w", "m", "cgl", "cm", "f", "n", "jp", "v", "vg", "vp", "vr", "co", "g", "tv", "k", "o", "an", "tg", "sp", "asp", "sci", "his", "int", "out", "toy", "i", "po", "p", "ck", "ic", "wg", "lit", "mu", "fa", "3", "gd", "diy", "wsg", "qst", "biz", "trv", "fit", "x", "adv", "lgbt", "mlp", "news", "wsr", "vip", "b", "r9k", "pol", "bant", "soc", "s4s", "s", "hc", "hm", "h", "e", "u", "d", "y", "t", "hr", "gif", "aco", "r" };

    public void setCategoryName(string newCategoryName)
    {
        //Set category name
        Debug.Log(newCategoryName);
        categoryName.text = newCategoryName;
    }
    public void setButtonID(int number)
    {
        //Set button ID
        buttonID = number;
    }

    public void pickCategoryClick()
    {
        //Starts the setup of the category wall that the user choosed
        Debug.Log("pickCategoryClick");
        GameObject.FindWithTag("svCatalogWall").GetComponent<svCatalog>().newGetCatalogData(boardsAbv[buttonID]);
    }

}
