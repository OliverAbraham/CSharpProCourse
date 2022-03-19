using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abraham.UI
{
    public enum MessageResult { OK, Yes, No, Cancel }

    public interface IFramework
    {
        void Close(int result = 0);

        MessageResult Message(string text, string caption = "");
    }
}
