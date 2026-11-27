namespace BusinessLogic.IAModule.AgentsModules
{
    public class AgentTokenUsageTracker
    {
        private readonly object _lock = new();

        public int InputTokens { get; private set; }
        public int OutputTokens { get; private set; }
        public int ModelCalls { get; private set; }

        public void Add(int? inputTokens, int? outputTokens)
        {
            lock (_lock)
            {
                InputTokens += inputTokens ?? 0;
                OutputTokens += outputTokens ?? 0;
                ModelCalls++;
            }
        }

        public int TotalTokens =>
            InputTokens + OutputTokens;

        public CAPA_NEGOCIO.AgentTokenUsage GetSummary()
        {
            lock (_lock)
            {
                return new CAPA_NEGOCIO.AgentTokenUsage
                {
                    InputTokens = InputTokens,
                    OutputTokens = OutputTokens,
                    TotalTokens = InputTokens + OutputTokens,
                    ModelCalls = ModelCalls
                };
            }
        }
    }
}