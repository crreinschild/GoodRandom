using System.Security.Cryptography;

namespace GoodRandom 
{
    public class GoodRandomGenerator 
    {
        private IRandomProvider provider;
        public GoodRandomGenerator(IRandomProvider provider) {
            this.provider = provider;
        }

        public int GetDiceRoll(int sides) => GetDiceRolls(sides, 1).First();

        public IEnumerable<int> GetDiceRolls(int sides, int times, int batchSize = 0) {
            if (sides <= 1)
            {
                throw new ArgumentException("Number of sides must be greater than 1");
            }
            
            return GetFairRandomSet(Enumerable.Range(1, sides).ToArray(), times, batchSize);
        }

        public IEnumerable<T> GetFairRandomSet<T>(T[] values, int times, int batchSize = 0) {
            var size = values.Count();
            var neededBytes = NeededBytes(size);
            var highestFairChoice = HighestFairNumber(size);

            for (var i = 0; i < times; i++) {
                var fair = GetFairInt(0, size);
                yield return values[(fair % size)];
            }
        }

        public int GetFairInt(int minInclusive, int maxExclusive) {
            var size = maxExclusive - minInclusive;
            
            var neededBytes = NeededBytes(size);
            var highestFairChoice = HighestFairNumber(size);
            int choice;
            do
            {
                choice = GetBigEnoughNumber(neededBytes);
            } while (choice > highestFairChoice);
            return choice;
        }

        int GetBigEnoughNumber(int neededBytes) {
            int n = provider.GetNextByte();
            for (var i = 1; i < neededBytes; i++)
            {
                n = n << 4;
                n = n | provider.GetNextByte();
            }
            return n;
        }

        int HighestFairNumber (int setSize) => (((int)(Math.Pow(2, NeededBytes(setSize) * 8)) - 1) / setSize) * setSize - 1;
        int NeededBytes(int sides) => (int)(Math.Log2(sides) / 8) + 1;

    }
}