﻿using Clothing_Store_POS.Config;
using Clothing_Store_POS.Pages.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppNotifications;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Clothing_Store_POS
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static Frame MainFrame { get; private set; }
        private static Mutex _mutex;
        private static bool _isFirstInstance;

        public App()
        {

            _mutex = new Mutex(true, "ClothingStoreAppId", out _isFirstInstance);

            if (!_isFirstInstance)
            {
                ActivateExistingWindow();
                Environment.Exit(0);
            }

            // connection to the database
            AppFactory.AppDBContext = new AppDBContext();
            this.InitializeComponent();
            QuestPDF.Settings.License = LicenseType.Community;
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            if (m_window == null)
            {
                m_window = new MainWindow();

                MainFrame = new Frame();
                m_window.Content = MainFrame;

                // Default Page
                MainFrame.Navigate(typeof(LoginPage));

                m_window.Activate();
            }
            else
            {
                m_window.Activate();
            }
        }

        private void ActivateExistingWindow()
        {
            if (m_window != null)
            {
                m_window.Activate();
            }
        }

        private Window m_window;

        public Window MainWindow
        {
            get
            {
                return m_window;
            }
        }
    }
}
