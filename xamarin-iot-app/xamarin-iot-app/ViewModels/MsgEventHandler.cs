using System.Collections.Generic;
using System.Text;
using System;

namespace xamarin_iot_app.ViewModels
{
    public class MsgEventArgs : EventArgs
    {
        #region Properties

        public string Msg { get; set; }

        #endregion

        #region Constructors

        public MsgEventArgs(string msg)
        {
            Msg = msg ?? throw new ArgumentNullException(nameof(msg));
        }

        #endregion
    }

    public delegate void MsgEventHandler(object sender, MsgEventArgs e);
}