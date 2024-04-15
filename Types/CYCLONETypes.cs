namespace CYCLONE.Types
{
    /// <summary>
    /// Represents the Main Block Type of the CYCLONE model.
    /// </summary>
    public enum MainType
    {
        /// <summary>
        /// The Main Block Type.
        /// </summary>
        MAIN,
    }

    /// <summary>
    /// Represents the Network Type of the CYCLONE model.
    /// </summary>
    public enum NetworkType
    {
        /// <summary>
        /// The Combi Network Type.
        /// </summary>
        COMBI,

        /// <summary>
        /// The Normal Network Type.
        /// </summary>
        NORMAL,

        /// <summary>
        /// The Queue Network Type.
        /// </summary>
        QUEUE,

        /// <summary>
        /// The Function Consolidate Network Type.
        /// </summary>
        FUNCTION_CONSOLIDATE,

        /// <summary>
        /// The Function Counter Network Type.
        /// </summary>
        FUNCTION_COUNTER,
    }

    /// <summary>
    /// Represents the Resource Type of the CYCLONE model.
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// The Resource Type.
        /// </summary>
        RESOURCE,
    }

    /// <summary>
    /// Represents the Duration Input Type of the CYCLONE model.
    /// </summary>
    public enum DurationType
    {
        /// <summary>
        /// The Staionary Duration Type.
        /// </summary>
        STATIONARY,

        /// <summary>
        /// The Non-Staionary Duration Type.
        /// </summary>
        NST,
    }

    /// <summary>
    /// Represents the Non-Staionary Category of the CYCLONE model.
    /// </summary>
    public enum NSTCategory
    {
        /// <summary>
        /// The First Non-Staionary Category.
        /// </summary>
        FIRST,

        /// <summary>
        /// The Second Non-Staionary Category.
        /// </summary>
        SECOND,
    }

    /// <summary>
    /// Represents the Distribution Type of the CYCLONE model.
    /// </summary>
    public enum DistributionType
    {
        /// <summary>
        /// The Deterministic Distribution Type. (Constant)
        /// </summary>
        DETERMINISTIC,

        /// <summary>
        /// The Exponential Distribution Type.
        /// </summary>
        EXPONENTIAL,

        /// <summary>
        /// The Uniform Distribution Type.
        /// </summary>
        UNIFORM,

        /// <summary>
        /// The Normal Distribution Type.
        /// </summary>
        NORMAL,

        /// <summary>
        /// The Triangular Distribution Type.
        /// </summary>
        TRIANGULAR,

        /// <summary>
        /// The Lognormal Distribution Type.
        /// </summary>
        LOGNORMAL,

        /// <summary>
        /// The Beta Distribution Type.
        /// </summary>
        BETA,
    }

    /// <summary>
    /// Represents the Reference Label Type of the CYCLONE model.
    /// </summary>
    public enum ReferenceType
    {
        /// <summary>
        /// The Reference Queue Label Type.
        /// </summary>
        REF_QUEUE,

        /// <summary>
        /// The Reference Combi Label Type.
        /// </summary>
        REF_COMBI,

        /// <summary>
        /// The Reference Normal Label Type.
        /// </summary>
        REF_NORMAL,

        /// <summary>
        /// The Reference Function Label Type. (Either Consolidate or Counter)
        /// </summary>
        REF_FUNCTION,
    }

    /// <summary>
    /// Represents the Resource Cost Type of the CYCLONE model.
    /// </summary>
    public enum ResourceCostType
    {
        /// <summary>
        /// The Variable Cost Type.
        /// </summary>
        VC,

        /// <summary>
        /// The Fixed Cost Type.
        /// </summary>
        FC,
    }
}
