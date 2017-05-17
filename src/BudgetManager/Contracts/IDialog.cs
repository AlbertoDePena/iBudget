namespace BudgetManager.Contracts
{
    public interface IDialog
    {
        void Save();

        dynamic ViewSettings(string title);
    }
}