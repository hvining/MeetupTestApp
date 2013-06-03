using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using MeetupTestClient.UILogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MeetupTestClient.Views;
using Windows.ApplicationModel.Resources;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace MeetupTestClient
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        IUnityContainer _container = new UnityContainer();
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("EventsView", null);
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            _container.RegisterInstance<INavigationService>(NavigationService);

            _container.RegisterType<EventsViewModel>(new TransientLifetimeManager());

            ViewModelLocator.Register(typeof(EventsView).ToString(), () => _container.Resolve<EventsViewModel>());

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "MeetupTestClient.UILogic.ViewModels.{0}ViewModel, MeetupTestClient.UILogic", viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
        }

        protected override Type GetPageType(string pageToken)
        {
            var assemblyQualifiedAppType = this.GetType().GetTypeInfo().AssemblyQualifiedName;

            var pageNameWithParameter = assemblyQualifiedAppType.Replace(this.GetType().FullName, this.GetType().Namespace + ".Views.{0}");

            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, pageToken);
            var viewType = Type.GetType(viewFullName);

            if (viewType == null)
            {
                viewType = base.GetPageType(pageToken);
            }

            return viewType;
        }

        protected override object Resolve(Type type)
        {
            // Use the container to resolve types (e.g. ViewModels and Flyouts)
            // so their dependencies get injected
            return _container.Resolve(type);
        }

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            base.OnFileActivated(args);
        }

        protected override async void OnSearchActivated(SearchActivatedEventArgs args)
        {
            base.OnSearchActivated(args);
        }

        protected override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            base.OnFileOpenPickerActivated(args);
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            base.OnShareTargetActivated(args);
        }

        protected override IList<SettingsCharmActionItem> GetSettingsCharmActionItems()
        {
            return base.GetSettingsCharmActionItems();
        }
    }
}
