using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackRechner
{
    public class Rechner
    {
        private Stack<string> stack = new Stack<string>();

        public void Push(string value)
        {
            stack.Push(value);
        }

        public string Berechne()
        {
            while (stack.Count > 0)
            {
                string element = stack.Pop(); // = *
                if ("+-".Contains(element[0]))
                {
                    string operand1  = Hole_Operand1();
                    string operand2  = Hole_Operand1();
                    string ergebnis = Berechne_ergebnis(operand1, operand2, element);
                    stack.Push(ergebnis);
                }
                else if ("*/".Contains(element[0]))
                {
                    string operand1  = Hole_Operand2();
                    string operand2  = Hole_Operand2();
                    string ergebnis = Berechne_ergebnis(operand1, operand2, element);
                    
                    if (stack.Count == 0)
                    {
                        stack.Push(ergebnis);
                    }
                    else
                    {
                        string top_element = stack.Peek();
                        if ("+-*/".Contains(top_element[0]))
                        {
                            top_element = stack.Pop();
                            stack.Push(ergebnis);
                            stack.Push(top_element);
                        }
                        else
                        {
                            stack.Push(ergebnis);
                        }
                    }
                }
                else
                {
                     return element;
                }
            }
            return "0";
        }

        private string Berechne_ergebnis(string operand1, string operand2, string @operator)
        {
            switch (@operator[0])
            {
                case '+': return (Double.Parse(operand1) + Double.Parse(operand2)).ToString();
                case '-': return (Double.Parse(operand1) - Double.Parse(operand2)).ToString();
                case '*': return (Double.Parse(operand1) * Double.Parse(operand2)).ToString();
                case '/': return (Double.Parse(operand1) / Double.Parse(operand2)).ToString();
                default: throw new Exception();
            }
        }

        private string Hole_Operand1()
        {
            while (stack.Count > 0)
            {
                string element = stack.Pop();
                if (element.Length > 0)
                {
                    if (char.IsDigit(element[0]))
                        return element;
                    else
                    {
                        stack.Push(element);
                        return Berechne();
                        //string element2 = stack.Pop();
                        //stack.Push(element);
                        //return element2;
                    }
                }
            }
            return "0";
        }

        private string Hole_Operand2()
        {
            while (stack.Count > 0)
            {
                string element = stack.Pop();
                if (element.Length > 0)
                {
                    if (char.IsDigit(element[0]))
                        return element;
                    else
                    {
                        string element2 = stack.Pop();
                        stack.Push(element);
                        return element2;
                    }
                }
            }
            return "0";
        }
    }
}
