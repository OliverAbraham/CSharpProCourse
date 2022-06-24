using System;

namespace BewerberAufgabenUndFragen
{
class WhatAreTheProblemHere
{
	private void Question1()
	{
		try
		{
			DoSomething();
		}
		catch (Exception e)
		{
			throw new Exception(e);
		}
	}

	private void Question2()
	{
		try
		{
			DoSomething();
		}
		catch (Exception)
		{
			throw;
		}
		catch
		{
			throw;
		}
	}

	private void Question3()
	{
		try
		{
			DoSomething();
		}
		catch (Exception)
		{
		}
	}
}
