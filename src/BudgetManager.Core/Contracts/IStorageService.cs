using System.Threading.Tasks;
using BudgetManager.Core.Models;

namespace BudgetManager.Core.Contracts
{
    public interface IStorageService
    {
        string GetDirectory();

        BudgetDataSource LoadBudget(string filePath);

        LookupDataSource LoadLookup(string filePath);

        void SaveBudget(BudgetDataSource dataSource);

        void SaveLookup(LookupDataSource dataSource);
    }
}