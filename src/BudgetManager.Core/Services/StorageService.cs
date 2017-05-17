using System;
using System.Configuration;
using System.IO;
using BudgetManager.Core.Contracts;
using BudgetManager.Core.Models;
using Newtonsoft.Json;

namespace BudgetManager.Core.Services
{
    public class StorageService : IStorageService
    {
        public string GetDirectory()
        {
            var appSetting = ConfigurationManager.AppSettings["BudgetManager:DataSourceDirectory"];

            if (string.IsNullOrEmpty(appSetting))
            {
                throw new InvalidOperationException("Directory app setting not found");
            }

            return appSetting;
        }

        public BudgetDataSource LoadBudget(string filePath) => Load<BudgetDataSource>(filePath);

        public LookupDataSource LoadLookup(string filePath) => Load<LookupDataSource>(filePath);

        public void SaveBudget(BudgetDataSource dataSource) => Save(dataSource);

        public void SaveLookup(LookupDataSource dataSource) => Save(dataSource);

        private T Load<T>(string filePath) where T : IDataSource
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            using (var reader = File.OpenText(filePath))
            {
                var json = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private void Save(IDataSource dataSource)
        {
            if (dataSource == null)
            {
                throw new InvalidOperationException($"{nameof(dataSource)} is null");
            }

            if (string.IsNullOrEmpty(dataSource.FilePath))
            {
                throw new InvalidOperationException($"Data Source file path is null or empty");
            }

            using (var writer = File.CreateText(dataSource.FilePath))
            {
                var json = JsonConvert.SerializeObject(dataSource);

                writer.Write(json);
            }
        }
    }
}