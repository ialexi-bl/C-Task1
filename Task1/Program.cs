class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter amount: ");
        if (!long.TryParse(Console.ReadLine(), out long amount))
        {
            throw new Exception("Amount must be an integer");
        }

        Console.Write("Enter notes separated by space: ");
        var notes = Console.ReadLine()
            ?.Trim()
            ?.Split(" ")
            ?.Select(x =>
            {
                if (!long.TryParse(x, out long note))
                {
                    throw new Exception("Each note must be an integer");
                }
                return note;
            })
            ?.Where(x => x != 0)
            ?.ToList();

        if (notes is null)
        {
            throw new Exception("No notes provided");
        }

        PrintBanknotes(amount, notes);
    }

    public static void PrintBanknotes(long amount, List<long> notes)
    {
        if (notes.Count == 0)
        {
            throw new ArgumentException("At least one note value must be provided");
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be a positive integer");
        }

        foreach (var note in notes)
        {
            if (note <= 0)
            {
                throw new ArgumentException("All notes must be positive integers");
            }
        }

        var sortedNotes = notes.Distinct().OrderByDescending(n => n).ToList();

        bool hasPrinted = PrintBanknotes(amount, sortedNotes, 0, new List<NotePack>());
        if (!hasPrinted)
        {
            Console.WriteLine("No combinations");
        }
    }

    private static bool PrintBanknotes(long amount, List<long> sortedNotes, int currentIndex, List<NotePack> currentSequence)
    {
        if (amount == 0)
        {
            PrintSequence(currentSequence);
            return true;
        }

        bool hasPrinted = false;
        for (int i = currentIndex; i < sortedNotes.Count; i++)
        {
            var note = sortedNotes[i];

            long count = amount / note;
            var pack = new NotePack(note, count);

            while (pack.count > 0)
            {
                currentSequence.Add(pack);
                hasPrinted = hasPrinted || PrintBanknotes(amount - pack.value * pack.count, sortedNotes, i + 1, currentSequence);
                currentSequence.RemoveAt(currentSequence.Count - 1);

                pack = new NotePack(pack.value, pack.count - 1);
            };
        }
        return hasPrinted;
    }

    private static void PrintSequence(List<NotePack> sequence)
    {
        sequence.ForEach(pack =>
        {
            Console.Write(pack);
            Console.Write(" ");
        });
        Console.WriteLine();
    }

    private record NotePack(long value, long count)
    {
        override public String ToString()
        {
            return value + " (x" + count + ")";
        }
    }
}