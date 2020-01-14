using System;
using System.IO;
using Android.App;


namespace YPA.Droid
{
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            //_xx_ string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbPath = Path.Combine(path, filename);

            System.Console.WriteLine("DEBUG - GetLocalFilePath: filename: {0}   path: {1}", filename, path);

            //_xx_ System.Console.WriteLine("DEBUG - NO SE COPIA la DB3!! Está comentado !!");
            CopyDatabaseIfNotExists(dbPath, filename);

            return dbPath;
        }

        private static void CopyDatabaseIfNotExists(string dbPath, string filename)
        {
            if (!File.Exists(dbPath))
            {
                System.Console.WriteLine("DEBUG - CopyDatabaseIfNotExists: Existe {0}", dbPath);
                using (var br = new BinaryReader(Application.Context.Assets.Open(filename)))
                {
                    using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.Create)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, length);
                        }
                    }
                }
            }
            else
                System.Console.WriteLine("DEBUG - CopyDatabaseIfNotExists: No existe {0}", dbPath);
        }
    }
}
