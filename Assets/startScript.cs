﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XnaFlashPlayer;

public class startScript : MonoBehaviour
{
    MainForm form;
    // Start is called before the first frame update
    void Start()
    {
         form = new MainForm();
        form.Show();
        form.ToolStripMenuItem_Click(null, null);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.O)||Input .GetMouseButtonDown(0))
        //{
        //    form.ToolStripMenuItem_Click(null, null);
        //}

        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            form.Play();
        }
    }
}