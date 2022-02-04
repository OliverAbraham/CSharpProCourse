// See https://aka.ms/new-console-template for more information
Console.WriteLine("Percentage display algorithm");

// we will do some longer work
int totalItemCount = 250;
for (int i=1; i<totalItemCount; i++)
{
	DoSomeWork(i);
}

void DoSomeWork(int i)
{
	Thread.Sleep(100);
	int percent = CalculatePercentage(i, totalItemCount);
	Console.WriteLine($"Done No.{i}  {percent} % done.");
}



int CalculatePercentage(int currentItemCount, int totalItemCount)
{
    float percentage = 100 * ((float)currentItemCount / (float)totalItemCount);
	return (int)percentage;
}