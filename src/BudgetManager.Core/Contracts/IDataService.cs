using System;
using System.Collections.Generic;
using BudgetManager.Core.Models;

namespace BudgetManager.Core.Contracts
{
    public interface IDataService
    {
        Budget Budget { get; }
        IEnumerable<Category> Categories { get; }
        IEnumerable<CategoryGroup> CategoryGroups { get; }
        IStorageService StorageService { get; }

        Guid CreateOrUpdateBudget(double grossIncome, float tithe);

        Guid CreateOrUpdateCategory(Category item);

        Guid CreateOrUpdateCategoryGroup(CategoryGroup item);

        Guid CreateOrUpdateTransaction(Transaction item);

        void DeleteCategory(Guid id);

        void DeleteCategoryGroup(Guid id);

        void DeleteTransaction(Guid guid);

        void LoadBudget(string filePath);

        void SaveBudget();
    }
}