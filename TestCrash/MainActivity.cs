using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace TestCrash
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            MrCrashAdapter mrCrashAdapter = new MrCrashAdapter();
            bool hasStableIds = mrCrashAdapter.HasStableIds; // this works
            System.Diagnostics.Debug.WriteLine(hasStableIds);

            // this crash with this packages configuration
            mrCrashAdapter.HasStableIds = true;
            // this works though (workaround)
            mrCrashAdapter.SetHasStableIds(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

       
    }

    class MrCrashAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return null;
        }

        IntPtr pointerToSetHasStableId = IntPtr.Zero;
        IntPtr class_ref = IntPtr.Zero;

        [Register("setHasStableIds", "(Z)V", "")]
        public virtual unsafe void SetHasStableIds(bool hasStableIds)
        {
            if (pointerToSetHasStableId == IntPtr.Zero)
            {
                class_ref = JNIEnv.FindClass(typeof(RecyclerView.Adapter));
                pointerToSetHasStableId = JNIEnv.GetMethodID(class_ref, "setHasStableIds", "(Z)V");
            }
            try
            {
                JValue* __args = stackalloc JValue[1];
                __args[0] = new JValue(hasStableIds);
                JNIEnv.CallVoidMethod(this.Handle, pointerToSetHasStableId, __args);
            }
            finally
            {
            }
        }

    }
}
