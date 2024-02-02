
using System;
using System.Collections.Generic;

using UnityEngine;

using Image = UnityEngine.Texture2D;
using uFont = UnityEngine.Font;

[Serializable]
public class AppResources
{
    public List<uFont> Fonts;
    public List<Image> Textures;
    public ReservedResources Images;

    /// <summary>
    /// System resources.
    /// </summary>
    [Serializable]
    public struct ReservedResources
    {
        [Tooltip("Form resize icon")]
        public Image ArrowDown;

        [Tooltip("Form resize icon, MonthCalendar, TabControl")]
        public Image ArrowLeft;

        [Tooltip("Form resize icon, MonthCalendar, TabControl")]
        public Image ArrowRight;

        [Tooltip("Form resize icon")]
        public Image ArrowUp;

        public Image Circle;

        [Tooltip("Checkbox, ToolStripItem")]
        public Image Checked;

        [Tooltip("Form close button")]
        public Image Close;

        public CursorImages Cursors;

        [Tooltip("ComboBox, VScrollBar")]
        public Image CurvedArrowDown;

        [Tooltip("HScrollBar")]
        public Image CurvedArrowLeft;

        [Tooltip("HScrollBar")]
        public Image CurvedArrowRight;

        [Tooltip("VScrollBar")]
        public Image CurvedArrowUp;

        [Tooltip("DateTimePicker button")]
        public Image DateTimePicker;

        [Tooltip("ToolStripDropDown")]
        public Image DropDownRightArrow;

        public Image FileDialogBack;
        public Image FileDialogFile;
        public Image FileDialogFolder;
        public Image FileDialogRefresh;
        public Image FileDialogUp;
        
        public Image FormResize;

        [Tooltip("NumericUpDown")]
        public Image NumericDown;

        [Tooltip("NumericUpDown")]
        public Image NumericUp;

        public Image RadioButton_Checked;
        public Image RadioButton_Hovered;
        public Image RadioButton_Unchecked;

        [Tooltip("Tree")]
        public Image TreeNodeCollapsed;

        [Tooltip("Tree")]
        public Image TreeNodeExpanded;
        public void LoadResource()
        {
            ArrowDown = Resources.Load<Image>("arrow_down");
            ArrowLeft = Resources.Load<Image>("arrow_down");
            ArrowRight = Resources.Load<Image>("arrow_down");
            ArrowUp = Resources.Load<Image>("arrow_down");
            Circle = Resources.Load<Image>("arrow_down");
            Checked = Resources.Load<Image>("arrow_down");
            Close = Resources.Load<Image>("arrow_down");
            CurvedArrowDown = Resources.Load<Image>("arrow_down");
            CurvedArrowLeft = Resources.Load<Image>("arrow_down");
            CurvedArrowRight = Resources.Load<Image>("arrow_down");
            CurvedArrowUp = Resources.Load<Image>("arrow_down");
            DateTimePicker = Resources.Load<Image>("arrow_down");
            DropDownRightArrow = Resources.Load<Image>("arrow_down");
            FileDialogBack = Resources.Load<Image>("arrow_down");
            FileDialogFile = Resources.Load<Image>("arrow_down");
            FileDialogFolder = Resources.Load<Image>("arrow_down");
            FileDialogRefresh = Resources.Load<Image>("arrow_down");
            FileDialogUp = Resources.Load<Image>("arrow_down");
            FormResize = Resources.Load<Image>("arrow_down");
            NumericDown = Resources.Load<Image>("arrow_down");
            NumericUp = Resources.Load<Image>("arrow_down");
            RadioButton_Checked = Resources.Load<Image>("arrow_down");
            RadioButton_Hovered = Resources.Load<Image>("arrow_down");
            RadioButton_Unchecked = Resources.Load<Image>("arrow_down");
            TreeNodeCollapsed = Resources.Load<Image>("arrow_down");
            TreeNodeExpanded = Resources.Load<Image>("arrow_down");
        }
    }

    [Serializable]
    public struct CursorImages
    {
        [Tooltip("Leave this field empty if you don't want to use your own cursor.")]
        public Image Default;

        public Image Hand;
        public Image Help;
        public Image HSplit;
        public Image IBeam;
        public Image SizeAll;
        public Image SizeNESW;
        public Image SizeNS;
        public Image SizeNWSE;
        public Image SizeWE;
        public Image VSplit;
    }
}