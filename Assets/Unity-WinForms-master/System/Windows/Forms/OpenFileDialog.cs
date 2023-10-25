using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Windows.Forms
{
    public sealed class OpenFileDialog : FileDialog
    {
        public OpenFileDialog()
        {
            Text = "Open";

            buttonOk.Text = "Open";
        }

        public string DefaultExt { get; internal set; }
        public string Title { get; internal set; }
    }
}
