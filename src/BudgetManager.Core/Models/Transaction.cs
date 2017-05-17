using System;

namespace BudgetManager.Core.Models
{
    public class Transaction
    {
        public double Amount { get; set; }
        public Guid BudgetId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime Date { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}