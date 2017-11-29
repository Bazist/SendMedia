using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendMedia
{
    public class ClipboardHelper
    {
        public static void PasteText(string text)
        {
            var oldText = Clipboard.GetText();

            CopyToClipboard(text);

            SendKeys.Send("^V");

            CopyToClipboard(oldText);
        }

        public static void ClearText(int amountSymbols)
        {
            var delStr = new StringBuilder();

            for (int i = 0; i < amountSymbols; i++)
            {
                delStr.Append("{BS}");
            }

            SendKeys.Send(delStr.ToString());
        }

        private static void CopyToClipboard(string text)
        {
            Clipboard.Clear();

            Clipboard.SetText(text);
        }
    }
}
