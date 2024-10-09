using System.Globalization;
using System.Text.RegularExpressions;


namespace ScienceCalc;

class Calculation
{
    public static double Calculate(String input)
    {
        var postfix = Postfix(input);
        Stack<String> actualCalculation = new Stack<String>();
        while(postfix.Count != 0)
        {
            while(double.TryParse(postfix.Peek(), out _))
            {
                actualCalculation.Push(postfix.Pop());
            }

            if (postfix.Count != 0)
            {
                Stack<String> temp = new Stack<String>();
                temp.Push(actualCalculation.Pop());
                temp.Push(postfix.Pop());
                temp.Push(actualCalculation.Pop());
                actualCalculation.Push(TwoNumberStackEvaluation(temp));

            }
            else {}
        }
        return double.Parse(actualCalculation.Pop());
    }

    public static string TwoNumberStackEvaluation(Stack<string> input)
    {
        double firstNum = double.Parse(input.Pop());
        string oper = input.Pop();
        double secondNum = double.Parse(input.Pop());
        string? result = null;
        switch (oper)
        {
            case "+": result = (firstNum + secondNum).ToString();
                break;
            case "-": result = (firstNum - secondNum).ToString();
                break;
            case "*": result = (firstNum * secondNum).ToString();
                break;
            case "/": result = (firstNum / secondNum).ToString();
                break;
            case "^": result = (Math.Pow(firstNum, secondNum)).ToString(CultureInfo.InvariantCulture);
                break;
        }
        return result!;
    }
    public static double AskAndCalculate()
    {
        string ?input = Console.ReadLine();
        double provideresult = 0;
        if (input != null)
        {
            provideresult = Calculate(input);
        }
        else Console.ReadLine();
        return provideresult;
    }
    public static Stack<string> Postfix(String inp)
    {
        Dictionary<String, (int precedence, string associativity)> opDict = new Dictionary<string, (int precedence, string associativity)>();
        opDict.Add("*", (3, "Left"));
        opDict.Add("/", (3, "Left"));
        opDict.Add("-", (2, "Left"));
        opDict.Add("+", (2, "Left"));
        opDict.Add("^", (4, "Right"));
        Stack<String> infix = new Stack<String>(Tokenize(inp));
        Stack<String> output  = new Stack<string>();
        Stack<String> opStack = new Stack<string>();
        foreach (String token in infix)
        {
            if (double.TryParse(token, out _))
            {
                output.Push(token);
            }
            else if (opDict.ContainsKey(token))
            {
                String o2;
                o2 = null;
                if (opStack.Count != 0)
                {
                    o2 = opStack.Peek();
                }
                else
                {
                    o2 = null;
                }
                
                while (opStack.Count > 0 && opDict.ContainsKey(o2) && o2 != "(" && (opDict[o2].precedence > opDict[token].precedence || (opDict[o2].precedence == opDict[token].precedence && opDict[token].associativity == "Left")))
                {
                    output.Push(opStack.Pop());
                }
                opStack.Push(token);
            }
            else if (token == "(")
            {
                opStack.Push(token);
            }
            else if (token == ")")
            {
                bool found = false;
                while (!found)
                {
                    if (opStack.Peek() != "(" && opStack.Count != 0)
                    {
                        output.Push(opStack.Pop());
                    }
                    if (opStack.Peek() == "(")
                    {
                        opStack.Pop();
                        found = true;
                    }

                    if (opStack.Count == 0 && found == false)
                    {
                        Console.WriteLine("Mismatching parenthesis!! pls enter again");
                        AskAndCalculate();
                    }
                }
            }
            
        }

        while (opStack.Count != 0)
        {
            if (opStack.Peek() == "(" || opStack.Peek() == ")")
            {
                Console.WriteLine("Mismatching parenthesis!! pls enter again");
                AskAndCalculate();
            }
            output.Push(opStack.Pop());
        }
        Stack<string> reverseoutput = new Stack<string>(output);
        return reverseoutput;
    }
    
    
    public static Stack<String> Tokenize(string inp)
    {
        var tokenList = new List<String>();
            tokenList = Regex.Split(inp, @"\s*([-+/*)(])\s*")
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList();

        Stack<string> tokenStack = new Stack<string>(tokenList);
        return tokenStack;
    }
}


class Program
{
    static void Main()
    {
        Console.WriteLine(Calculation.AskAndCalculate());
    }
    
}