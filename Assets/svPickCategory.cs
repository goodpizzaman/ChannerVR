using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class svPickCategory : MonoBehaviour
{
    [SerializeField] private GameObject pickCategoryPF;
    [SerializeField] List<string> boardNames = new List<string> { "Anime & Manga", "Anime Cute", "Anime Wallpapers", "Mecha", "Cosplay & EGL", "Cute Male", "Flash", "Transportation", "Otaku Culture", "Video Games", "Video Game Generals", "Pokémon", "Retro Games", "Comics & Cartoons", "Technology", "Television & Film", "Weapons", "Auto", "Animals & Nature", "Traditional Games", "Sports", "Alternative Sports", "Science & Math", "History & Humanities", "International", "Outdoors", "Toys", "Oekaki", "Papercraft & Origami", "Photography", "Food & Cooking", "Artwork Critique", "Wallpapers General", "Literature", "Music", "Fashion", "3DCG", "Graphic Design", "Do It Yourself", "Worksafe GIF", "Quests", "Business & Finance", "Travel", "Fitness", "Paranormal", "Advice", "LGBT", "Pony", "Current News", "Worksafe Requests", "Very Important Posts", "Random", "ROBOT9001", "Politically Incorrect", "International Random", "Cams & Meetups", "Shit 4chan Says", "Sexy Beautiful Women", "Hardcore", "Handsome Men", "Hentai", "Ecchi", "Yuri", "Hentai Alternative", "Yaoi", "Torrents", "High Resolution", "Adult GIF", "Adult Cartoons", "Adult Requests" };
    
    // Start is called before the first frame update
    void Start()
    {
        //Makes pickCategory buttons
        for (int i = 0; i < boardNames.Count; i++)
        {

            GameObject newButton = Instantiate(pickCategoryPF);
            newButton.SetActive(true);
            newButton.transform.SetParent(pickCategoryPF.transform.parent, false);

            Debug.Log(boardNames[i]);
            newButton.GetComponent<pickCategoryButton>().setCategoryName(boardNames[i]);
            newButton.GetComponent<pickCategoryButton>().setButtonID(i);
        }

    }

}
