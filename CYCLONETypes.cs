namespace CYCLONE
{
	public enum MainType
	{
		MAIN
	}

	public enum NetworkType
	{
		COMBI,
		NORMAL,
		QUEUE,
		FUNCTION_CONSOLIDATE,
		FUNCTION_COUNTER
	}

	public enum DurationType
	{
		STATIONARY,
		NST
	}

	public enum DistributionType
	{
		DETERMINISTIC,
		EXPONENTIAL,
		UNIFORM,
		NORMAL,
		TRIANGULAR,
		LOGNORMAL,
		BETA
	}

	public enum ReferenceType
	{
		REF_QUEUE,
		REF_COMBI,
		REF_NORMAL,
		REF_FUNCTION
	}
}
