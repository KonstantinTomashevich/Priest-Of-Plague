using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    public delegate void OnReply ();
    public Text DialogText;
    public Text FirstReplyText;
    public Text SecondReplyText;

    private OnReply _onFirst;
    private OnReply _onSecond;

    public void ReplyPressed (int replyIndex)
    {
        if (replyIndex == 0)
        {
            _onFirst ();
        }
        else
        {
            _onSecond ();
        }
        gameObject.SetActive (false);
    }

    public void Show (string text, string firstReply, string secondReply, OnReply onFirst, OnReply onSecond)
    {
        DialogText.text = text;
        FirstReplyText.text = firstReply;
        SecondReplyText.text = secondReply;

        _onFirst = onFirst;
        _onSecond = onSecond;
        gameObject.SetActive (true);
    }

    private void Start ()
    {
        gameObject.SetActive (false);
    }
}
