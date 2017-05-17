using System;
using System.Collections.Generic;

namespace BudgetManager.Core.Models
{
    public class Budget
    {
        public DateTime CreatedOn { get; set; }
        public Guid? Id { get; set; }
        public double GrossIncome { get; set; }
        public float Tithe { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}