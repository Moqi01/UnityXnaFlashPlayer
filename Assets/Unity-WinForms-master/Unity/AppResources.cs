
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
            ArrowLeft = Resources.Load<Image>("arrow_left");
            ArrowRight = Resources.Load<Image>("arrow_right");
            ArrowUp = Resources.Load<Image>("arrow_up");
            Checked = Resources.Load<Image>("checked");
            Circle = Resources.Load<Image>("circle");
            Close = Resources.Load<Image>("close");
            CurvedArrowDown = Resources.Load<Image>("curved_arrow_down");
            CurvedArrowLeft = Resources.Load<Image>("curved_arrow_left");
            CurvedArrowRight = Resources.Load<Image>("curved_arrow_right");
            CurvedArrowUp = Resources.Load<Image>("curved_arrow_up");
            DateTimePicker = Resources.Load<Image>("datetimepicker");
            DropDownRightArrow = Resources.Load<Image>("dropdown_rightarrow");
            FileDialogBack = Resources.Load<Image>("filedialog_back");
            FileDialogFile = Resources.Load<Image>("filedialog_file");
            FileDialogFolder = Resources.Load<Image>("filedialog_folder");
            FileDialogRefresh = Resources.Load<Image>("filedialog_refresh");
            FileDialogUp = Resources.Load<Image>("filedialog_up");
            FormResize = Resources.Load<Image>("form_resize");
            NumericDown = Resources.Load<Image>("numeric_down");
            NumericUp = Resources.Load<Image>("numeric_up");
            RadioButton_Checked = Resources.Load<Image>("radiobutton_checked");
            RadioButton_Hovered = Resources.Load<Image>("radiobutton_hovered");
            RadioButton_Unchecked = Resources.Load<Image>("radiobutton_unchecked");
            TreeNodeCollapsed = Resources.Load<Image>("treenode_collapsed");
            TreeNodeExpanded = Resources.Load<Image>("treenode_expanded");
            Cursors.LoadResource();
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
        public void LoadResource()
        {
            Default = Resources.Load<Image>("cursors/hand");
            Help = Resources.Load<Image>("cursors/help");
            HSplit = Resources.Load<Image>("cursors/hsplit");
            IBeam = Resources.Load<Image>("cursors/ibeam");
            SizeAll = Resources.Load<Image>("cursors/sizeall");
            SizeNESW = Resources.Load<Image>("cursors/sizenesw");
            SizeNS = Resources.Load<Image>("cursors/sizens");
            SizeNWSE = Resources.Load<Image>("cursors/sizenwse");
            SizeWE = Resources.Load<Image>("cursors/sizewe");
            VSplit = Resources.Load<Image>("cursors/vsplit");

        }
    }
}