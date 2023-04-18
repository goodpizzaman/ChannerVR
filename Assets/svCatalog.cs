using System.Collections;
using System.Collections.Generic;
using OVRSimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Meta.WitAi.Json;
using Unity.VisualScripting.FullSerializer;
using System.Text.RegularExpressions;
using TMPro;

public class svCatalog : MonoBehaviour
{
    [SerializeField] private GameObject buttonPF;
    [SerializeField] private TextMeshProUGUI catalogMessage;
    [SerializeField] private Button threadSlot1Button;
    [SerializeField] private Button threadSlot2Button;
    [SerializeField] private Button threadSlot3Button;
    [SerializeField] private Button threadSlot4Button;
    [SerializeField] private Button threadSlot5Button;
    [SerializeField] private Button threadSlot6Button;

    [SerializeField] string currentBoard;
    [SerializeField] int tempThreadNumber;

    [SerializeField] List<string> subjectComplete;
    [SerializeField] List<string> commentComplete;
    [SerializeField] List<string> imageComplete;
    [SerializeField] List<int> catalogThreadNumberList;

    [SerializeField] public List<int> threadList;

    // Start is called before the first frame update
    void Start()
    {
        //Add listeners to the threadSlot buttons, sets thread slot with the the thread of the catalog button that the user choosed
        threadSlot1Button.onClick.AddListener(chooseThreadSlot1);
        threadSlot2Button.onClick.AddListener(chooseThreadSlot2);
        threadSlot3Button.onClick.AddListener(chooseThreadSlot3);
        threadSlot4Button.onClick.AddListener(chooseThreadSlot4);
        threadSlot5Button.onClick.AddListener(chooseThreadSlot5);
        threadSlot6Button.onClick.AddListener(chooseThreadSlot6);
        //Turns off the thread slot message and the thread slot messages
        disbalethreadSlotButtonsResetCatalogMessage();

        //TEST - set board and thread for threadslot1 on startup
        //GameObject.FindWithTag("svThreadOverlay1").GetComponent<svThreadOverlay>().startThreadData("g", "0000");

    }

    public void newGetCatalogData(string boardAbv)
    {
        //Start getting the category data of all the threads currently 
        currentBoard = boardAbv;

        catalogMessage.text = "LOADING...";

        //Clear the catalog buttons if they are any
        Debug.Log("newGetCatalogData");
        if (subjectComplete.Count > 0)
        {
            ClearSVCatalogButtons();
        }

        //Clear catalog data
        subjectComplete.Clear();
        commentComplete.Clear();
        imageComplete.Clear();
        catalogThreadNumberList.Clear();

        //Start getting new catalog data
        StartCoroutine(getCatalogData(boardAbv));
    }

    public IEnumerator getCatalogData(string boardAbv)
    {
        //Start the process of getting the catalog data from the JSON of the category
        Debug.Log("getCatalogData");
        //https://a.4cdn.org/\(boardAbv)/\(page).json

        for (int p = 1; p <= 10; p++)
        {
            // get catalog page data
            //Debug.Log(p);
            var url = "https://a.4cdn.org/" + (boardAbv) + "/" + p + ".json";
            //Debug.Log("=========" + url);
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + request.error);
                }
                else
                {
                    Debug.Log("Received: " + request.downloadHandler.text);
                    var json = request.downloadHandler.text;
                    CatalogWall jsonThread = JsonConvert.DeserializeObject<CatalogWall>(json);
                    //Debug.Log(jsonThread.threads[0].posts[0].tim);

                    var threadCount = jsonThread.threads.Count;
                    //Debug.Log(threadCount);
                    Debug.Log("=========" + url);

                    //get thread data organized
                    for (int t = 0; t < threadCount; t++)
                    {
                        //Debug.Log("----------" + t);
                        var post = jsonThread.threads[t].posts[0];
                        
                        //image
                        var imageName = post.tim + "s.jpg";
                        if (post.ext == ".webm")
                        {
                            imageName = post.tim + "s.jpg";
                        }
                        //Debug.Log(imageName);
                        var imageURLComplete = "https://i.4cdn.org/" + boardAbv + "/" + imageName;
                        imageComplete.Add(imageURLComplete);
                        //Debug.Log(imageURLComplete);

                        //subject
                        var subject = post.sub;
                        if (subject != null)
                        {
                            subject = removeHTMLFromString(subject);
                        }
                        subjectComplete.Add(subject);
                        //Debug.Log(subject);

                        //comment
                        var comment = post.com;
                        if(comment != null)
                        {
                            comment = removeHTMLFromString(comment);
                        }
                        commentComplete.Add(comment);
                        //Debug.Log(comment);

                        //threadNumber
                        catalogThreadNumberList.Add(post.no);
                        //Debug.Log(post.no);

                    }

                    if (p == 10)
                    {
                        //At JSON page 10 setup the category buttons
                        setupCategoryCatalogButtons();
                    }
                }

            }
        }
    }

    public static string removeHTMLFromString(string value)
    {
        //Remove HTML tags from the subject and comment
        var step1 = Regex.Replace(value, @"<[^>]+>", "").Trim();
        var step2 = Regex.Replace(step1, @"&nbsp;", " ");
        var step3 = Regex.Replace(step2, @"&quot;", " ");
        var step4 = Regex.Replace(step3, @"&gt;", " ");
        var step5 = Regex.Replace(step4, @"&amp;", " ");
        var step6 = Regex.Replace(step5, @"&#039;", "'");
        var finalStep = Regex.Replace(step6, @"\s{2,}", " ");
        return finalStep;
    }

    void setupCategoryCatalogButtons()
    {
        //Setup the catalog buttons from the JSON data and lists
        Debug.Log("setupCategoryCatalogButtons");

        //Make buttons
        for (int i = 0; i < commentComplete.Count; i++)
        {

            GameObject newButton = Instantiate(buttonPF);
            newButton.SetActive(true);
            newButton.transform.SetParent(buttonPF.transform.parent, false);

            newButton.GetComponent<catalogButton>().setImage(imageComplete[i]);
            newButton.GetComponent<catalogButton>().setSubject(subjectComplete[i]);
            //Debug.Log(subjectComplete[i]);
            newButton.GetComponent<catalogButton>().setComment(commentComplete[i]);
            newButton.GetComponent<catalogButton>().setThreadNumber(catalogThreadNumberList[i]);
            //Debug.Log(catalogThreadNumberList[i]);
            newButton.tag = "catalogButton";
        }

        catalogMessage.text = "";

    }

    public void addThreadToThreadList(int threadNumber)
    {
        //Ask the user which thread slot is going to be used for which thread slot
        //Future feature - add thread to a thread/watch list
        Debug.Log("addThreadToThreadList");
        //threadList.Add(threadNumber);
        Debug.Log(currentBoard);
        Debug.Log(threadNumber);
        tempThreadNumber = threadNumber;

        //User needs to choose thread slot
        catalogMessage.text = "Which thread slot to set it to?";
        threadSlot1Button.gameObject.SetActive(true);
        threadSlot2Button.gameObject.SetActive(true);
        threadSlot3Button.gameObject.SetActive(true);
        threadSlot4Button.gameObject.SetActive(true);
        threadSlot5Button.gameObject.SetActive(true);
        threadSlot6Button.gameObject.SetActive(true);

    }

    //Set the thread slot with the board and thread number
    public void chooseThreadSlot1() {
        Debug.Log("chooseThreadSlot1");
        GameObject.FindWithTag("svThreadOverlay1").GetComponent<svThreadOverlay>().startThreadData(currentBoard, tempThreadNumber.ToString());
        tempThreadNumber = 0;
        disbalethreadSlotButtonsResetCatalogMessage();
    }
    void chooseThreadSlot2()
    {
        Debug.Log("chooseThreadSlot2");
        GameObject.FindWithTag("svThreadOverlay2").GetComponent<svThreadOverlay>().startThreadData(currentBoard, tempThreadNumber.ToString());
        tempThreadNumber = 0;
        disbalethreadSlotButtonsResetCatalogMessage();

    }
    void chooseThreadSlot3()
    {
        Debug.Log("chooseThreadSlot3");
        GameObject.FindWithTag("svThreadOverlay3").GetComponent<svThreadOverlay>().startThreadData(currentBoard, tempThreadNumber.ToString());
        tempThreadNumber = 0;
        disbalethreadSlotButtonsResetCatalogMessage();

    }
    void chooseThreadSlot4()
    {
        Debug.Log("chooseThreadSlot4");
        GameObject.FindWithTag("svThreadOverlay4").GetComponent<svThreadOverlay>().startThreadData(currentBoard, tempThreadNumber.ToString());
        tempThreadNumber = 0;
        disbalethreadSlotButtonsResetCatalogMessage();

    }
    void chooseThreadSlot5()
    {
        Debug.Log("chooseThreadSlot5");
        GameObject.FindWithTag("svThreadOverlay5").GetComponent<svThreadOverlay>().startThreadData(currentBoard, tempThreadNumber.ToString());
        tempThreadNumber = 0;
        disbalethreadSlotButtonsResetCatalogMessage();

    }
    void chooseThreadSlot6()
    {
        Debug.Log("chooseThreadSlot6");
        GameObject.FindWithTag("svThreadOverlay6").GetComponent<svThreadOverlay>().startThreadData(currentBoard, tempThreadNumber.ToString());
        tempThreadNumber = 0;
        disbalethreadSlotButtonsResetCatalogMessage();

    }
    //

    void disbalethreadSlotButtonsResetCatalogMessage()
    {
        //Turns off the thread slot message and the thread slot messages
        threadSlot1Button.gameObject.SetActive(false);
        threadSlot2Button.gameObject.SetActive(false);
        threadSlot3Button.gameObject.SetActive(false);
        threadSlot4Button.gameObject.SetActive(false);
        threadSlot5Button.gameObject.SetActive(false);
        threadSlot6Button.gameObject.SetActive(false);

        catalogMessage.text = "";
    }

    void ClearSVCatalogButtons()
    {
        //Clear the catalog buttons if they are any
        Debug.Log("ClearSVCatalogButtons");

        GameObject[] catalogButtons = GameObject.FindGameObjectsWithTag("catalogButton");
        var count = catalogButtons.Length;
        Debug.Log(count);

        for (int i = 0; i < count; i++)
        {
            Destroy(catalogButtons[i]);
        }

    }

}



