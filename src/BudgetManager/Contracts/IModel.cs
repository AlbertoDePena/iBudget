namespace BudgetManager.Contracts
{
    public interface IModel
    {
        void CopyModelToEntity();

        bool CanSave();
    }
}