using System.Security.Cryptography;

namespace GoodRandom
{
    public class GoodRandomGenerator
    {
        private IRandomProvider provider;

        public GoodRandomGenerator(IRandomProvider provider)
        {
            this.provider = provider;
        }

        public int GetDiceRoll(int sides) => GetDiceRolls(sides, 1).First();

        public IEnumerable<int> GetDiceRolls(int sides, int times)
        {
            if (sides <= 1)
            {
                throw new ArgumentException("Number of sides must be greater than 1");
            }

            return GetFairRandomSet(Enumerable.Range(1, sides).ToArray(), times);
        }

        public IEnumerable<T> GetFairRandomSet<T>(T[] values, int times)
        {
            var size = values.Count();
            var neededBytes = NeededBytes(size);
            var highestFairChoice = HighestFairNumber(size);

            for (var i = 0; i < times; i++)
            {
                var fair = GetFairInt(0, size);
                yield return values[(fair % size)];
            }
        }

        public int GetFairInt(int minInclusive, int maxExclusive)
        {
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

        int GetBigEnoughNumber(int neededBytes)
        {
            int n = provider.GetNextByte();
            for (var i = 1; i < neededBytes; i++)
            {
                n = n << 4;
                n = n | provider.GetNextByte();
            }

            return n;
        }

        int HighestFairNumber(int setSize) =>
            (((int)(Math.Pow(2, NeededBytes(setSize) * 8)) - 1) / setSize) * setSize - 1;

        /// <summary>
        /// Calculates the number of bytes needed to maximize the the chance of choosing a fair number with minimum
        /// number of misses while also minimize the number of random bytes needed to be generated.
        /// </summary>
        /// <remarks>
        /// Needs to be enough bytes so that the highest fair number is less than the MaxValue of the combined number of
        /// bytes.
        ///
        /// Also limited the number of bytes to 4 since we currently only allow up to an integer's worth of choices.
        /// </remarks>
        /// <param name="sides">Number of choices that we are rolling for.</param>
        /// <returns>Number of bytes needed to fairly choose a fair random without too many misses. Max 4.</returns>
        protected virtual int NeededBytes(int sides) => sides < 1 ? 1 : Math.Min((int)(Math.Log2((uint)sides * 2) / 8) + 1, 4);
    }
}