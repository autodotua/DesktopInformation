﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopInformation
{
    public static class Enums
    {
        [Serializable]
        public enum ObjType
        {
            None,
            PlainText,
            Text,
            Bar,
            Pie
        }
        [Serializable]
        public enum Status
        {
           Running,
           Pausing,
           Stoped
        }
    }
}
