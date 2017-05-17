using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BudgetManager.Core.Contracts;
using BudgetManager.Core.Models;

namespace BudgetManager.Core.Services
{
    public class DataService : IDataService
    {
        public DataService(IStorageService storageService)
        {
            StorageService = storageService;

            BudgetDataSource = new BudgetDataSource() { Budget = new Budget() };
            LookupDataSource = new LookupDataSource() { FilePath = $@"{StorageService.GetDirectory()}\Lookup.json" };
        }

        public Budget Budget => BudgetDataSource.Budget;
        public IEnumerable<Category> Categories => LookupDataSource.Categories;
        public IEnumerable<CategoryGroup> CategoryGroups => LookupDataSource.CategoryGroups;
        public IStorageService StorageService { get; }
        private BudgetDataSource BudgetDataSource { get; set; }
        private LookupDataSource LookupDataSource { get; set; }

        public Guid CreateOrUpdateBudget(double grossIncome, float tithe)
        {
            var date = DateTime.UtcNow;

            var filePath = $@"{StorageService.GetDirectory()}\{date.ToString("MMMM yyyy")}.json";

            if (File.Exists(filePath))
            {
                Budget.GrossIncome = grossIncome;
                Budget.Tithe = tithe;

                return Budget.Id.GetValueOrDefault();
            }

            var budget = new Budget()
            {
                Id = Guid.NewGuid(),
                GrossIncome = grossIncome,
                Tithe = tithe,
                CreatedOn = date
            };

            BudgetDataSource = new BudgetDataSource() { Budget = budget, FilePath = filePath };

            return Budget.Id.GetValueOrDefault();
        }

        public Guid CreateOrUpdateCategory(Category item)
        {
            var category = LookupDataSource.Categories.FirstOrDefault(x => x.Id == item.Id);

            if (category == null)
            {
                category = new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    Amount = item.Amount,
                    CategoryGroupId = item.CategoryGroupId
                };

                LookupDataSource.Categories.Add(category);

                return category.Id.GetValueOrDefault();
            }

            category.Name = item.Name;
            category.Amount = item.Amount;
            category.CategoryGroupId = item.CategoryGroupId;

            return category.Id.GetValueOrDefault();
        }

        public Guid CreateOrUpdateCategoryGroup(CategoryGroup item)
        {
            var categoryGroup = LookupDataSource.CategoryGroups.FirstOrDefault(x => x.Id == item.Id);

            if (categoryGroup == null)
            {
                categoryGroup = new CategoryGroup()
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name
                };

                LookupDataSource.CategoryGroups.Add(categoryGroup);

                return categoryGroup.Id.GetValueOrDefault();
            }

            categoryGroup.Name = item.Name;

            return categoryGroup.Id.GetValueOrDefault();
        }

        public Guid CreateOrUpdateTransaction(Transaction item)
        {
            var transaction = BudgetDataSource.Budget.Transactions.FirstOrDefault(x => x.Id == item.Id);

            if (transaction == null)
            {
                transaction = new Transaction()
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    Amount = item.Amount,
                    CategoryId = item.CategoryId,
                    Date = item.Date
                };

                BudgetDataSource.Budget.Transactions.Add(transaction);

                return transaction.Id.GetValueOrDefault();
            }

            transaction.Name = item.Name;
            transaction.Amount = item.Amount;
            transaction.Date = item.Date;
            transaction.CategoryId = item.CategoryId;

            return transaction.Id.GetValueOrDefault();
        }

        public void DeleteCategory(Guid id)
        {
            var category = LookupDataSource.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null)
                return;

            var inUse = BudgetDataSource.Budget.Transactions.Any(x => x.CategoryId == id);

            if (inUse)
            {
                throw new InvalidOperationException("Category has a reference. Cannot delete.");
            }

            LookupDataSource.Categories.Remove(category);
        }

        public void DeleteCategoryGroup(Guid id)
        {
            var categoryGroup = LookupDataSource.CategoryGroups.FirstOrDefault(x => x.Id == id);

            if (categoryGroup == null)
                return;

            var inUse = LookupDataSource.Categories.Any(x => x.CategoryGroupId == id);

            if (inUse)
            {
                throw new InvalidOperationException("Category group has a reference. Cannot delete.");
            }

            LookupDataSource.CategoryGroups.Remove(categoryGroup);
        }

        public void DeleteTransaction(Guid id)
        {
            var transaction = BudgetDataSource.Budget.Transactions.FirstOrDefault(x => x.Id == id);

            if (transaction == null)
                return;

            BudgetDataSource.Budget.Transactions.Remove(transaction);
        }

        public void LoadBudget(string filePath)
        {
            BudgetDataSource = StorageService.LoadBudget(filePath);

            if (BudgetDataSource == null)
            {
                BudgetDataSource = new BudgetDataSource() { Budget = new Budget() };
            }

            LookupDataSource = StorageService.LoadLookup($@"{StorageService.GetDirectory()}\Lookup.json");

            if (LookupDataSource == null)
            {
                LookupDataSource = new LookupDataSource() { FilePath = $@"{StorageService.GetDirectory()}\Lookup.json" };
            }
        }

        public void SaveBudget()
        {
            StorageService.SaveBudget(BudgetDataSource);
            StorageService.SaveLookup(LookupDataSource);
        }
    }
}