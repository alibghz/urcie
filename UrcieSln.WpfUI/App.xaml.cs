using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UrcieSln.Domain;
using UrcieSln.WpfUI.Common;
using UrcieSln.WpfUI.ViewModels;
using UrcieSln.WpfUI.Views;

namespace UrcieSln.WpfUI
{
    public partial class App : Application
    {
        IUnityContainer container;
        public static string[] Args;

        protected override void OnStartup(StartupEventArgs e)
        {
            Args = e.Args;
            ConfigureContainer();
            ComposeObjects();
            Application.Current.ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;
            Application.Current.MainWindow.Show();
            base.OnStartup(e);
        }

        private void ConfigureContainer()
        {
            container = new UnityContainer();
            FileStorage storage = FileStorage.GetInstance();
            container.RegisterInstance<FileStorage>(storage);
        }

        private void ComposeObjects()
        {
            var mainWindow = container.Resolve<MainWindow>();
            Application.Current.MainWindow = container.Resolve<MainWindow>();
        }
    }
}