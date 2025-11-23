using System;
using System.Collections.Generic;

namespace Infiniminers
{
    /// <summary>
    /// Менеджер выбора ресурс-паков: навигация и загрузка паков.
    /// </summary>
    public class ResourcePackManager
    {
        private ResourceManager resourceManager;
        private int selectedPackIndex = 0;

        public ResourcePackManager(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
            SyncSelectedPackIndex();
        }

        public List<string> GetAvailablePacks() => resourceManager.GetAvailablePacks();

        public string GetCurrentPack() => resourceManager.GetCurrentPackName();

        /// <summary>
        /// Синхронизирует индекс выбора с текущим паком ResourceManager.
        /// </summary>
        private void SyncSelectedPackIndex()
        {
            var packs = GetAvailablePacks();
            string currentPack = GetCurrentPack();
            selectedPackIndex = packs.IndexOf(currentPack);

            // Если пака нет в списке (не должно быть), выставляем 0
            if (selectedPackIndex < 0)
                selectedPackIndex = 0;
        }

        public void SelectPack(int index)
        {
            var packs = GetAvailablePacks();
            if (index >= 0 && index < packs.Count)
            {
                selectedPackIndex = index;
                resourceManager.LoadResourcePack(packs[index]);
            }
        }

        public void NextPack()
        {
            var packs = GetAvailablePacks();
            if (packs.Count <= 0) return;

            selectedPackIndex = (selectedPackIndex + 1) % packs.Count;
            SelectPack(selectedPackIndex);
        }

        public void PreviousPack()
        {
            var packs = GetAvailablePacks();
            if (packs.Count <= 0) return;

            selectedPackIndex = (selectedPackIndex - 1 + packs.Count) % packs.Count;
            SelectPack(selectedPackIndex);
        }

        public int GetSelectedPackIndex()
        {
            // Синхронизируем индекс перед возвратом
            SyncSelectedPackIndex();
            return selectedPackIndex;
        }

        public string GetSelectedPackName()
        {
            var packs = GetAvailablePacks();
            if (selectedPackIndex >= 0 && selectedPackIndex < packs.Count)
                return packs[selectedPackIndex];
            return "default";
        }
    }
}
