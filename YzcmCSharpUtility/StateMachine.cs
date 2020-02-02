using System.Threading.Tasks;
using System.Threading;

namespace YzcmCSharpUtility.DesignPattern
{
    public interface IState<StateContextT>
    {
        public IState<StateContextT> Execute(StateContextT context) { return null; }
        public Task<IState<StateContextT>> ExecuteAsync(StateContextT context) { return null; }
    }
    public class StateMachine<StateContextT>
    {
        public StateMachine(IState<StateContextT> startupState) : this(startupState, null) { }
        public StateMachine(IState<StateContextT> startupState, IState<StateContextT> teardownState)
        {
            this.startupState = startupState;
            this.teardownState = teardownState;
        }
        public async void Run(StateContextT context)
        {
            var nextState = this.startupState;
            while(nextState != null)
            {
                IState<StateContextT> result = null;
                IState<StateContextT> resultAsync = null;

                result = nextState.Execute(context);
                if(result == null)
                {
                    var t = nextState.ExecuteAsync(context);
                    if(t == null)
                    {
                        resultAsync = null;
                    }
                    else
                    {
                        if(SynchronizationContext.Current != null)
                        {
                            resultAsync = await t;
                        }
                        else
                        {
                            resultAsync = t.Result;
                        }
                    }
                }

                if (result == null && resultAsync == null)
                {
                    break;
                }

                nextState = result != null ? result : resultAsync;
            }

            this.teardownState?.Execute(context);
        }
        private readonly IState<StateContextT> startupState;
        private readonly IState<StateContextT> teardownState;
    }
}
