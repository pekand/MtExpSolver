using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MtExpSolver
{
    public class ConsoleWrapper
    {
        public ConsoleForm? consoleForm = null;

        public delegate void MessageEventHandler(string message);

        public event MessageEventHandler MessageEvent;

        public void log(object message)
        {
            MessageEvent?.Invoke(message.ToString());
        }

        public void error(object message)
        {
            MessageEvent?.Invoke(message.ToString());
        }
    }
}
