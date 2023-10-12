using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Match_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;


        int rightPicks = 0;
        int wrongPicks = 0;

        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();
            timer.Interval = TimeSpan.FromSeconds(.01);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10f).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                if (int.Parse(timeTextBlock.Text) < int.Parse(bestTimeTextBlock.Text)) 
                { 
                 bestTimeTextBlock.Text = "High Score:" + timeTextBlock.Text; 
                }
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> FoodEmoji = new List<string>()
            {
                "🍕","🍕",
                "🍔","🍔",
                "🍟","🍟",
                "🌭","🌭",
                "🍿","🍿",
                "🥞","🥞",
                "🥪","🥪",
                "🌯","🌯",
            };

            Random random = new Random();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock" && textBlock.Name != "textRight" && textBlock.Name != "textWrong" && textBlock.Name != "textPercent" && textBlock.Name != "bestTimeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(FoodEmoji.Count);
                    string nextEmoji = FoodEmoji[index];
                    textBlock.Text = nextEmoji;
                    FoodEmoji.RemoveAt(index);

                } 
            }
                timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            // Not looking for match yet
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
                Debug.WriteLine("MatchFound");
            }
            // Looking for match and found one
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
                rightPicks++;
                updateStats();
            }
            // Looking for match, didn't find one
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
                wrongPicks++;
                updateStats();
            }
        }

        private void updateStats() 
        { 
            textRight.Text = rightPicks.ToString();
            textWrong.Text = wrongPicks.ToString();
            float p = (float)rightPicks / (float)(rightPicks + wrongPicks);
            textPercent.Text = p.ToString();
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
