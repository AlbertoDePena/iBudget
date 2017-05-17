using System;
using System.Collections.Generic;
using System.Windows;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using BudgetManager.Core.Services;
using BudgetManager.Helper;
using BudgetManager.ViewModels;
using Caliburn.Micro;

namespace BudgetManager
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void BuildUp(object instance)
            => _container.BuildUp(instance);

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Singleton<IWindowManager, AppWindowManager>();
            _container.Singleton<IEventAggregator, EventAggregator>();
            _container.Singleton<IStorageService, StorageService>();
            _container.Singleton<IDataService, DataService>();
            _container.Singleton<IDialogService, MessageHelper>();
            _container.PerRequest<IShellView, ShellViewModel>();
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
            => _container.GetAllInstances(service);

        protected override object GetInstance(Type service, string key)
            => _container.GetInstance(service, key);

        protected override void OnStartup(object sender, StartupEventArgs e)
            => DisplayRootViewFor<IShellView>();
    }
}