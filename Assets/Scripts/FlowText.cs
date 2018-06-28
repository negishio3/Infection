﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowText : MonoBehaviour {
    public enum FlowState
    {
        STATEPOSITION,
        FLOWMOVING,
        STOPMOVE
    }
    FlowState flowState;

    private RectTransform rect;
    private Text text;
    private int wave=1;   //現在のウェーブ
    private float speed=12f;

    public bool ChangeWave { get; set; }

    void Start() {
        rect = GetComponent<RectTransform>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        switch (flowState)
        {
            case FlowState.STATEPOSITION:
                StatePosition();
                break;
            case FlowState.FLOWMOVING:
                FlowMoving();
                break;
            case FlowState.STOPMOVE:
                StopMove();
                break;
        }
        text.text = "WAVE" + wave.ToString();
    }

    void StatePosition()
    {
        //初期位置移動処理
        flowState = FlowState.FLOWMOVING;
        rect.localPosition = new Vector3(550f, 0, 0);
        ChangeWave = false;
    }

    void FlowMoving()
    {
        //右から左へ流れる
        rect.localPosition -= new Vector3(Mathf.Abs(speed), 0, 0);
        if (rect.localPosition.x > -100 && rect.localPosition.x < 100)
        {
            speed = 5f;
        }
        else if (rect.localPosition.x < -550)
        {
            flowState = FlowState.STOPMOVE;
        }
        else
        {
            speed = 12f;
        }
    }

    void StopMove()
    {
        //流れた後の待機処理
        if (ChangeWave)
        {
            flowState = FlowState.STATEPOSITION;
            wave++;
        }
    }
}