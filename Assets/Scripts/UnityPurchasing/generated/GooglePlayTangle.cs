// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("2+VyGZTqu4MwXqSk1dI4oatlZYMR0rC2zjE91ePDF+ZCo8052TBdn5Q79O5xNWW9lRTDE2V6JK+1Fqy1oROQs6Gcl5i7F9kXZpyQkJCUkZLxOjFgogny4nXBCr1j9RB+VlGV9jJBIbJ7KOIiOmiNBe0irOod/QdU8JaFm7wMaRTbes3JaqxxhtOt+UyxXkIqm7VWAPctFV0n9dAJoB0zahOQnpGhE5CbkxOQkJE0CV0SPtfQWmqZf55fDFkF9nn5obC155FG/ThrNXbkpnrfOFMKtb7BirqLwQpsoYxQyLy/5W9pNjt31ERkkseEV1kboEKLw+qSLPY3n1hF7OY+FKOp7vj0bzNpidQuYWdx6ctMla+7ruWtGfFaLBrjH5L/9pOSkJGQ");
        private static int[] order = new int[] { 8,10,5,8,11,11,9,9,10,13,13,13,13,13,14 };
        private static int key = 145;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
