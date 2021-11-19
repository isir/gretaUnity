using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using OscJack;

public class TextInputWatcher : MonoBehaviour
{
    public InputField _textToWatch;
    protected UnityAction<string> _action;
    //protected OscPropertySender _sender;
    public UnityEvent<string> _onChanged;

    private void Start()
    {
        _action += OnValueChanged;
        _textToWatch.onValueChanged.AddListener(_action);
        //_sender = GetComponent<OscPropertySender>();
    }

    private void OnValueChanged(string msg)
    {
        _onChanged.Invoke(msg);
        //_sender.SendMessage(msg);
    }
}
