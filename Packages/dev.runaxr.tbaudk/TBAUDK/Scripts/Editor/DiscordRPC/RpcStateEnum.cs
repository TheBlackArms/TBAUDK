namespace TBAUDK
{
    public static class RpcStateInfo
    {
        public static string StateName(this RpcState state)
        {
            switch (state)
            {
                case RpcState.EDITMODE: return "Modifying";
                case RpcState.PLAYMODE: return "Testing";
                case RpcState.UPLOADPANEL: return "Updating content";
                default: return "Idle";
            }
        }
    }

        public enum RpcState
    {
        EDITMODE = 0,
        PLAYMODE = 1,
        UPLOADPANEL = 2
    }
}
