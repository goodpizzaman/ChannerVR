using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using OVRSimpleJSON;

public class catalogButton : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI subject;
    [SerializeField] private TextMeshProUGUI comment;
    [SerializeField] private int threadNumber;
    [SerializeField] private Image buttonImage;

    public void catalogButtonClicked(){
        //Add thread to one of the thread slots using the thread number
        Debug.Log("catalogButtonClicked");
        Debug.Log(subject.text);
        Debug.Log(threadNumber);

        GameObject.FindWithTag("svCatalogWall").GetComponent<svCatalog>().addThreadToThreadList(threadNumber);
    }

    public void setSubject(string newSubject)
    {
        //Set subject for catalog thread button
        subject.text = newSubject;
    }
    public void setComment(string newComment)
    {
        //Set comment for catalog thread button
        comment.text = newComment;
    }
    public void setThreadNumber(int newThreadNumber)
    {
        //Set thread number for the catalog thread button
        //Not used visually
        threadNumber = newThreadNumber;
    }

    public void setImage(string url)
    {
        //Set image for the catalog button and starts the download from the web
        StartCoroutine(setImageFromURL(url));
    }
    IEnumerator setImageFromURL(string newImageURL)
    {
        //Downloads image from the web and adds it to the button

        //Debug.Log("setImageFromURL");
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(newImageURL);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError)
            Debug.Log(request.error);
        else
        {
            //Debug.Log("Image downloaded and inserted");
            var texture2d = DownloadHandlerTexture.GetContent(request);
            //Debug.Log(texture2d.height);
            //Debug.Log(texture2d.width);
            var sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
            buttonImage.sprite = sprite;
        }
    } 
}
