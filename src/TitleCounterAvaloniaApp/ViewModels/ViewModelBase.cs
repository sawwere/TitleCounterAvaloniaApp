﻿using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using ReactiveUI.Validation.Helpers;

namespace tc.ViewModels
{
    public abstract class ViewModelBase : ReactiveValidationObject
    {
        //public void RaisePropertyChanging(PropertyChangingEventArgs args) => OnPropertyChanging(args);

        //public void RaisePropertyChanged(PropertyChangedEventArgs args) => OnPropertyChanged(args);

        private static readonly string[] NO_ERRORS = Array.Empty<string>();
        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        /// <inheritdoc />
        //public virtual IEnumerable GetErrors(string? propertyName)
        //{
        //    if (_errorsByPropertyName.TryGetValue(propertyName, out var errorList))
        //    {
        //        return errorList;
        //    }
        //    return NO_ERRORS;
        //}

        ///// <inheritdoc />
        //public bool HasErrors => _errorsByPropertyName.Count > 0;

        ///// <inheritdoc />
        //public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        //protected void AddError(string propertyName, string error)
        //{
        //    if (_errorsByPropertyName.TryGetValue(propertyName, out var errorList))
        //    {
        //        if (!errorList.Contains(error))
        //        {
        //            errorList.Add(error);
        //        }
        //    }
        //    else
        //    {
        //        _errorsByPropertyName.Add(propertyName, new List<string> { error });
        //    }
        //    ErrorsChanged.
        //    ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //}

        //protected void RemoveError(string propertyName)
        //{
        //    if (_errorsByPropertyName.ContainsKey(propertyName))
        //    {
        //        _errorsByPropertyName.Remove(propertyName);
        //        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        //    }
        //}
    }
}
