using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CRRD.Resources.Activities
{
    /// <summary>
    /// Android Activity: Used to move to ErrorActivity should necessary data not be available to the app.
    /// </summary>
    /// <seealso cref="Android.App.Activity" />
    public class ErrorCheckActivity : Activity
    {
        
        /// <summary>
        /// Moves to AppErrorActivity if XMLHandler is invalid
        /// </summary>
        /// <param name="context">The calling context.</param>
        /// <param name="handlerIsInitialized">The value of isInitialed, which indicates if the Category and Business lists have been set.</param>
        public static void checkXMLHandlerInitialization(Context context, Boolean handlerIsInitialized)
        {
            if (!handlerIsInitialized)
            {
                    
                var intent = new Intent(context, typeof(ErrorActivity));
                intent.SetFlags(ActivityFlags.NewTask);
                intent.PutExtra("errorMessage", context.GetString(Resource.String.errorMissingData));
                context.StartActivity(intent);
            }
        }
        
    }
}