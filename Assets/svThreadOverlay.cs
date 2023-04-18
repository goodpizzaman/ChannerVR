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
using AngleSharp.Html.Parser;
using AngleSharp.Dom;

public class svThreadOverlay : MonoBehaviour
{

    [SerializeField] private GameObject threadObj;
    [SerializeField] private string threadSlotID;
    [SerializeField] private string currentBoardAbv;
    [SerializeField] private string currentThreadNumber;

    [SerializeField] List<string> tripcodesComplete;
    [SerializeField] List<string> dateTimeComplete;
    [SerializeField] List<int> replyNumberComplete;
    [SerializeField] List<string> mediaNameComplete;
    [SerializeField] List<string> commentComplete;
    [SerializeField] List<string> mediaExtComplete;

    [SerializeField] private Button refreshButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI loading;
    [SerializeField] private GameObject webmPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //Turns off the webm player
        webmPlayer.SetActive(false);
        //Adds listener for refresh button
        refreshButton.onClick.AddListener(refreshButtonOnClick);
    }

    public void refreshButtonOnClick()
    {
        //Refresh the thread data by clicking on the refresh button
        Debug.Log("refreshButtonOnClick");
        startThreadData(currentBoardAbv, currentThreadNumber);
    }

    void ClearSVThreadButtons()
    {
        //Clear the thread buttons
        Debug.Log("ClearSVThreadButtons");

        GameObject[] threadButtons = GameObject.FindGameObjectsWithTag("threadObj" + threadSlotID);
        var count = threadButtons.Length;
        Debug.Log(count);

        for (int i = 0; i < count; i++)
        {
            Destroy(threadButtons[i]);
        }

    }

    public void startThreadData(string boardAbv, string threadNumber)
    {
        //Starts the process of getting the thread data
        Debug.Log("=====startThreadData");

        //Clear data
        ClearSVThreadButtons();
        tripcodesComplete.Clear();
        dateTimeComplete.Clear();
        replyNumberComplete.Clear();
        mediaNameComplete.Clear();
        commentComplete.Clear();

        loading.gameObject.SetActive(true);
        currentBoardAbv = boardAbv;
        Debug.Log(boardAbv);
        currentThreadNumber = threadNumber;
        Debug.Log(threadNumber);
        

        StartCoroutine(getThreadData());
    }

    public IEnumerator getThreadData()
    {
        //Process of getting the thread data

        Debug.Log("=====getThreadData");
        //https://a.4cdn.org/\(self.boardAbv)/thread/\(threadNumber).json
        var url = "https://a.4cdn.org/" + currentBoardAbv + "/thread/" + currentThreadNumber + ".json";
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
                threadReplies thread = JsonConvert.DeserializeObject<threadReplies>(json);
                Debug.Log(thread);

                var replyCount = thread.posts.Count;
                Debug.Log(replyCount);
                Debug.Log("=========" + url);

                for (int t = 0; t < replyCount; t++){

                    Debug.Log(t);
                    var tripcode = thread.posts[t].name;
                    Debug.Log(tripcode);
                    tripcodesComplete.Add(tripcode);
                    var dateTime = thread.posts[t].now;
                    Debug.Log(dateTime);
                    dateTimeComplete.Add(dateTime);
                    var replyNumber = thread.posts[t].no;
                    Debug.Log(replyNumber);
                    replyNumberComplete.Add(replyNumber);
                    var mediaName = "https://i.4cdn.org/" + currentBoardAbv + "/" + thread.posts[t].tim;
                    //https://i.4cdn.org/adv/1681427803098176.jpg
                    Debug.Log(mediaName);
                    mediaExtComplete.Add(thread.posts[t].ext);
                    mediaNameComplete.Add(mediaName);


                    var comment = thread.posts[t].com;
                    //Debug.Log(comment);
                    //commentComplete.Add(comment);
                    var parser = new HtmlParser(new HtmlParserOptions
                    {
                        IsNotConsumingCharacterReferences = true,
                    });
                    IDocument commentParsed = parser.ParseDocument(comment);

                    var queryLength = commentParsed.QuerySelectorAll("*").Length;
                    commentComplete.Add(commentParsed.QuerySelectorAll("*")[2].TextContent.Replace("&gt;&gt;", ">>").Replace("&gt;", ">").Replace("&#039;", "'"));
                }

                Debug.Log("=====getThreadData DONE");
                setupThread();
            }
        }
    }

    void setupThread()
    {
        //Setup threadObjs
        Debug.Log("=====setupThread");

        //Make buttons
        for (int i = 0; i < commentComplete.Count; i++)
        {

            GameObject threadObjNew = Instantiate(threadObj);
            threadObjNew.SetActive(true);
            threadObjNew.transform.SetParent(threadObj.transform.parent, false);

            threadObjNew.GetComponent<threadObj>().setTripcode(tripcodesComplete[i]);
            threadObjNew.GetComponent<threadObj>().setDateTime(dateTimeComplete[i]);
            threadObjNew.GetComponent<threadObj>().setReplyNumber(replyNumberComplete[i].ToString());
            threadObjNew.GetComponent<threadObj>().setMediaExt(mediaExtComplete[i]);
            threadObjNew.GetComponent<threadObj>().setThreadContent(commentComplete[i], mediaNameComplete[i]);
            threadObjNew.GetComponent<threadObj>().setthreadSlotID(threadSlotID);

        }

        loading.gameObject.SetActive(false);
        Debug.Log("=====setupThreadDONE");
    }

}
