namespace BudgetManager.Contracts
{
    public interface IView
    {
        bool CanSaveChanges();

        void Load();
    }
}