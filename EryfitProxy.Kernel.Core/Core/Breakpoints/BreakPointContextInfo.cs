

using System;
using System.Collections.Generic;
using EryfitProxy.Kernel.Rules.Filters;

namespace EryfitProxy.Kernel.Core.Breakpoints
{
    public class BreakPointContextInfo
    {
        private readonly IDictionary<BreakPointLocation, IBreakPointAlterationModel> _currentModels;

        public BreakPointContextInfo(
            IDictionary<BreakPointLocation, IBreakPointAlterationModel> currentModels,
            ExchangeInfo exchange,
            ConnectionInfo? connectionInfo,
            bool exchangeComplete,
            BreakPointLocation lastLocation,
            BreakPointLocation? currentHit, 
            Filter originFilter)
        {
            _currentModels = currentModels;
            Exchange = exchange;
            ConnectionInfo = connectionInfo;
            LastLocation = lastLocation;
            CurrentHit = currentHit;
            OriginFilter = originFilter;
            Done = LastLocation == BreakPointLocation.Response && CurrentHit == null; 
        }

        public int ExchangeId => Exchange.Id; 

        public ExchangeInfo Exchange { get;  }

        public ConnectionInfo? ConnectionInfo { get; }

        public BreakPointLocation LastLocation { get; }

        public BreakPointLocation? CurrentHit { get; }

        public bool Done { get; }
        
        /// <summary>
        /// The filter that triggers this break point
        /// </summary>
        public Filter OriginFilter { get; }

        public IEnumerable<BreakPointContextStepInfo> StepInfos {
            get
            {
                // enumerate breakpoint locationIndex 

                foreach (BreakPointLocation location in Enum.GetValues(typeof(BreakPointLocation))) {
                    // TODO : minimize the cost of reflection here by using caching 

                    var description = location.GetEnumDescription();

                    if (description == null)
                        continue;

                    var status = CurrentHit == location ? BreakPointStatus.Current
                        : (int) LastLocation >= (int) location ? BreakPointStatus.AlreadyRun
                        : BreakPointStatus.Pending;

                    if (Done) {
                        status = BreakPointStatus.AlreadyRun; 
                    }

                    _currentModels.TryGetValue(location, out var alterationModel); 

                    var stepInfo = new BreakPointContextStepInfo(
                         location, $"{description}", status, alterationModel);

                    yield return stepInfo;
                }
            }
        }
    }

    public enum BreakPointStatus
    {
        AlreadyRun = 1, 
        Current,
        Pending
    }
}
