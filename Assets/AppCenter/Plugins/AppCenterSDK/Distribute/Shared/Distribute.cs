﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Distribute.Internal;

namespace Microsoft.AppCenter.Unity.Distribute
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Distribute
    {
        // Used by App Center Unity Editor Extensions: https://github.com/Microsoft/AppCenter-SDK-Unity-Extension
        public const string DistributeSDKVersion = "2.3.0";

        public static void PrepareEventHandlers()
        {
            DistributeInternal.PrepareEventHandlers();
        }

        public static void AddNativeType(List<RawType> nativeTypes)
        {
            DistributeInternal.AddNativeType(nativeTypes);
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return DistributeInternal.IsEnabledAsync();
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
        {
            return DistributeInternal.SetEnabledAsync(enabled);
        }

        /// <summary>
        /// Change the base URL opened in the browser to get update token from user login information.
        /// </summary>
        /// <param name="installUrl">Install base URL.</param>
        public static void SetInstallUrl(string installUrl)
        {
            DistributeInternal.SetInstallUrl(installUrl);
        }

        /// <summary>
        /// Change the base URL used to make API calls.
        /// </summary>
        /// <param name="apiUrl">API base URL.</param>
        public static void SetApiUrl(string apiUrl)
        {
            DistributeInternal.SetApiUrl(apiUrl);
        }

        /// <summary>
        /// Sets the release available callback.
        /// </summary>
        /// <value>The release available callback.</value>
        public static ReleaseAvailableCallback ReleaseAvailable
        {
            get; set;
        }

        /// <summary>
        /// If update dialog is customized by returning <c>true</c> in <see cref="ReleaseAvailableCallback"/>,
        /// You need to tell the distribute SDK using this function what is the user action.
        /// </summary>
        /// <param name="updateAction">Update action. On mandatory update, you can only pass <see cref="UpdateAction.Update"/></param>
        public static void NotifyUpdateAction(UpdateAction updateAction)
        {
            DistributeInternal.NotifyUpdateAction((int)updateAction);
        }
    }
}
