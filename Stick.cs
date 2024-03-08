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
namespace Towers_of_Hanoi
{
    //Class for creating stacks (Sticks)
    public class Stick
    {
        private List<int> stack = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0};
        public int pointer=0;
        public bool IsEnabled;//Do want IsEnabled to be seen and changed
        public Stick(bool enabled)//Class Constructer
        {
            IsEnabled = enabled;
        }
        public int Push(int data)//Pushes an item onto the top of the stack
        {
            if (pointer >= stack.Count)//Returns -1 if stack full
            {
                Debug.WriteLine("Stack is full!");
                return -1;
            }
            else if (data > Top()&Top()!=-20)
            {
                Debug.WriteLine("Disc too large!");
                return -1;
            }
            else
            {
                stack[pointer] = data;
                pointer++;
                Debug.WriteLine("Pushed: " + data);
                return 0;
            }
        }
        public int Pop()//Pops an item off the top of the stack
        {
            if (pointer == 0)//Returns -1 if stack empty
            {
                Debug.WriteLine("Stack is empty!");
                return -1;
            }
            else
            {
                pointer--;
                int data = stack[pointer];
                stack[pointer] = 0;
                Debug.WriteLine("Popped: " + data);
                return data;
            }
        }
        public int Top()//Gets the top item from the stack but doesnt remove it from the stack
        {
            if (pointer == 0)//Returns -20 if stack empty
            {
                Debug.WriteLine("Stack is empty");
                return -20;
            }
            else
            {
                Debug.WriteLine("Topped: " + stack[pointer - 1]);
                return stack[pointer - 1];
            }
        }
    }
}