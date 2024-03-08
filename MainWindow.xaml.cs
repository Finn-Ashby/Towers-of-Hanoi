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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Threading;
namespace Towers_of_Hanoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<double> index = new List<double> { 133, 258, 408, 558, 708, 10000 };
        double interval;
        bool ai = false;
        Point canvasRelativePosition;
        int numMoves = 0;
        bool playing = false;
        int f;
        int whatMove = 0;
        Button draggedItem;
        bool IsDragging;
        List<Stick> sticks = new List<Stick>();
        List<Button> btns = new List<Button>();
        int numSticks = 3;
        int numDiscs = 3;
        double r = 3;
        double g = 29;
        double b = 93;
        double num = 5;
        const double aaad = 34.7142857143;
        bool even;
        SoundPlayer win = new SoundPlayer((Stream)Properties.Resources.YouWin);
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer tmrRain = new DispatcherTimer(); //Starts the timer
            tmrRain.Interval = TimeSpan.FromMilliseconds(100); //Interval of 100 milliseconds 
            tmrRain.Tick += TmrRain_Tick;
            DispatcherTimer tmrAI = new DispatcherTimer();
            tmrAI.Interval = TimeSpan.FromMilliseconds(10); //Interval of 10 milliseconds 
            tmrAI.Tick += TmrAI_Tick;
            tmrAI.Start();
            tmrRain.Start();
            //Adds all buttons to a list
            btns.Add(btn0);
            btns.Add(btn1);
            btns.Add(btn2);
            btns.Add(btn3);
            btns.Add(btn4);
            btns.Add(btn5);
            btns.Add(btn6);
            btns.Add(btn7);
            btns.Add(btn8);
            //Creates the sticks and resizes the window
            sticks = Create(numSticks, numDiscs);
            Resize(numSticks);
            //pushing(numDiscs, 0);
            Debug.WriteLine(sticks[0].Top());
            foreach (Button btn in btns)
            {
                btn.Content = "0";
            }
            //Snap(1, btn0,sticks[Int32.Parse(btn0.Content.ToString())]);
            //sticks[0].;
            //5*(pointer-1)-8
            //Grid.SetColumn(btn0, 1);
        }
        
        //When mouseover a disc
        public void btn_mouseover(object sender, MouseEventArgs e)
        {
            Button btnOver = (Button)sender;
            int f = -1;

            for (int i = 0; i < btns.Count; i++)
            {
                if (btnOver == btns[i])
                {
                    f = 10 - i;
                    break;
                }
            }
            Debug.WriteLine("F: " + f + " Top: " + sticks[Int32.Parse(btnOver.Content.ToString())].Top());
            //Checks if the stick is on the top of the stack and game is being played
            if (sticks[Int32.Parse(btnOver.Content.ToString())].Top() == f&playing)
            {
                //Reduces opacity of disc
                btnOver.Opacity = 0.92;
            }
        }
        //Updates the number of moves
        public void changeMoves(int moves)
        {
            txtMoves.Text = "Moves:\n" + moves.ToString();
        }
        public void btn_mouseleave(object sender, MouseEventArgs e)
        {
            //Returns the opacity of the disc back to 1
            Button btnOver = (Button)sender;
            btnOver.Opacity = 1;
        }

        //When a discs is selected and clicked again Snap is called
        private Button Snap(int coll, Button btn, int pointer)
        {
            btn.Content = coll.ToString();//Changes the buttons text to the collumn its in
            Thickness x = btn.Margin;
            //Snaps the disc to the nearest collumn
            x.Left = 47.5 + coll * 148 - btn.Width / 2 - coll + 9;
            x.Top = 338 - pointer * aaad + 35;
            btn.Margin = x;
            Debug.WriteLine("Margin: " + x);
            //Returns the button with the new margin
            return btn;
        }
        //Resises window based on number of sticks
        private void Resize(int numOfSticks)
        {
            Application.Current.MainWindow.Width = numOfSticks * 136 + 275;
            //Enables first num sticks; rest are disabled
            for (int i = 0; i < sticks.Count; i++)
            {
                if (i < numOfSticks)
                {
                    sticks[i].IsEnabled = true;
                }
                else
                {
                    sticks[i].IsEnabled = false;
                }
            }
            return;
        }
        //Function to create a list of sticks
        public List<Stick> Create(int numOfSticks, int numOfBtns)
        {
            List<Stick> stickss = new List<Stick>();
            for (int i = 0; i < numOfSticks; i++)//Creates a list of sticks
            {
                stickss.Add(new Stick(false));
            }
            //Disables every stick except first 3
            stickss = enableSticksnButtons(3, numDiscs, stickss);
            Debug.WriteLine("Top: " + stickss[0].Top().ToString());
            return stickss;
        }
        //Adds a certain number of discs to a stick; used when initialising the game
        public void pushing(int numDiscs, int stick)
        {
            for (int i = 0; i < numDiscs; i++)
            {
                sticks[stick].Push(10 - i);
            }
            
        }
        //Enables the first x number of buttons and y number sticks
        public List<Stick> enableSticksnButtons(int stics, int discs, List<Stick> stick)
        {
            for (int i = 0; i < btns.Count; i++)
            {
                if (i < discs)
                {
                    btns[i].IsEnabled = true;
                }
                else
                {
                    btns[i].IsEnabled = false;
                }
            }
            return stick;
        }
        //Moves the window if the top border is held down
        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        //Timer for making the background gradient
        private void TmrRain_Tick(object sender, EventArgs e)
        {
            r += num;
            g += -(num * 29) / 128;
            b += num * (35 / 128);
            if (r > 60 | r <= 2)
            {
                num = num * -1;
            }
            Color col = new Color();
            Color col2 = new Color();
            col = Color.FromRgb((byte)Convert.ToInt32(r), (byte)Convert.ToInt32(g), (byte)Convert.ToInt32(b));
            col2 = Color.FromRgb((byte)Convert.ToInt32(r + 60), (byte)Convert.ToInt32(g - 10), (byte)Convert.ToInt32(b + 50));

            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush();
            myHorizontalGradient.StartPoint = new Point(0, 0);
            myHorizontalGradient.EndPoint = new Point(1, 1);

            myHorizontalGradient.GradientStops.Add(
                new GradientStop(col, 0.0));
            myHorizontalGradient.GradientStops.Add(
                new GradientStop(col2, 1.0));

            grid.Background = myHorizontalGradient;
        }
        //When a disc is pressed
        private void btn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine(playing);
            //If the game is being played by the user
            if (playing)
            {
                Button btnClicked = (Button)sender;
                //If the button is already being dragged
                if (IsDragging)
                {
                    if (draggedItem != btnClicked)
                    {
                        return;
                    }
                    int checky = 1;
                    IsDragging = false;
                    Thickness canvasRelativePosition = btnClicked.Margin;
                    //CHECKY CHECK STUFF
                    //Finds what stick/collumn to put the disc on
                    for (int i = 0; i < index.Count; i++)
                    {
                        if ((canvasRelativePosition.Left + btnClicked.Width / 2) < index[i])
                        {
                            checky = i;
                            break;
                        }
                    }
                    //If the mouse is off the sceen
                    if (checky > 8)
                    {
                        checky = 8;
                    }
                    while (true)
                    {
                        if (!sticks[checky].IsEnabled)
                        {
                            checky--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //If the disc selected is smaller than the discs on the top of the stack it is being added to:
                    if (sticks[checky].Push(sticks[Int32.Parse(btnClicked.Content.ToString())].Top()) == 0)
                    {
                        Debug.WriteLine(sticks[Int32.Parse(btnClicked.Content.ToString())].Pop());
                        //If the collumn is different to the one it was originally on:
                        if (btnClicked.Content.ToString() != checky.ToString())
                        {
                            numMoves++;
                            changeMoves(numMoves);
                            WinCheck();
                            btnClicked.Content = checky;
                        }
                        btnClicked = Snap(checky, btnClicked, sticks[checky].pointer);
                    }
                    else
                    {
                        //Snap the disc back to where it was originally
                        btnClicked = Snap(Int32.Parse(btnClicked.Content.ToString()), btnClicked, sticks[Int32.Parse(btnClicked.Content.ToString())].pointer);
                    }
                    return;
                }
                //Else if the button clicked isnt being dragged
                f = -1;
                //Gets the value of the button pressed
                for (int i = 0; i < btns.Count; i++)
                {
                    if (btnClicked == btns[i])
                    {
                        f = 10 - i;
                        break;
                    }
                }
                Debug.WriteLine("F: " + f + " Top: " + sticks[Int32.Parse(btnClicked.Content.ToString())].Top());
                //If the top of the stack is the button pressed
                if (sticks[Int32.Parse(btnClicked.Content.ToString())].Top() == f)
                {
                    Debug.WriteLine("Dragging");
                    IsDragging = true;
                    draggedItem = (Button)sender;
                }
            }
        }
        //Checks for win
        private bool WinCheck()
        {
            //If the last sticks pointer = the number of discs:
            if (sticks[numSticks - 1].pointer == numDiscs)
            {
                //Stops playing and returns true
                win.Play();
                Debug.WriteLine("WIN!");
                ai = false;
                playing = false;
                return true;
            }
            return false;
        }
        //If the mouse moves in the grid
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            //If a button isnt being dragged:
            if (!IsDragging)
            {
                return;
            }
            //Else:
            
            //Changes the discs position to be the mouse position
            canvasRelativePosition = e.GetPosition(grid0);
            Thickness x = draggedItem.Margin;
            x.Left = canvasRelativePosition.X - draggedItem.Width / 2;
            x.Top = canvasRelativePosition.Y - 17.5;
            draggedItem.Margin = x;
        }
        //Closes the form down
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            //Sets the number of moves to 0
            numMoves = 0;
            changeMoves(numMoves);
            playing = true;
            ai = false;
            //Creates the discs and sticks
            numDiscs = Convert.ToInt16(sldNumDiscs.Value);
            numSticks = Convert.ToInt16(sldNumSticks.Value);
            sticks = Create(9, numDiscs);
            //Resises the screen and adds the discs
            Resize(numSticks);
            pushing(numDiscs, 0);
            if ((bool)tckAI.IsChecked)
            {
                //disables the user from being able to click on and move the discs
                btnPlay.IsEnabled = false;
                ai = true;
                playing = false;
                //If even number of discs uses even algorithm
                if (numDiscs % 2 == 0)
                {
                    even = true;
                }
                //Else use odd algorithm
                else
                {
                    even = false;
                }
            }
            //Sets the correct margin for all the discs
            for (int i = 0; i < btns.Count; i++)
            {
                btns[i].Content = "0";
                Thickness x = btns[i].Margin;
                x.Left = 5 * i;
                x.Top = 338 - 35 * i;
                btns[i].Margin = x;
            }
        }
        //Changes how much the discs are moved by when the slider value changes
        private void sldSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            interval = sldSpeed.Value;
        }
        private void TmrAI_Tick(object sender, EventArgs e)
        {
            if (ai)
            {
                //Remainder when moves%3 to get what move to do
                //Moves the button towards the new location
                //Pop and push disc once positions equal to each other
                //Add 1 to moves
                //Move(btn0, 145, 60,interval);
                whatMove = numMoves % 3;
                
                if (even)
                {
                    Debug.WriteLine("Whatmove: " + whatMove);
                    switch (whatMove)
                    {
                        case 2:
                            Debug.WriteLine("case 2");
                            if (sticks[1].Top() != -20 & sticks[1].Top() < sticks[numSticks - 1].Top() | sticks[numSticks-1].Top() == -20)//B to C B<C  sticks[0].Top() < sticks[numSticks - 1].Top()
                            {
                                double lefft = 47.5 + (numSticks-1) * 148 - btns[10 - sticks[1].Top()].Width / 2 -(numSticks-1)+9;//1010
                                double topp = 338 - (sticks[numSticks-1].pointer) * aaad;
                                if(Move(btns[10-sticks[1].Top()],lefft,topp,interval))
                                {
                                    sticks[numSticks-1].Push(sticks[1].Pop());
                                    numMoves++;
                                }
                            }
                            else if (sticks[numSticks-1].Top() != -20 & sticks[1].Top() > sticks[numSticks - 1].Top() | sticks[1].Top() == -20) //C to B
                            {
                                double lefft = 47.5 + (1) * 148 - btns[10 - sticks[numSticks-1].Top()].Width / 2 -1+9;//1010
                                double topp = 338 - (sticks[1].pointer) * aaad;
                                if (Move(btns[10 - sticks[numSticks-1].Top()], lefft, topp, interval))
                                {
                                    sticks[1].Push(sticks[numSticks - 1].Pop());
                                    numMoves++;
                                }
                            }
                            //B C
                            break;
                        case 1:
                            Debug.WriteLine("case 1");
                            if (sticks[0].Top() != -20 & sticks[0].Top() < sticks[numSticks - 1].Top() | sticks[numSticks-1].Top() == -20)//A to C A<C  sticks[0].Top() < sticks[numSticks - 1].Top()
                            {
                                double lefft = 47.5 + (numSticks - 1) * 148 - btns[10 - sticks[0].Top()].Width / 2 - (numSticks - 1)+9; //101010
                                double topp = 338 - (sticks[numSticks - 1].pointer) * aaad;
                                if (Move(btns[10 - sticks[0].Top()], lefft, topp, interval))
                                {
                                    sticks[numSticks-1].Push(sticks[0].Pop());
                                    numMoves++;
                                }
                            }
                            else if (sticks[numSticks-1].Top() != -20 & sticks[0].Top() > sticks[numSticks - 1].Top() | sticks[0].Top() == -20) //C to A
                            {
                                double lefft = 47.5 + (0) * 148 - btns[10 - sticks[numSticks - 1].Top()].Width / 2 + 9;//101010
                                double topp = 338 - (sticks[0].pointer) * aaad;
                                if (Move(btns[10 - sticks[numSticks-1].Top()], lefft, topp, interval))
                                {
                                    sticks[0].Push(sticks[numSticks-1].Pop());
                                    numMoves++;
                                }
                            }
                            //A C
                            break;
                        default:
                            Debug.WriteLine("case 0");
                            if (sticks[0].Top()!=-20&sticks[0].Top() < sticks[1].Top()|sticks[1].Top()==-20)//A to B 
                            {
                                Debug.WriteLine("A to b");
                                Debug.WriteLine("10-sticks[0].Top(): " +( 10 - sticks[0].Top()));
                                double lefft = 47.5 + (1) * 148 - btns[10-sticks[0].Top()].Width / 2 -1+9;//101010
                                double topp = 338 - (sticks[1].pointer ) * aaad ;
                                if (Move(btns[10 - sticks[0].Top()], lefft, topp, interval))
                                {
                                    sticks[1].Push(sticks[0].Pop());
                                    numMoves++;

                                }
                            }
                            else if(sticks[1].Top() != -20 & sticks[0].Top()>sticks[1].Top()|sticks[0].Top() == -20)//B to A
                            {
                                Debug.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaggvgygh");
                                double lefft = 47.5 + (0) * 148 - btns[10 - sticks[1].Top()].Width / 2 + 9;//101010
                                double topp = 338 - (sticks[0].pointer) * aaad;
                                if (Move(btns[10 - sticks[1].Top()], lefft, topp, interval))
                                {
                                    sticks[0].Push(sticks[1].Pop());
                                    numMoves++;
                                }
                            }
                            //A B
                            break;
                    }
                }
                else
                {
                    switch (whatMove)
                    {
                        case 2:
                            if (sticks[1].Top() != -20 & sticks[1].Top() < sticks[numSticks - 1].Top() | sticks[numSticks - 1].Top() == -20)//B to C B<C  sticks[0].Top() < sticks[numSticks - 1].Top()
                            {
                                //Left=  
                                double lefft = 47.5 + (numSticks - 1) * 148 - btns[10 - sticks[1].Top()].Width / 2 - (numSticks - 1)+9;//1010
                                double topp = 338 - (sticks[numSticks - 1].pointer) * aaad;
                                if (Move(btns[10 - sticks[1].Top()], lefft, topp, interval))
                                {
                                    sticks[numSticks - 1].Push(sticks[1].Pop());
                                    numMoves++;
                                }
                            }
                            else if (sticks[numSticks - 1].Top() != -20 & sticks[1].Top() > sticks[numSticks - 1].Top() | sticks[1].Top() == -20) //C to B
                            {
                                double lefft = 47.5 + (1) * 148 - btns[10 - sticks[numSticks - 1].Top()].Width / 2 -1+9;//1010
                                double topp = 338 - (sticks[1].pointer) * aaad;
                                if (Move(btns[10 - sticks[numSticks - 1].Top()], lefft, topp, interval))
                                {
                                    sticks[1].Push(sticks[numSticks - 1].Pop());
                                    numMoves++;
                                }
                            }
                            //B C
                            break;
                        case 1:
                            if (sticks[0].Top() != -20 & sticks[0].Top() < sticks[1].Top() | sticks[1].Top() == -20)//A to B 
                            {
                                double lefft = 47.5 + (1) * 148 - btns[10 - sticks[0].Top()].Width / 2 + -1 + 9;//101010
                                double topp = 338 - (sticks[1].pointer) * aaad;
                                if (Move(btns[10 - sticks[0].Top()], lefft, topp, interval))
                                {
                                    sticks[1].Push(sticks[0].Pop());
                                    numMoves++;

                                }
                            }
                            else if (sticks[1].Top() != -20 & sticks[0].Top() > sticks[1].Top() | sticks[0].Top() == -20)//B to A
                            {
                                double lefft = 47.5 + (0) * 148 - btns[10 - sticks[1].Top()].Width / 2+9;//101010
                                double topp = 338 - (sticks[0].pointer) * aaad;
                                if (Move(btns[10 - sticks[1].Top()], lefft, topp, interval))
                                {
                                    sticks[0].Push(sticks[1].Pop());
                                    numMoves++;
                                }
                            }
                            //A C
                            break;
                        default:
                            if (sticks[0].Top() != -20 & sticks[0].Top() < sticks[numSticks - 1].Top() | sticks[numSticks - 1].Top() == -20)//A to C A<C  sticks[0].Top() < sticks[numSticks - 1].Top()
                            {
                                //Left=  
                                double lefft = 47.5 + (numSticks - 1) * 148 - btns[10 - sticks[0].Top()].Width / 2- (numSticks - 1)+9; //101010
                                double topp = 338 - (sticks[numSticks - 1].pointer) * aaad;
                                if (Move(btns[10 - sticks[0].Top()], lefft, topp, interval))
                                {
                                    sticks[numSticks - 1].Push(sticks[0].Pop());
                                    numMoves++;
                                }
                            }
                            else if (sticks[numSticks - 1].Top() != -20 & sticks[0].Top() > sticks[numSticks - 1].Top() | sticks[0].Top() == -20) //C to A
                            {
                                double lefft = 47.5 + (0) * 148 - btns[10 - sticks[numSticks - 1].Top()].Width / 2+9;//101010
                                double topp = 338 - (sticks[0].pointer) * aaad;
                                if (Move(btns[10 - sticks[numSticks - 1].Top()], lefft, topp, interval))
                                {
                                    sticks[0].Push(sticks[numSticks - 1].Pop());
                                    numMoves++;
                                }
                            }
                            //A C
                            //A B
                            break;
                    }
                }
                //Updates the number of moves
                changeMoves(numMoves);
                //If the algorithm is completed
                if (WinCheck())
                {
                    btnPlay.IsEnabled = true;
                    ai = false;
                    playing = false;
                }
            }
        }
        //Moves the discs
        private bool Move(Button btn,double left,double top,double by)
        {
            Thickness x = btn.Margin;
            if (btn.Margin.Left == left|(btn.Margin.Left+by>left&btn.Margin.Left-by<left))//If the disc goes further than the point it is going to
            {
                x.Left = left;
                x.Top = top;
                btn.Margin= x;
                //Returns true when the discs is at the desired point
                return true;
            }
            //Works out the distance and direction 
            double distance = Math.Sqrt(Math.Pow(left - x.Left, 2) + Math.Pow(top - x.Top, 2));
            double directionX = (left - x.Left) / distance;
            double directionY = (top - x.Top) / distance;
            //Moves the disc the direction*the AI speed
            x.Left += directionX * by;
            x.Top += directionY * by;
            btn.Margin = x;
            return false;
        }
    }
}