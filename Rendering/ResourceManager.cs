using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Infiniminers
{
    /// <summary>
    /// Менеджер ресурсов: загрузка текстур, управление ресурс-паками.
    /// </summary>
    public class ResourceManager : IDisposable
    {
        private string resourcesPath;
        private string currentPackName = "default";
        private Dictionary<string, Bitmap> loadedTextures = new Dictionary<string, Bitmap>();
        private Dictionary<string, Bitmap> defaultTextures = new Dictionary<string, Bitmap>();
        private List<string> availablePacks = new List<string>();
        private bool debugMode = false;
        private bool disposed = false;

        // Константы
        private static readonly string[] SUPPORTED_EXTENSIONS = { ".png", ".jpg", ".bmp", ".gif" };
        private const string DEFAULT_PACK_NAME = "default";
        private const string PACKS_FOLDER = "packs";

        public ResourceManager(string basePath = "Assets")
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectRoot = Path.Combine(exePath, "..", "..", "..");
            resourcesPath = Path.Combine(projectRoot, basePath);
            resourcesPath = Path.GetFullPath(resourcesPath);

            Log($"Путь к ресурсам: {resourcesPath}");

            if (!Directory.Exists(resourcesPath))
            {
                Directory.CreateDirectory(resourcesPath);
                CreateDefaultStructure();
            }

            // Загружаем дефолтный набор ресурсов
            LoadDefaultTextures();

            // Ищем все доступные паки
            ScanAvailablePacks();
            LoadResourcePack(DEFAULT_PACK_NAME);
        }

        private void LoadDefaultTextures()
        {
            Log("Загружаю дефолтные текстуры...");

            string defaultPath = Path.Combine(resourcesPath, DEFAULT_PACK_NAME);
            if (Directory.Exists(defaultPath))
                LoadTexturesFromDirectoryToDict(defaultPath, defaultTextures);

            Log($"Дефолтных текстур загружено: {defaultTextures.Count}");
        }

        private void ScanAvailablePacks()
        {
            availablePacks.Clear();
            availablePacks.Add(DEFAULT_PACK_NAME);

            string packsPath = Path.Combine(resourcesPath, PACKS_FOLDER);
            if (Directory.Exists(packsPath))
            {
                var dirs = Directory.GetDirectories(packsPath);
                foreach (var dir in dirs)
                {
                    string packName = Path.GetFileName(dir);
                    availablePacks.Add(packName);
                    Log($"Найден пак: {packName}");
                }
            }

            Log($"Всего паков: {availablePacks.Count}");
        }

        private void CreateDefaultStructure()
        {
            try
            {
                Directory.CreateDirectory(Path.Combine(resourcesPath, DEFAULT_PACK_NAME, "ores"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, DEFAULT_PACK_NAME, "player"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, DEFAULT_PACK_NAME, "background"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, DEFAULT_PACK_NAME, "ui"));
                Directory.CreateDirectory(Path.Combine(resourcesPath, PACKS_FOLDER));
                Log("Структура папок создана.");
            }
            catch (Exception ex)
            {
                Log($"✗ Ошибка при создании структуры: {ex.Message}");
            }
        }

        public void LoadResourcePack(string packName)
        {
            if (!availablePacks.Contains(packName))
            {
                Log($"✗ Пак '{packName}' не существует!");
                packName = DEFAULT_PACK_NAME;
            }

            currentPackName = packName;
            loadedTextures.Clear();

            string packPath = GetPackPath(packName);

            Log($"Загружаю пак: {packName}");
            Log($"Путь: {packPath}");

            if (Directory.Exists(packPath))
                LoadTexturesFromDirectoryToDict(packPath, loadedTextures);
            else
                Log($"✗ Папка пака не найдена!");

            Log($"Загружено текстур из пака: {loadedTextures.Count}");
        }

        private string GetPackPath(string packName)
        {
            return packName == DEFAULT_PACK_NAME
                ? Path.Combine(resourcesPath, DEFAULT_PACK_NAME)
                : Path.Combine(resourcesPath, PACKS_FOLDER, packName);
        }

        private void LoadTexturesFromDirectoryToDict(string packPath, Dictionary<string, Bitmap> targetDict)
        {
            try
            {
                foreach (var file in Directory.GetFiles(packPath, "*.*", SearchOption.AllDirectories))
                {
                    string extension = Path.GetExtension(file).ToLower();

                    if (SUPPORTED_EXTENSIONS.Contains(extension))
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

                            Log($"✓ {key}");
                        }
                        catch (Exception ex)
                        {
                            Log($"✗ Ошибка загрузки файла: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"✗ Ошибка при сканировании папки: {ex.Message}");
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
                Log($"Текстура '{textureName}' не найдена в паке '{currentPackName}', используется дефолтная");
                return defaultTexture;
            }

            Log($"✗ Текстура '{textureName}' не найдена!");
            return null;
        }

        public string GetCurrentPackName() => currentPackName;

        public List<string> GetAvailablePacks()
        {
            ScanAvailablePacks();
            return new List<string>(availablePacks);
        }

        public void SetDebugMode(bool enabled) => debugMode = enabled;

        private void Log(string message)
        {
            if (debugMode)
                Console.WriteLine($"[ResourceManager] {message}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                foreach (var texture in loadedTextures.Values)
                    texture?.Dispose();
                foreach (var texture in defaultTextures.Values)
                    texture?.Dispose();

                loadedTextures.Clear();
                defaultTextures.Clear();
            }

            disposed = true;
        }

        ~ResourceManager()
        {
            Dispose(false);
        }
    }
}
