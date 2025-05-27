using System.Collections.Generic;

namespace EryfitProxy.Kernel.Clipboard
{
    public class CopyableData
    {
        public CopyableData(List<CopyArtefact> artefacts)
        {
            Artefacts = artefacts;
        }

        public List<CopyArtefact> Artefacts { get; }
    }
}