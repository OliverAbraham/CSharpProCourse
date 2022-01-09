

// ATTENTION: This program is for code review only! It's not meant to be started.
class Program
{
    public class InputException : Exception 
    {
		public string MessageText{ get; set; }
		public InputException()
		{
		}
		public InputException(string messageText)
		{
            MessageText = messageText;
		}
	}


    static void Main(string[] args) 
    { 
    }









    static void Old_style()
    {
        int Result = MyMethod_with_old_error_handling("my input", out string ErrorMessage);
        if (Result == -99) // magic value 1
        {
            Console.WriteLine(ErrorMessage);
        }
        if (Result == -98) // magic value 2
        {
            Console.WriteLine(ErrorMessage);
        }
    }

    static int MyMethod_with_old_error_handling(string input, out string errorMessage)
    {
        if (input.Length > 20)
        {
            errorMessage = "Input too long!";
            return -99;
        }
        else if (input.Length > 40)
        {
            errorMessage = "Input waaaay too long!";
            return -98;
        }
        else
        {
            errorMessage = "";
            ; // .... do something
            return input.Length;
        }
    }















    static void Old_style_Better()
    {
        var Returncode = MyMethod_with_old_better_error_handling("my input", out int Result, out string ErrorMessage);
        if (Returncode == MyMethodErrorCodes.CODE_1) // magic value 1
        {
            Console.WriteLine(ErrorMessage);
        }
        if (Returncode == MyMethodErrorCodes.CODE_2) // magic value 2
        {
            Console.WriteLine(ErrorMessage);
        }
    }

    static MyMethodErrorCodes MyMethod_with_old_better_error_handling(string input, out int result, out string errorMessage)
    {
        if (input.Length > 20)
        {
            errorMessage = "Input too long!";
            result = 0;
            return MyMethodErrorCodes.CODE_1;
        }
        else if (input.Length > 40)
        {
            errorMessage = "Input waaaay too long!";
            result = 0;
            return MyMethodErrorCodes.CODE_2;
        }
        else
        {
            errorMessage = "";
            ; // .... do something
            result = input.Length;
            return 0; // 0 means "ok"
        }
    }

    enum MyMethodErrorCodes
	{
        CODE_1 = -99,
        CODE_2 = -98,
	}













    static void Old_style_with_magic_values()
    {
        int Result2 = Multiply(100, 100);
        if (Result2 == ERROR_CODE_1)
        {
            //some error message
        }
    }

    const int ERROR_CODE_1 = -9999;

    static int Multiply(int val1, int val2)
    {
        if ( /* some error condition here */ false)
            return ERROR_CODE_1;

        return val1 * val2; // what if the result is -9999 ?
    }















    static void New_style()
    {
        try
        {
            MyDialog();
        }
        catch (Exception ex)
        {
            Console.WriteLine("A strange thing has happened. We apologize");
            // send email to provider
        }
    }

    private static void MyDialog()
    {
        try
        {
            MyInputValidator("aaaaaaaaaaaaaaaaaaaaaaa");
        }
        catch (InputException ex)
        {
            Console.WriteLine("Please enter correct values" + ex.ToString() );
        }
    }

    static int MyInputValidator(string input)
    {
        if (input.Length > 20)
            throw new InputException("Input too long!");
            
        if (input.Length > 40)
            throw new InputException("Input waaay too long!");
            
        if (true) // something_strange_happened
            throw new Exception("else error!");

        ; // .... do something
        return input.Length;
    }










    static void The_effort_of_the_new_style()
    {
        try
        {
            My_business_logic();
        }
        catch (Exception ex)
        {
            // one place to handle errors
        }
    }

    private static void My_business_logic()
    {
        MyFunction1();
        MyFunction2();
        MyFunction3();
    }

    static void MyFunction1() { }
    static void MyFunction2() { }
    static void MyFunction3() { }



    static void Before_that_it_looked_like_this()
    {
        try
        {
            int ErrorCode = My_Function1();
            if (ErrorCode != 0)
                ; // do error handling

            ErrorCode = My_Function2();
            if (ErrorCode != 0)
                ; // do error handling

            ErrorCode = My_Function3();
            if (ErrorCode != 0)
                ; // do error handling
        }
        catch (Exception ex)
        {
            // one place to handle errors
        }
    }

    static int My_Function1() { return -99; }
    static int My_Function2() { return -99; }
    static int My_Function3() { return -99; }
}
