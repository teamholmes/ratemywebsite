using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPlatform.General;

namespace OpenPlatform.General.Model
{
    public class CBoxListInfo
    {
        public CBoxListInfo(string value, string displayText, bool isChecked)
        {
            this.Value = value;
            this.DisplayText = displayText;
            this.IsChecked = isChecked;
        }

        public string Value { get; private set; }
        public string DisplayText { get; private set; }
        public bool IsChecked { get; private set; }
    }



}
