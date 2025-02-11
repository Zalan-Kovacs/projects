using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escape.Persistence
{
    public class EscapeFileDataAccess : IEscapeDataAccess
    {
        public async Task<EscapeTable> LoadAsync(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line = await reader.ReadLineAsync() ?? String.Empty;
                    String[] numbers = line.Split(' ');
                    int tableSize = int.Parse(numbers[0]);
                    EscapeTable table = new EscapeTable(tableSize);

                    for(int i = 0; i < tableSize; i++)
                    {
                        line = await reader.ReadLineAsync() ?? String.Empty;
                        numbers = line.Split(' ');

                        for(int j = 0; j < tableSize; j++)
                        {
                            table.SetValue(i, j, int.Parse(numbers[j]), "start");
                        }
                    }
                return table;
                }
            }
            catch
            {
                throw new Exception();
            }
        }
        public async Task SaveAsync(string path, EscapeTable table)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteLineAsync(table.Size.ToString());

                    for (int i = 0; i < table.Size; i++)
                    {
                        for (int j = 0; j < table.Size; j++)
                        {
                            await writer.WriteAsync(table[i, j] + " ");
                        }
                        await writer.WriteLineAsync();
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
