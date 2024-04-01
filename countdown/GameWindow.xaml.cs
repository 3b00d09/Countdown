using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace countdown
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GameWindow : Window
    {

        public List<int> bigNumbers = [25, 50, 75, 100];
        public List<int> smallNumbers = [1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10];
        public int counter = 30;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public int selectedCount = 0;

        public GameWindow()
        {
            InitializeComponent();


            // generate the 28 buttons in a for loop

            for (int i = 0; i < 29; i++)
            {
                Button button = new Button
                {
                    Content = ""
                };

                // each if / else if covers a row, each row has 7 elements
                // for the columns, we do elements per row + current index - 7 * current row. figured it out by trial and error
                if(i >=0 && i < 8)
                {
                    Grid.SetRow(button, 0);
                    Grid.SetColumn(button, 7 + i - 7);
                }
                else if(i >= 8 && i < 15)
                {
                    Grid.SetRow(button, 1);
                    Grid.SetColumn(button, 6 + i - 14);
                }
                else if(i >= 14 && i < 22)
                {
                    Grid.SetRow(button, 2);
                    Grid.SetColumn(button, 6 + i - 21);
                }
                else
                {
                    Grid.SetRow(button, 3);
                    Grid.SetColumn(button, 6 + i - 28);
                }

                // first 4 buttons are for large numbers
                if(i >= 0 && i < 4)
                {
                    button.Click += bigNumberSelected;
                }
                else
                {
                    button.Click += smnallNumberSelected;
                }

                button.IsEnabled = false;
                BtnsContainer.Children.Add(button);
                
            }
        }

        private void startGame(object sender, RoutedEventArgs e)
        {
            // reset global variables
            bigNumbers = [25, 50, 75, 100];
            smallNumbers = [1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10];
            selectedCount = 0;
            lblTarget.Content = null;

            // reset the 6 choices fields
            foreach (var child in pnlSelected.Children)
            {
                Label label = (Label)child;
                label.Content = null;
            }

            foreach (var child in BtnsContainer.Children)
            {
                Button button = (Button)child;
                button.Content = null;
            }


            // enable the buttons grid
            enableButtons();
            btnTarget.IsEnabled = false;

        }

        public void bigNumberSelected(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            clickedButton.IsEnabled = false;
            int choice = extractBigNumber();
            clickedButton.Content = choice;

            // loop through all labels and as soon as we find an empty one, we put a number in it, increment selectedCount, and then break out
            foreach (var child in pnlSelected.Children)
            {
                Label label = (Label)child;
                if(label.Content == null)
                {
                    label.Content = choice;
                    selectedCount++;
                    break;
                }
            }

            if(selectedCount == 6)
            {
                btnTarget.IsEnabled = true;
            }
        }

        public void smnallNumberSelected(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            clickedButton.IsEnabled = false;
            int choice = extractSmallNumber();
            clickedButton.Content = choice;

            foreach (var child in pnlSelected.Children)
            {
                Label label = (Label)child;
                if (label.Content == null)
                {
                    label.Content = choice;
                    selectedCount++;
                    break;
                }
            }
            if (selectedCount == 6)
            {
                btnTarget.IsEnabled = true;
            }
        }


        public int extractBigNumber()
        {
            // get a random index within the bigNumbers array and get a number out of it, then remove the number from the array to ensure we dont pick it again
            Random random = new Random();
            int targetIndex = random.Next(bigNumbers.Count);
            int randomNumber = bigNumbers[targetIndex];
            bigNumbers.RemoveAt(targetIndex);
            return randomNumber;
        }

        public int extractSmallNumber()
        {
            Random random = new Random();
            int targetIndex = random.Next(smallNumbers.Count);
            int randomNumber = smallNumbers[targetIndex];
            smallNumbers.RemoveAt(targetIndex);
            return randomNumber;
        }

        public void generateNumber(object sender, RoutedEventArgs e)
        {

            Random random = new Random();
            int number = random.Next(100, 999);
            lblTarget.Content = number;

            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();

            disableButtons();
            btnTarget.IsEnabled = false;
            btnShuffle.IsEnabled = false;
            disableButtons();
        }

        public void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            int seconds = int.Parse(lblTime.Content.ToString());

            seconds--;

            lblTime.Content = seconds.ToString();
            if(seconds == 0)
            {
                stopGame();
            }
        }

        public void stopGame()
        {
            dispatcherTimer.Stop();
            MessageBox.Show("Time is up!");
            btnShuffle.IsEnabled = true;
        }

        public void disableButtons()
        {
            foreach (var child in BtnsContainer.Children)
            {
                Button button = (Button)child;
                button.IsEnabled = false;
            }
        }

        public void enableButtons()
        {
            foreach (var child in BtnsContainer.Children)
            {
                Button button = (Button)child;
                button.IsEnabled = true;
            }
        }


    }
}
