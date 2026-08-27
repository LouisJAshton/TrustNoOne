using System;
using UnityEngine;

namespace Combat.UI
{
    [Serializable]
    public struct LogData
    {
        public string title;
        public string message;
        public Sprite icon;

        public LogData(string message, string title, Sprite icon = null)
        {
            this.message = message;
            this.icon = icon;
            this.title = title;
        }
    }
}