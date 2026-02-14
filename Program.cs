using ATMApp.View;

using System;

public class Program
{
    public static void Main(string[] args)
    {
        BankingView.Run();
    }
}

public class BankingService
{
    private double lastTransactionAmount = 0.0;

    // Pass-by-value
    public double CheckBalance(double balance)
    {
        return balance;
    }

    // Deposit using ref
    public bool Deposit(ref double balance, double amount)
    {
        if (amount <= 0)
            return false;

        balance += amount;
        lastTransactionAmount = amount;
        return true;
    }

    // Withdraw using ref + out
    public void Withdraw(ref double balance, double amount, out bool success)
    {
        if (amount <= 0)
        {
            success = false;
            return;
        }

        if (amount > balance)
        {
            success = false;
            return;
        }

        balance -= amount;
        lastTransactionAmount = -amount;
        success = true;
    }

    // Mini Statement (pass-by-value)
    public string GetMiniStatement(double balance)
    {
        return "--- Mini Statement ---\n" +
               $"Current Balance: {balance:F2}\n" +
               $"Last Transaction Amount: {lastTransactionAmount:F2}";
    }
}

public class BankingView
{
    public static void Run()   // ✅ FIXED HERE
    {
        BankingService service = new BankingService();
        double balance = 1000.00;

        Console.WriteLine("Izzy Broniola");
        Console.WriteLine("=== Simple ATM System ===");
        Console.WriteLine($"Initial Balance: {balance:F2}\n");

        while (true)
        {
            Console.WriteLine("1: Check Balance");
            Console.WriteLine("2: Deposit Money");
            Console.WriteLine("3: Withdraw Money");
            Console.WriteLine("4: Print Mini Statement");
            Console.WriteLine("5: Exit");
            Console.Write("Select option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine($"Current Balance: {service.CheckBalance(balance):F2}\n");
                    break;

                case "2":
                    Console.Write("Enter amount to deposit: ");
                    if (!double.TryParse(Console.ReadLine(), out double depositAmount))
                    {
                        Console.WriteLine("Invalid deposit amount. Please enter a positive value.\n");
                        continue;
                    }

                    if (!service.Deposit(ref balance, depositAmount))
                    {
                        Console.WriteLine("Invalid deposit amount. Please enter a positive value.\n");
                        continue;
                    }

                    Console.WriteLine("Deposit successful.");
                    Console.WriteLine($"Updated Balance: {balance:F2}\n");
                    break;

                case "3":
                    Console.Write("Enter amount to withdraw: ");
                    if (!double.TryParse(Console.ReadLine(), out double withdrawAmount))
                    {
                        Console.WriteLine("Invalid withdrawal amount. Please enter a positive value.\n");
                        continue;
                    }

                    service.Withdraw(ref balance, withdrawAmount, out bool success);

                    if (!success)
                    {
                        if (withdrawAmount <= 0)
                            Console.WriteLine("Invalid withdrawal amount. Please enter a positive value.\n");
                        else
                            Console.WriteLine("Withdrawal failed. Insufficient balance.\n");

                        continue;
                    }

                    Console.WriteLine("Withdrawal successful.");
                    Console.WriteLine($"Updated Balance: {balance:F2}\n");
                    break;

                case "4":
                    Console.WriteLine(service.GetMiniStatement(balance));
                    Console.WriteLine();
                    break;

                case "5":
                    Console.WriteLine("Thank you for using the ATM. Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid option selected. Please try again.\n");
                    continue;
            }

            if (choice == "5")
                break;
        }
    }
}
