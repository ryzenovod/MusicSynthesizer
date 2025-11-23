using System;
using System.IO;

public class Solution
{
    public static void Main()
    {
        // читаем все из input.txt
        string text = File.ReadAllText("input.txt");
        string[] parts = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

        long[] limits = new long[7];
        for (int i = 0; i < 7; i++)
            limits[i] = long.Parse(parts[i]);

        long answer = Solve(limits);

        // выводим в output.txt
        File.WriteAllText("output.txt", answer.ToString());
    }

    // Быстрое решение для 7 клавиш
    private static long Solve(long[] limits)
    {
        const int totalNotes = 7;

        // если первая нота уже с нулевым лимитом – играть нечем
        if (limits[0] <= 0)
            return 0;

        // первое нажатие по позиции 0
        limits[0]--;
        long tacts = 1;

        // период последовательности индексов (со 2-го нажатия)
        int[] cycle = new int[]
        {
            2, 3, 5, 1, 6, 0, 6, 6, 5, 4, 2, 6, 1, 0, 1, 1
        };
        int period = cycle.Length;

        // сколько раз за один период нажимается каждая клавиша
        long[] freq = new long[totalNotes];
        for (int i = 0; i < period; i++)
            freq[cycle[i]]++;

        // максимальное количество целых периодов
        long maxFullCycles = long.MaxValue;
        for (int i = 0; i < totalNotes; i++)
        {
            if (freq[i] > 0)
            {
                long possible = limits[i] / freq[i];
                if (possible < maxFullCycles)
                    maxFullCycles = possible;
            }
        }
        if (maxFullCycles == long.MaxValue)
            maxFullCycles = 0;

        // применяем полные периоды
        if (maxFullCycles > 0)
        {
            tacts += maxFullCycles * period;
            for (int i = 0; i < totalNotes; i++)
                limits[i] -= freq[i] * maxFullCycles;
        }

        // хвост: добегаем по циклу, пока хватает лимитов
        for (int i = 0; i < period; i++)
        {
            int note = cycle[i];
            if (limits[note] == 0)
                break;

            limits[note]--;
            tacts++;
        }

        return tacts;
    }
}
