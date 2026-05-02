using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TimeSeriesWPF
{
    public static class UserRepository
    {
        private static readonly string FilePath = GetFilePath();

        private static string GetFilePath()
        {
            // Получаем путь к папке с exe файлом
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(baseDir, "users.txt");

            Console.WriteLine($"[UserRepository] Путь к файлу: {filePath}");

            return filePath;
        }

        public static List<User> LoadAllUsers()
        {
            var users = new List<User>();

            if (!File.Exists(FilePath))
            {
                Console.WriteLine("[UserRepository] Файл не найден, создаём новый...");
                try
                {
                    File.Create(FilePath).Close();
                    Console.WriteLine("[UserRepository] Файл создан успешно");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[UserRepository] Ошибка создания файла: {ex.Message}");
                    throw;
                }
                return users;
            }

            try
            {
                var lines = File.ReadAllLines(FilePath);
                Console.WriteLine($"[UserRepository] Загружено {lines.Length} строк из файла");

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parts = line.Split(';');
                    if (parts.Length == 3)
                    {
                        users.Add(new User
                        {
                            Username = parts[0],
                            Password = parts[1],
                            Role = parts[2]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserRepository] Ошибка чтения файла: {ex.Message}");
                throw;
            }

            return users;
        }

        public static void SaveUser(User user)
        {
            Console.WriteLine($"[UserRepository] Попытка сохранить пользователя: {user.Username}");

            var users = LoadAllUsers();
            if (users.Any(u => u.Username == user.Username))
            {
                throw new Exception("Пользователь уже существует");
            }

            var line = $"{user.Username};{user.Password};{user.Role}";
            File.AppendAllText(FilePath, line + Environment.NewLine);

            Console.WriteLine($"[UserRepository] Пользователь сохранён в файл: {FilePath}");
        }

        public static User ValidateLogin(string username, string password)
        {
            var users = LoadAllUsers();
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}