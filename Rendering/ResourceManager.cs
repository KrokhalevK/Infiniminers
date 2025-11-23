using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Infiniminers
{
    public class ResourceManager
    {
        private string resourcesPath;
        private string currentPackName = "default";
        private Dictionary<string, Bitmap> loadedTextures = new Dictionary<string, Bitmap>();
        private Dictionary<string, Bitmap> defaultTextures = new Dictionary<string, Bitmap>();  // ← ДЕФОЛТ
        private List<string> availablePacks = new List<string>();

        public ResourceManager(string basePath = "Assets")
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.Combine(exePath, "..", "..", "..");
            resourcesPath = Path.Combine(projectRoot, basePath);
            resourcesPath = Path.GetFullPath(resourcesPath);

            Console.WriteLine($"[ResourceManager] Путь к ресурсам: {resourcesPath}");

            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
                CreateDefaultStructure();
            }

            // Загружаем дефолтный набор ресурсов
            LoadDefaultTextures();

            // Ищем все доступные паки
            ScanAvailablePacks();
            LoadResourcePack("default");
        }

        private void LoadDefaultTextures()
        {
            Console.WriteLine("[ResourceManager] Загружаю дефолтные текстуры...");

            string defaultPath = Path.Combine(resourcesPath, "default");
            if (Directory.Exists(defaultPath))
                LoadTexturesFromDirectoryToDict(defaultPath, defaultTextures);

            Console.WriteLine($"[ResourceManager] Дефолтных текстур загружено: {defaultTextures.Count}");
        }

        private void ScanAvailablePacks()
        {
            availablePacks.Clear();
            availablePacks.Add("default");

            string packsPath = Path.Combine(resourcesPath, "packs");
            if (Directory.Exists(packsPath))
            {
                var dirs = Directory.GetDirectories(packsPath);
                foreach (var dir in dirs)
                {
                    string packName = Path.GetFileName(dir);
                    availablePacks.Add(packName);
                    Console.WriteLine($"[ResourceManager] Найден пак: {packName}");
                }
            }

            Console.WriteLine($"[ResourceManager] Всего паков: {availablePacks.Count}");
        }

        private void CreateDefaultStructure()
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(resourcesPath, "default", "ores"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, "default", "player"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, "default", "backgrounds"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, "default", "ui"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, "packs"));
                Console.WriteLine("[ResourceManager] Структура папок создана.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ResourceManager] Ошибка при создании структуры: {ex.Message}");
            }
        }

        public void LoadResourcePack(string packName)
        {
            if (!availablePacks.Contains(packName))
            {
                Console.WriteLine($"[ResourceManager] ✗ Пак '{packName}' не существует!");
                packName = "default";
            }

            currentPackName = packName;
            loadedTextures.Clear();

            string packPath;
            if (packName == "default")
                packPath = Path.Combine(resourcesPath, "default");
            else
                packPath = Path.Combine(resourcesPath, "packs", packName);

            Console.WriteLine($"[ResourceManager] Загружаю пак: {packName}");
            Console.WriteLine($"[ResourceManager] Путь: {packPath}");

            if (Directory.Exists(packPath))
                LoadTexturesFromDirectoryToDict(packPath, loadedTextures);
            else
                Console.WriteLine($"[ResourceManager] ✗ Папка пака не найдена!");

            Console.WriteLine($"[ResourceManager] Загружено текстур из пака: {loadedTextures.Count}");
        }

        private void LoadTexturesFromDirectoryToDict(string packPath, Dictionary<string, Bitmap> targetDict)
        {
            var imageExtensions = new[] { ".png", ".jpg", ".bmp", ".gif" };

            try
            {
                foreach (var file in Directory.GetFiles(packPath, "*.*", SearchOption.AllDirectories))
                {
                    string extension = Path.GetExtension(file).ToLower();

                    if (imageExtensions.Contains(extension))
                    {
                        try
                        {
                            string relativePath = file.Substring(packPath.Length + 1);
                            string key = relativePath
                                .Replace("\\", "/")
                                .Replace(extension, "")
                                .ToLower();

                            var bitmap = new Bitmap(file);
                            targetDict[key] = bitmap;

                            Console.WriteLine($"[ResourceManager] ✓ {key}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[ResourceManager] ✗ Ошибка: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ResourceManager] ✗ Ошибка при загрузке: {ex.Message}");
            }
        }

        public Bitmap? GetTexture(string textureName)
        {
            if (string.IsNullOrEmpty(textureName))
                return null;

            string key = textureName.ToLower();

            // Сначала ищем в текущем паке
            if (loadedTextures.TryGetValue(key, out var texture))
                return texture;

            // Если не найдено, ищем в дефолтном паке
            if (defaultTextures.TryGetValue(key, out var defaultTexture))
            {
                Console.WriteLine($"[ResourceManager] Текстура '{textureName}' не найдена в паке '{currentPackName}', используется дефолтная");
                return defaultTexture;
            }

            Console.WriteLine($"[ResourceManager] ✗ Текстура '{textureName}' не найдена ни в каком паке!");
            return null;
        }

        public string GetCurrentPackName() => currentPackName;

        public List<string> GetAvailablePacks()
        {
            ScanAvailablePacks();
            return new List<string>(availablePacks);
        }

        public void Dispose()
        {
            foreach (var texture in loadedTextures.Values)
                texture?.Dispose();
            foreach (var texture in defaultTextures.Values)
                texture?.Dispose();

            loadedTextures.Clear();
            defaultTextures.Clear();
        }
    }
}
