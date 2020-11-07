using CG.Hosting;
using CG.Tools.CodeMap.Options;
using CG.Validations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace CG.Tools.CodeMap
{
    /// <summary>
    /// This class represents the code-behind for the application.
    /// </summary>
    public partial class App : Application
    {
        // *******************************************************************
        // Properties.
        // *******************************************************************

        #region Properties

        /// <summary>
        /// This property contains a host for the application.
        /// </summary>
        public IHost Host { get; protected set; }

        #endregion

        // *******************************************************************
        // Protected methods.
        // *******************************************************************

        #region Protected methods

        /// <summary>
        /// This method is called when the application starts.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnStartup(
            StartupEventArgs e
            )
        {
            // Create and configure a 'standard' host.
            Host = StandardHost.CreateStandardBuilder<App, AppOptions>()
                .Build();

            // Get the configuration.
            var configuration = Host.Services.GetRequiredService<IConfiguration>();

            // Set the syncfusion license key.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                configuration["Syncfusion"]
                );

            // Create and show the splash screen.
            var splashScreen = new SplashScreen("/Images/Splash.png");
            splashScreen.Show(true);

            // Wire up a handler, just in case ...
            AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
            {
                // Tell the world about our little problem ...
                MessageBox.Show(
                    (ex.ExceptionObject as Exception).Message +
                    Environment.NewLine +
                    (ex.ExceptionObject as Exception).StackTrace
                    );
            };

            // Give the base class a chance.
            base.OnStartup(e);
        }

        #endregion
    }
}
