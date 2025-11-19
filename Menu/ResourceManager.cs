using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Infiniminers_v0._0
{
    public class ResourceManager
    {
        private string resourcesPath;
        private string currentResourcePackName = "default";
        private Dictionary<string, Bitmap> loadedTextures = new Dictionary<string, Bitmap>();

        public ResourceManager(string basePath = "Resources")
        {
            resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, basePath);

            Console.WriteLine($"[ResourceManager] Базовый путь: {AppDomain.CurrentDomain.BaseDirectory}");
            Console.WriteLine($"[ResourceManager] Путь к ресурсам: {resourcesPath}");

            if (!Directory.Exists(resourcesPath))
            {
                Console.WriteLine($"[ResourceManager] Папка ресурсов не найдена, создаю...");
                Directory.CreateDirectory(resourcesPath);
                CreateDefaultStructure();
            }

            LoadResourcePack(currentResourcePackName);
        }

        private void CreateDefaultStructure()
        {
            string defaultPath = Path.Combine(resourcesPath, "default");
            Directory.CreateDirectory(Path.Combine(defaultPath, "textures", "player"));
            Directory.CreateDirectory(Path.Combine(defaultPath, "textures", "ores"));
            Directory.CreateDirectory(Path.Combine(defaultPath, "textures", "background"));
            Directory.CreateDirectory(Path.Combine(defaultPath, "textures", "ui"));
            Console.WriteLine("[ResourceManager] Структура папок создана.");
        }

        public void LoadResourcePack(string packName)
        {
            currentResourcePackName = packName;
            loadedTextures.Clear();

            string packPath = Path.Combine(resourcesPath, packName);
            Console.WriteLine($"[ResourceManager] Загружаю ресурс-пак: {packName}");
            Console.WriteLine($"[ResourceManager] Путь пака: {packPath}");

            if (!Directory.Exists(packPath))
            {
                Console.WriteLine($"[ResourceManager] ОШИБКА: Ресурс-пак '{packName}' не найден!");
                packPath = Path.Combine(resourcesPath, "default");
            }

            LoadTexturesFromDirectory(packPath);
            Console.WriteLine($"[ResourceManager] Всего загружено текстур: {loadedTextures.Count}");
        }

        private void LoadTexturesFromDirectory(string packPath)
        {
            string texturesPath = Path.Combine(packPath, "textures");
            Console.WriteLine($"[ResourceManager] Поиск текстур в: {texturesPath}");

            if (!Directory.Exists(texturesPath))
            {
                Console.WriteLine($"[ResourceManager] ОШИБКА: Папка textures не найдена!");
                return;
            }

            var imageExtensions = new[] { ".png", ".jpg", ".bmp" };

            foreach (var file in Directory.GetFiles(texturesPath, "*.*", SearchOption.AllDirectories))
            {
                if (imageExtensions.Contains(Path.GetExtension(file).ToLower()))
                {
                    try
                    {
                        string relativePath = file.Substring(texturesPath.Length + 1);
                        string fullKey = relativePath.Replace("\\", "/").Replace(Path.GetExtension(file), "");

                        var bitmap = new Bitmap(file);
                        loadedTextures[fullKey] = bitmap;
                        Console.WriteLine($"[ResourceManager] ✓ Загружена текстура: {fullKey} ({bitmap.Width}x{bitmap.Height})");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ResourceManager] ✗ ОШИБКА при загрузке {file}: {ex.Message}");
                    }
                }
            }
        }

        public Bitmap GetTexture(string textureName)
        {
            if (loadedTextures.TryGetValue(textureName, out var texture))
            {
                return texture;
            }

            Console.WriteLine($"[ResourceManager] ✗ Текстура '{textureName}' не найдена!");
            Console.WriteLine($"[ResourceManager] Доступные текстуры: {string.Join(", ", loadedTextures.Keys)}");
            return null;
        }

        public List<string> GetAvailableResourcePacks()
        {
            var packsPath = Path.Combine(resourcesPath, "resourcepacks");
            if (!Directory.Exists(packsPath))
                Directory.CreateDirectory(packsPath);

            var packs = new List<string> { "default" };
            packs.AddRange(Directory.GetDirectories(resourcesPath)
                .Where(d => Path.GetFileName(d) != "resourcepacks")
                .Select(d => Path.GetFileName(d)));

            return packs;
        }

        public string GetCurrentResourcePackName()
        {
            return currentResourcePackName;
        }

        public void Dispose()
        {
            foreach (var texture in loadedTextures.Values)
            {
                texture?.Dispose();
            }
            loadedTextures.Clear();
        }
    }
}
