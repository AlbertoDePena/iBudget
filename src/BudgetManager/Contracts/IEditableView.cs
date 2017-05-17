namespace BudgetManager.Contracts
{
    public interface IEditableView : IView
    {
        void Add();

        void Remove(object dataContext);
    }
}