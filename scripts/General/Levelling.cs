using System;

public static class Levelling {
    public static int LevelFromXp(int totalXp)
            => (int)Math.Floor(Math.Sqrt(totalXp / 100.0)) + 1;

        public static int XpAtLevelStart(int level)
            => 100 * (level - 1) * (level - 1);

        public static (int level, int xpIntoLevel, int xpNeeded) FromTotalXp(int totalXp)
        {
            int level = LevelFromXp(totalXp);
            int start = XpAtLevelStart(level);
            int next = XpAtLevelStart(level + 1);
            return (level, totalXp - start, next - start);
        }
}
