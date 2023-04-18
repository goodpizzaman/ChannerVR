using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class threadObj : MonoBehaviour
{

    [SerializeField] private Button replyExapndShrink;
    [SerializeField] private TextMeshProUGUI repliesForThread;
    [SerializeField] private Button mediaButton;
    [SerializeField] private string mediaButtonURL;
    [SerializeField] private string threadSlotID;

    [SerializeField] private TextMeshProUGUI tripcode;
    [SerializeField] private TextMeshProUGUI dateTime;
    [SerializeField] private TextMeshProUGUI replyNumber;

    [SerializeField] private Image mediaImage;
    [SerializeField] private string mediaExt;
    [SerializeField] private TextMeshProUGUI textWithMedia;
    [SerializeField] private TextMeshProUGUI textNoMedia;
    [SerializeField] private GameObject webmPlayer;
    [SerializeField] private Button webmPlayerExit;

    public void setTripcode(string newTripcode)
    {
        //Set the tripcode
        tripcode.text = newTripcode;
    }
    public void setDateTime(string newDateTime)
    {
        //Set data and time 
        dateTime.text = newDateTime;
    }
    public void setReplyNumber(string newReplyNumber)
    {
        //Set the reply number
        replyNumber.text = newReplyNumber;
    }
    public void setMediaExt(string newMediaExt)
    {
        //Set the media extension
        mediaExt = newMediaExt;
    }
    public void setthreadSlotID(string newthreadSlotID)
    {
        //Set the thread slot
        threadSlotID = newthreadSlotID;
    }

    public void setThreadContent(string comment, string media)
    {
        //Set the thread content
        //Set if the reply has media or not
        Debug.Log("=====setThreadContent");
        Debug.Log(media);
        Debug.Log(mediaExt);
        if (mediaExt == ".jpeg" || mediaExt == ".jpg" || mediaExt == ".webm" || mediaExt == ".gif")
        {
            //Media content
            //webm
            mediaButton.gameObject.SetActive(false);
            if (mediaExt == ".webm")
            {
                mediaButton.gameObject.SetActive(true);
                mediaButton.onClick.AddListener(mediaButtonClick);
            }

            if (mediaExt == ".webm" || mediaExt == ".gif")
            {
                setMediaImage(media + "s.jpg");
                mediaButtonURL = media + mediaExt;
            }
            else
            {
                setMediaImage(media + mediaExt);
            }

            setTextWithMedia(comment);

        }
        else
        {
            //No media content
            mediaButton.gameObject.SetActive(false);
            setTextNoMedia(comment);
        }
    }
    public void setMediaImage(string newMediaImage)
    {
        //Set media thumbnail 
        Debug.Log("setMediaImage");
        Debug.Log(newMediaImage);
        StartCoroutine(setImageFromURL(newMediaImage));
    }
    IEnumerator setImageFromURL(string newImageURL)
    {
        //Process of getting image of a reply with media

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
            mediaImage.sprite = sprite;
        }
    }
    public void setTextWithMedia(string newComment)
    {
        //Set reply with media
        textNoMedia.enabled = false;
        textWithMedia.text = newComment;
    }
    public void setTextNoMedia(string newComment)
    {
        //Set reply with no media
        textWithMedia.enabled = false;
        textNoMedia.text = newComment;
    }

    void mediaButtonClick()
    {
        //Process when user click on the eye media button to play webm
        //VP8 and VP9 works on Quest Pro
        Debug.Log("mediaButtonClick");
        Debug.Log("threadSlotID " + threadSlotID);
        if (mediaExt == ".webm")
        {
            webmPlayer.SetActive(true);
            webmPlayerExit.onClick.AddListener(exitWebm);
            var videoPlayer = GameObject.FindWithTag("webm" + threadSlotID).GetComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.url = mediaButtonURL;
            videoPlayer.Play();

        }
    }
    void exitWebm()
    {
        //Closes the webm player
        webmPlayer.SetActive(false);
    }
}
