// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("FXNgflnpjPE+nygsj0mUYzZIHKlUu6fPflCz5RLI8LjCEDXsRfjWj7+PfJp7uum84BOcHERVUAJ0oxjdRPZ1VkR5cn1e8jzyg3l1dXVxdHdFp24mD3fJE9J6vaAJA9vxRkwLHXHeEQuU0IBYcPEm9oCfwUpQ80lQ16TEV57NB8ffjWjgCMdJD/gY4rGO0JMBQ5863bbvUFskb19uJO+JRD4Al/xxD15m1btBQTA33UROgIBm9DdVUyvU2DAGJvIDp0Yo3DzVuHoRitaMbDHLhIKUDC6pcEpeSwBI/BTf1IVH7BcHkCTvWIYQ9ZuztHATabUtWVoAiozT3pIxoYF3ImGyvP72dXt0RPZ1fnb2dXV00ey499syNRS/yf8G+ncaE3Z3dXR1");
        private static int[] order = new int[] { 9,6,13,9,13,5,13,7,8,10,12,11,12,13,14 };
        private static int key = 116;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
