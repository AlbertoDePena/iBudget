using System;

namespace BudgetManager.Core.Models
{
    public class Category
    {
        public double Amount { get; set; }
        public Guid CategoryGroupId { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}