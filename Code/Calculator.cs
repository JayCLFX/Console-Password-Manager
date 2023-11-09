using System;

namespace Password_Manager
{
    public class Calculator : Code
    {
        public static void Calculate(double Number1, double Number2, out double result, out double Number1_display, out double Number2_display, out char op_display)
        {
            result = 0; Number1_display = 0; Number2_display = 0; op_display = 'X';
            char op;
            Console.Clear();
            Console.WriteLine("Number 1: \n"); if (Double.TryParse(Console.ReadLine(), out Number1) == false) return;
            Console.Clear();
            Console.WriteLine("Number 2: \n"); if (Double.TryParse(Console.ReadLine(), out Number2) == false) return;
            Console.Clear();
            Console.WriteLine("Operator"); char.TryParse(Console.ReadLine(), out op); bool valid = !Char.IsLetter(op); if (valid == false) return;
            Console.Clear();
            result = op switch { '+' => Number1 + Number2, '-' => Number1 - Number2, '*' => Number1 * Number2, '/' => Number1 / Number2, _ => throw new NotImplementedException() };
            op_display = op; Number1_display = Number1; Number2_display = Number2;
            return;
        }
    }
}
