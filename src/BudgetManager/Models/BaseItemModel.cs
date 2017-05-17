using System.Windows.Media;

namespace BudgetManager.Models
{
    public abstract class BaseItemModel
    {
        protected BaseItemModel(string name, double budgeted, double spent)
        {
            Name = name;
            Spent = $"{spent.ToString("C")} / {budgeted.ToString("C")}";

            if (spent == budgeted)
            {
                Brush = new SolidColorBrush(Colors.Black);
            }
            else if (spent > budgeted)
            {
                Brush = new SolidColorBrush(Colors.Red);
            }
            else if (spent < budgeted)
            {
                Brush = new SolidColorBrush(Colors.Green);
            }
        }

        public string Name { get; }

        public string Spent { get; }

        public SolidColorBrush Brush { get; }
    }
}