using System;
using System.IO;
using System.Threading.Tasks;
using Awari.Droid.Persistence;
using Awari.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDataAccess))]
namespace Awari.Droid.Persistence
{
    public class AndroidDataAccess : IAwariDataAccess
    {
        /// <summary>
        /// Fájl betöltése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <returns>A beolvasott mezőértékek.</returns>
        public async Task<AwariTable> LoadAsync(String path)
        {
            // a betöltés a személyen könyvtárból történik
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // a fájlműveletet taszk segítségével végezzük (aszinkron módon)
            String[] values = (await Task.Run(() => File.ReadAllText(filePath))).Split(' ');

            Int32 nn = Int32.Parse(values[0]);
            AwariTable table = new AwariTable(nn); // létrehozzuk a táblát

            for (int i = 0; i <= (table.NNumber + 1); i++)
            {
                table.SetValue(i, int.Parse(values[i + 1]));
            }
            return table;
        }

        /// <summary>
        /// Fájl mentése.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        public async Task SaveAsync(String path, AwariTable table)
        {
            String text = table.NNumber.ToString() + " "; // méret

            for (int i = 0; i <= table.NNumber + 1; i++)
            {
                text += table.GetValue(i).ToString() + " ";
            }
            // fájl létrehozása
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // kiírás (aszinkron módon)
            await Task.Run(() => File.WriteAllText(filePath, text));
        }
    }
}