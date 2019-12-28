using System;
using System.Collections.Generic;
using System.Text;

namespace xamarin_iot_app.ViewModels
{

    public delegate void MsgEventHandler(object sender, MsgEventArgs e);

    public class MsgEventArgs : EventArgs
    {
        public string Msg { get; set; }

        public MsgEventArgs(string msg)
        {
            Msg = msg ?? throw new ArgumentNullException(nameof(msg));
        }
    }
}
