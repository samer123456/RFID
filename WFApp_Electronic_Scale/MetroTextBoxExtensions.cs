using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFApp_Electronic_Scale
{
    public static class MetroTextBoxExtensions
    {
        public static void AppendText(this MetroTextBox textBox, string text, Color color)
        {
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
            textBox.ForeColor = color;
            textBox.AppendText(text);
            textBox.ForeColor = textBox.ForeColor;
        }
    }
}
