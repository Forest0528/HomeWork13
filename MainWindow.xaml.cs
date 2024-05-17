using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BankingLibrary;

namespace HomeWork13
{
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        private Core core = new Core();
        private Log log = new Log();

        public MainWindow()
        {
            InitializeComponent();

            core.Transaction += Core_Transaction;
            bankList.ItemsSource = core.CreateBank();
            transList.ItemsSource = log.logFile;
        }

        private void Core_Transaction(string message)
        {
            log.AddToLog(message);
        }

        private void MenuItem_OnClick_Debug(object sender, RoutedEventArgs e) { }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("SkillBank v.0.2", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClientList_OnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                ContextMenu cm = this.FindResource("CmButton") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
            }
        }

        void RefreshList()
        {
            var bankDep = bankList.SelectedItem as BankDep;
            clientList.ItemsSource = (bankDep.Clients).Where(x => x != null);

            clientInfo.ItemsSource = clientList.SelectedItems;
            CollectionViewSource.GetDefaultView(clientList.SelectedItems).Refresh();
        }

        private void MenuItemSimpleDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            pSimpDep.IsOpen = true;
        }

        private void MenuItemSimpDep_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;

            bool result = UInt32.TryParse(amountSimpDepTextBox.Text, out uint amountSimpDeposit);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool checkFunds = core.CheckSuffAmount(currentClient, UInt32.Parse(amountSimpDepTextBox.Text));
            if (!checkFunds)
            {
                MessageBox.Show("Insufficient funds", "Insufficient funds", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            core.MakeSimpleDeposit(currentClient, amountSimpDeposit);

            pSimpDep.IsOpen = false;

            RefreshList();

            MessageBox.Show("Success", "Simple deposit", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MenuItemCapitalizedDeposit_OnClick(object sender, RoutedEventArgs e)
        {
            pCapDep.IsOpen = true;
        }

        private void MenuItemCapDep_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;

            bool result = UInt32.TryParse(amountCapDepTextBox.Text, out uint amountCapDeposit);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool checkFunds = core.CheckSuffAmount(currentClient, UInt32.Parse(amountCapDepTextBox.Text));
            if (!checkFunds)
            {
                MessageBox.Show("Insufficient funds", "Insufficient funds", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            core.MakeCapitalizedDeposit(currentClient, amountCapDeposit);

            pCapDep.IsOpen = false;

            RefreshList();

            MessageBox.Show("Success", "Capitalized deposit", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MenuItemLoan_OnClick(object sender, RoutedEventArgs e)
        {
            pLoan.IsOpen = true;
        }

        private void MenuItemGetLoan_OnClick(object sender, RoutedEventArgs e)
        {
            bool result = UInt32.TryParse(amountLoanTextBox.Text, out uint amountLoan);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Client currentClient = clientList.SelectedItem as Client;

            core.GetLoan(currentClient, amountLoan);

            pLoan.IsOpen = false;

            RefreshList();

            MessageBox.Show("Success", "Get loan", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MenuItemTransfer_OnClick(object sender, RoutedEventArgs e)
        {
            pTransfer.IsOpen = true;
            transferTo.ItemsSource = clientList.ItemsSource;
        }

        private void MenuItemMakeTransfer_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;
            Client recipient = transferTo.SelectedItem as Client;

            if (currentClient == transferTo.SelectedItem)
            {
                MessageBox.Show("You cannot make a transfer to yourself", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool result = UInt32.TryParse(amountTransferTextBox.Text, out uint amountTransfer);
            if (!result)
            {
                MessageBox.Show("Wrong amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool checkFunds = core.CheckSuffAmount(currentClient, UInt32.Parse(amountTransferTextBox.Text));
            if (!checkFunds)
            {
                MessageBox.Show("Insufficient funds", "Insufficient funds", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            core.TransferFunds(currentClient, recipient, amountTransfer);

            pTransfer.IsOpen = false;

            RefreshList();

            MessageBox.Show("Transfer completed", "Funds transfer", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ButtonDepInfo_OnClick(object sender, RoutedEventArgs e)
        {
            Client currentClient = clientList.SelectedItem as Client;

            if (currentClient.IsDeposit == Deposit.No)
            {
                MessageBox.Show("No information available", "Deposit information", MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            double[] months = core.DepositInfo(currentClient);

            month1.Text = months[0].ToString();
            month2.Text = months[1].ToString();
            month3.Text = months[2].ToString();
            month4.Text = months[3].ToString();
            month5.Text = months[4].ToString();
            month6.Text = months[5].ToString();
            month7.Text = months[6].ToString();
            month8.Text = months[7].ToString();
            month9.Text = months[8].ToString();
            month10.Text = months[9].ToString();
            month11.Text = months[10].ToString();
            month12.Text = months[11].ToString();

            pDepInfo.IsOpen = true;
        }

        private void MenuItemDepInfo_OnClick(object sender, RoutedEventArgs e)
        {
            pDepInfo.IsOpen = false;
        }

        private void BankList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (bankList.SelectedItems != null)
            {
                var clients = (e.OriginalSource as ListBox).SelectedItem as BankDep;
                clientList.ItemsSource = clients.Clients;
            }
        }

        private void ClientInfo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (clientList.SelectedItems != null)
            {
                var client = (e.OriginalSource as ListBox).SelectedItems;
                clientInfo.ItemsSource = client;
            }
        }

        private void UsersColumnHeader_OnClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                clientList.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            clientList.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
    }

    public class SortAdorner : Adorner
    {
        private static readonly Geometry ascGeometry = Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");
        private static readonly Geometry descGeometry = Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform(
                AdornedElement.RenderSize.Width - 15,
                (AdornedElement.RenderSize.Height - 5) / 2
            );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
}
