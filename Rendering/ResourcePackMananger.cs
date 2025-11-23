using System;
using System.Collections.Generic;
using System.IO;

namespace Infiniminers
{
    public class ResourcePackManager
    {
        private ResourceManager resourceManager;
        private int selectedPackIndex = 0;

        public ResourcePackManager(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        public List<string> GetAvailablePacks() => resourceManager.GetAvailablePacks();

        public string GetCurrentPack() => resourceManager.GetCurrentPackName();

        public void SelectPack(int index)
        {
            var packs = GetAvailablePacks();
            if (index >= 0 && index < packs.Count)
            {
                selectedPackIndex = index;
                resourceManager.LoadResourcePack(packs[index]);
                Console.WriteLine($"[PackManager] Выбран пак: {packs[index]}");
            }
        }

        public void NextPack()
        {
            var packs = GetAvailablePacks();
            selectedPackIndex = (selectedPackIndex + 1) % packs.Count;
            SelectPack(selectedPackIndex);
        }

        public void PreviousPack()
        {
            var packs = GetAvailablePacks();
            selectedPackIndex = (selectedPackIndex - 1 + packs.Count) % packs.Count;
            SelectPack(selectedPackIndex);
        }

        public int GetSelectedPackIndex() => selectedPackIndex;
    }
}
