Using this library in your project:

- Ensure that you install Xamarin.Essentials into all of your projects.
- Ensure that the Mvvm.TinyIoC nuget is installed in your Xamarin.Forms project.
    - When instantiating the IoCContainer, call IoCCoreContainer.
- Implement the ISettingsService in your Xamarin.Forms project
- To support certificate pinning. Call the LoadTrustedCert() method on the HttpClient from App.xaml.cs