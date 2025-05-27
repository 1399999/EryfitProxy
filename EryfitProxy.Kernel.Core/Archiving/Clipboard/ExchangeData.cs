using System.Collections.Generic;

namespace EryfitProxy.Kernel.Clipboard
{
    public class ExchangeData : CopyableData
    {
        public ExchangeData(List<CopyArtefact> artefacts)
            : base(artefacts)
        {
        }
    }
}