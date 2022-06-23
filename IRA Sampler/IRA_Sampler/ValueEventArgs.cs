using System;
using System.Collections.Generic;

namespace IRA_Sampler;

public class ValueEventArgs : EventArgs
{
	private Dictionary<string, int> _smth;

	private double _percent;

	private int _pots;

	private string _comments;

	public Dictionary<string, int> Someth_property => _smth;

	public double percent_property => _percent;

	public string comments_property => _comments;

	public int pots_property => _pots;

	public ValueEventArgs(Dictionary<string, int> smth, double percent, int pots, string comments)
	{
		_smth = smth;
		_pots = pots;
		_percent = percent;
		_comments = comments;
	}
}
