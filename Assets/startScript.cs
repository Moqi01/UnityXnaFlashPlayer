using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XnaFlashPlayer;

public class startScript : MonoBehaviour
{
    MainForm form;
    public Text n;
    public Vector3 Pos;
    public float Value = 5000;
    public float AddValue = 100;
    public static Vector2 vector2;

    // Start is called before the first frame update
    void Start()
    {
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        form = new MainForm();
        form.Show();
        form.ToolStripMenuItem_Click(null, null);
        form.HideMainMenu();
        Pos = Camera.main.transform.position;
        //UnityEngine.PSVita.Diagnostics.enableHUD = true;
        n.text = form.FileName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            Application.LoadLevel(Application.loadedLevelName);
        }

        if (Input.GetKeyDown(KeyCode.P)|| Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            form.Play();
        }

       else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.JoystickButton11))
        {
            form.L();
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.JoystickButton9))
        {
            form.R();
        }

        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton8))
        {
           form.Up(); 
        }

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.JoystickButton10))
        {
           form.Down(); 
        }

        else if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            form.OpenFile();

           
        }

        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            form.předchozíSnímekToolStripMenuItem_Click(null,null);
        }
        else if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            form.dalšíSnímekToolStripMenuItem_Click(null, null);
        }

        if (Input.GetKeyDown(KeyCode.G))
            SetCameraPos();
    }

    public void SetCameraPos()
    {
        Microsoft.Xna.Framework.Vector2 vector2 = XnaFlash.FlashDocument.WH;
        Camera.main .transform.position = new Vector3(Pos.x + vector2.X / Value * AddValue, Pos.y + vector2.Y / Value * AddValue, Pos.z);
    }
}
