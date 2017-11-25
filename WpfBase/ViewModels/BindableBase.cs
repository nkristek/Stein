﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfBase.ViewModels
{
    /// <summary>
    /// INotifyPropertyChanged implementation
    /// </summary>
    public abstract class BindableBase
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises an event on the PropertyChangedEventHandler
        /// </summary>
        /// <param name="propertyName">Name of the property which changed</param>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Gets called after a property changed and can be safely overriden in subclasses
        /// </summary>
        /// <param name="propertyName">Name of the property which changed</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) { }

        /// <summary>
        /// Sets a property if the value is different and raises an event on the PropertyChangedEventHandler
        /// </summary>
        /// <typeparam name="T">Type of the property to set</typeparam>
        /// <param name="storage">Reference to the storage variable</param>
        /// <param name="value">New value to set</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>True if the value was different from the storage variable and the PropertyChanged event was raised</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
