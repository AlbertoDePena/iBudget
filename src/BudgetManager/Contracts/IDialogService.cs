using System;
using System.Windows;

namespace BudgetManager.Contracts
{
    public interface IDialogService
    {
        void ShowException(Exception e);

        void ShowInvalidData();

        void ShowOpenBudget();

        MessageBoxResult ShowSavePendingChanges();
    }
}