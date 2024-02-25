namespace APIClinic.Helper
{
    public class GarbageCollector
    {
        public static void GarbageCollection()
        {
            try
            {
                GC.Collect();
            }
            catch (Exception ex)
            {

            }

        }

        public static void GarbageCollectionFinalizer()
        {
            try
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
