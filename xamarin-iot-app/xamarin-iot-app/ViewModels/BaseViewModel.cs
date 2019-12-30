using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace xamarin_iot_app.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Events

        public event MsgEventHandler OnError;

        #endregion

        #region Fields

        private bool isBusy = false;
        private bool isInitialized = false;
        private string title = string.Empty;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #endregion

        #region Methods

        public void Initialize()
        {
            if (!isInitialized)
            {
                InitializeInternal();
                isInitialized = true;
            }
        }

        protected abstract void InitializeInternal();

        protected void RaiseError(string error)
        {
            OnError?.Invoke(this, new MsgEventArgs(error));
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}