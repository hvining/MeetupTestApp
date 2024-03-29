// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    /// <summary>
    /// The INavigationAware interface should be used for view models that require persisting and loading state due to suspend/resume events.
    /// The Microsoft.Practices.Prism.StoreApps.ViewModel base class implements this interface, therefore every view model that inherits from it
    /// will support the navigation aware methods. 
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Called when navigation is performed to a page. You can use this method to load state if it is available.
        /// </summary>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <param name="navigationMode">The navigation mode.</param>
        /// <param name="viewModelState">The state of the view model.</param>
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState);

        /// <summary>
        /// This method will be called when navigating away from a page. You can use this method to save your view model data in case of a suspension event.
        /// </summary>
        /// <param name="viewModelState">The state of the view model.</param>
        /// <param name="suspending">If set to <c>true</c> you are navigating away from this view model due to a suspension event.</param>
        void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending);
    }
}
