using System;
using System.Windows;
using BudgetManager.Contracts;
using BudgetManager.Core.Extensions;

namespace BudgetManager.Helper
{
    public class MessageHelper : IDialogService
    {
        public void ShowException(Exception e)
            => MessageBox.Show(e.ToDetail(),
                  "iBudget", MessageBoxButton.OK, MessageBoxImage.Error);

        public void ShowInvalidData()
            => MessageBox.Show($"You have invalid data. {Environment.NewLine}Make sure there are no empty / duplicates values and values are properly formatted.",
                   "iBudget", MessageBoxButton.OK, MessageBoxImage.Error);

        public void ShowOpenBudget()
            => MessageBox.Show("Please open a budget.",
                  "iBudget", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        public MessageBoxResult ShowSavePendingChanges()
            => MessageBox.Show("You have pending changes. Would you like to save this budget before exiting?",
                    "iBudget", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
    }
}